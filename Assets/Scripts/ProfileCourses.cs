using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProfileCourses : MonoBehaviour
{
    [SerializeField] private GameObject infoProfile;
    [SerializeField] private GameObject prefabCompleteCourse;
    [SerializeField] private GameObject prefabUncompleteCourse;
    [SerializeField] private GameObject prefabCompleteLine;
    [SerializeField] private GameObject prefabUncompleteLine;
    [SerializeField] private GameObject activeList;
    [SerializeField] private GameObject finishList;

    void Awake()
    {
        infoProfile.GetComponentInChildren<Text>().text = CurrentProfile.Profile.Name;
        infoProfile.GetComponentInChildren<Image>().sprite = CurrentProfile.Profile.Icon;

        DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine(Application.streamingAssetsPath, "Courses"));
        var filesCoursesProfile = dirInfo.GetDirectories().SelectMany(folder => folder.GetFiles().Where(file => file.Extension == ".json")).ToList();

        foreach (var courseProfile in CurrentProfile.Profile.Courses)
        {
            Course course = null;
            var file = filesCoursesProfile.Where(f => f.Name == courseProfile.Title + ".json").FirstOrDefault();
            if (file != null)
            {
                string jsonString = File.ReadAllText(file.FullName);
                course = JsonUtility.FromJson<Course>(jsonString);
                for (int i = 0; i < course.Lessons.Count; i++)
                {
                    course.Lessons[i].Finished = courseProfile.Lessons[i].Finished;
                }
            }

            GameObject courseObj;
            if (!courseProfile.Finished)
            {
                courseObj = Instantiate(prefabUncompleteCourse);
                var folderText = courseObj.transform.GetChild(0).GetChild(1);
                StartCoroutine(LoadTextureFromServer(courseObj, course));
                folderText.GetChild(0).GetComponent<Text>().text = courseProfile.Title;
                var indicator = courseObj.transform.GetChild(1);
                foreach (var lesson in courseProfile.Lessons)
                {
                    GameObject lineObj;
                    if (lesson.Finished)
                    {
                        lineObj = Instantiate(prefabCompleteLine);
                    }
                    else
                    {
                        lineObj = Instantiate(prefabUncompleteLine);
                    }
                    lineObj.transform.SetParent(indicator);

                }
                folderText.GetChild(1).GetComponent<Text>().text = courseProfile.LastLesson.Title;

                courseObj.transform.SetParent(activeList.transform, false);
            }
            else
            {
                courseObj = Instantiate(prefabCompleteCourse);
                var folderText = courseObj.transform.GetChild(0).GetChild(1);
                folderText.GetChild(0).GetComponent<Text>().text = courseProfile.Title;
                StartCoroutine(LoadTextureFromServer(courseObj, course));
                courseObj.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = course?.Icon;
                courseObj.transform.SetParent(finishList.transform, false);
            }
            courseObj.GetComponent<Button>().onClick.AddListener(() => {
                SceneManager.LoadScene("Course");
                CurrentProfile.CurrentCourse = course;
                
            });
        }
    }

    private IEnumerator LoadTextureFromServer(GameObject courseObj, Course course)
    {
        var request = UnityWebRequestTexture.GetTexture(course.PathIcon);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            var texture = DownloadHandlerTexture.GetContent(request);
            var sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            course.Icon = sprite;
            courseObj.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = course.Icon;
        }
        request.Dispose();
    }
}

