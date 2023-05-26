using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class AllCourses : MonoBehaviour
{
    [Serializable]
    private class CardsCourse
    {
        public GameObject notStart;
        public GameObject start;
        public GameObject complete;
    }

    [SerializeField] private CardsCourse card;

    [Space(10)]
    [SerializeField] private GameObject infoProfile;
    [SerializeField] private GameObject content;
    [SerializeField] private InputField inputSearch;
    [SerializeField] private ObjectProfiles objectProfiles;

    private readonly List<GameObject> listCourse = new List<GameObject>();
    AssetBundle loadedAssetBundle = null;

    private void Awake()
    {
        DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine(Application.streamingAssetsPath, "Courses"));

        foreach (var fileInfo in dirInfo.GetFiles().Where(f => !f.Name.EndsWith(".meta")))
        {
            loadedAssetBundle = AssetBundle.LoadFromFile(fileInfo.FullName);
            TextAsset jsonFile = loadedAssetBundle.LoadAsset<TextAsset>(fileInfo.Name);

            Course course = JsonUtility.FromJson<Course>(jsonFile.text);
            Sprite icon = loadedAssetBundle.LoadAsset<Sprite>(course.PathIcon);
            course.Icon = icon;
            string fullPathIconCourse = course.PathIcon;
            GameObject gameObjectCourse = null;
            if (CurrentProfile.Profile.Courses.Exists(x => x.Title.Contains(course.Title)))
            {
                var myCourse = CurrentProfile.Profile.Courses.Find(x => x.Title.Contains(course.Title));
                for (int i = 0; i < course.Lessons.Count; i++)
                {
                    course.Lessons[i].Finished = myCourse.Lessons[i].Finished;
                }
                /* Если курс уже прошли */
                if (myCourse.Finished)
                {
                    gameObjectCourse = Instantiate(card.complete, content.transform, false);
                    gameObjectCourse.GetComponent<ViewAllCourse>().Init(course.Title, icon, course.Lessons.Count);
                }
                /* Если курс уже начали */
                else
                {
                    gameObjectCourse = Instantiate(card.start, content.transform, false);
                    int numberIncompleteLesson = course.Lessons.FindAll(x => !x.Finished).Count;
                    gameObjectCourse.GetComponent<ViewAllCourse>().Init(course.Title, icon, numberIncompleteLesson, course.Lessons.Count - numberIncompleteLesson);
                }
            }
            /* Если не было ещё такого курса */
            else
            {
                gameObjectCourse = Instantiate(card.notStart, content.transform, false);
                gameObjectCourse.GetComponent<ViewAllCourse>().Init(course.Title, icon, course.Lessons.Count);
                gameObjectCourse.GetComponent<ViewAllCourse>().AddListener(() => objectProfiles.AddCourse(course));
            }

            gameObjectCourse.GetComponent<Button>().onClick.AddListener(() =>
            {
                CurrentProfile.CurrentCourse = course;
                SceneManager.LoadScene("CourseDescription");
            });
            gameObjectCourse.GetComponent<ViewAllCourse>().AddListener(() => 
            {
                CurrentProfile.CurrentCourse = course;
                SceneManager.LoadScene("Course");
            });

            listCourse.Add(gameObjectCourse);

        }
        inputSearch.onValueChanged.AddListener((value) => FilterCard(value));
        loadedAssetBundle.Unload(false);
    }
    private void Start()
    {
        infoProfile.GetComponentInChildren<Image>().sprite = CurrentProfile.Profile.Icon;
        infoProfile.GetComponentInChildren<Text>().text = CurrentProfile.Profile.Name;
    }

    private void FilterCard(string value)
    {
        var list = listCourse.FindAll(x => !x.GetComponent<ViewAllCourse>().Title.Contains(value, StringComparison.CurrentCultureIgnoreCase));
        listCourse.ForEach(x => x.SetActive(true));
        list.ForEach(x => x.SetActive(false));
    }

    private void OnDestroy()
    {
        if (loadedAssetBundle != null)
        {
            loadedAssetBundle.Unload(false);
        }
    }
}
