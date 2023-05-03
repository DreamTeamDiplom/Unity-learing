using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProfileCourses : MonoBehaviour
{
    [SerializeField] private GameObject infoProfile;
    [SerializeField] private GameObject prefabCompleteCourse;
    [SerializeField] private GameObject prefabUncompleteCourse;
    [SerializeField] private GameObject activeList;
    [SerializeField] private GameObject finishList;

    void Awake()
    {
        infoProfile.GetComponentInChildren<Text>().text = CurrentProfile.Profile.Name;
        infoProfile.GetComponentInChildren<Image>().sprite = CurrentProfile.Profile.Icon;

        foreach (var courseProfile in CurrentProfile.Profile.Courses)
        {
            AssetBundle loadedAssetBundle = GetAssetBundle(courseProfile);
            TextAsset jsonFile = loadedAssetBundle.LoadAsset<TextAsset>(courseProfile.Title);

            Course course = JsonUtility.FromJson<Course>(jsonFile.text);
            Sprite iconCourse = loadedAssetBundle.LoadAsset<Sprite>(course.PathIcon);
            course.Icon = iconCourse;
            loadedAssetBundle.Unload(false);
            for (int i = 0; i < course.Lessons.Count; i++)
            {
                course.Lessons[i].Finished = courseProfile.Lessons[i].Finished;
            }

            GameObject gameObjectCourse;
            if (!courseProfile.Finished)
            {
                gameObjectCourse = Instantiate(prefabUncompleteCourse, activeList.transform, false);
                int numberUncompleteLesson = courseProfile.Lessons.FindAll(l => !l.Finished).Count();
                gameObjectCourse.GetComponent<ViewProfileCourse>().Init(courseProfile.Title, iconCourse, numberUncompleteLesson, 
                    courseProfile.Lessons.Count - numberUncompleteLesson, courseProfile.LastLesson.Title);
            }
            else
            {
                gameObjectCourse = Instantiate(prefabCompleteCourse, activeList.transform, false);
                gameObjectCourse.GetComponent<ViewProfileCourse>().Init(course.Title, iconCourse);
            }
            gameObjectCourse.GetComponent<Button>().onClick.AddListener(() => {
                SceneManager.LoadScene("Course");
                CurrentProfile.CurrentCourse = course;
                
            });
        }
    }

    private AssetBundle GetAssetBundle(ProfileCourse courseProfile)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine(Application.streamingAssetsPath, "Courses"));

        FileInfo fileInfo = dirInfo.GetFiles().Where(f => !f.Name.EndsWith(".meta") &&
            f.Name.Contains(courseProfile.Title, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

        if (fileInfo == null)
        {
            /* Обработка незагруженного курса */
            return null;
        }

        return AssetBundle.LoadFromFile(fileInfo.FullName);
    }
}