using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProfileCourses : MonoBehaviour
{
    [SerializeField] private GameObject infoProfile;
    [SerializeField] private GameObject prefabCourse;
    [SerializeField] private GameObject prefabCompleteLine;
    [SerializeField] private GameObject prefabUncompleteLine;
    [SerializeField] private GameObject activeList;
    [SerializeField] private GameObject finishList;

    private Text nameProfile;
    private Image icon;
    // Start is called before the first frame update
    void Awake()
    {
        infoProfile.GetComponentInChildren<Text>().text = CurrentProfile.Profile.Name;
        infoProfile.GetComponentInChildren<Image>().sprite = CurrentProfile.Profile.Icon;

        foreach (var courseProfile in CurrentProfile.Profile.Courses)
        {
            if (!courseProfile.Finished)
            {
                var courseObj = Instantiate(prefabCourse);
                var folderText = courseObj.transform.GetChild(0).GetChild(1);
                //courseObj.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = course.icon;
                StartCoroutine(LoadTextureFromServer(courseObj, courseProfile));
                folderText.GetChild(0).GetComponent<Text>().text = courseProfile.Course.Title;
                var indicator = courseObj.transform.GetChild(1);
                foreach (var lesson in courseProfile.Course.Lessons)
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
                courseObj.GetComponent<Button>().onClick.AddListener(() => {
                    SceneManager.LoadScene("Course");
                    CurrentProfile.CurrentCourse = courseProfile;
                    });
            }
            else
            {
                var courseObj = Instantiate(prefabCourse);
                var folderText = courseObj.transform.GetChild(0).GetChild(1);
                folderText.GetChild(0).GetComponent<Text>().text = courseProfile.Course.Title;
                StartCoroutine(LoadTextureFromServer(courseObj, courseProfile));
                courseObj.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = courseProfile.Course.Icon;
                courseObj.transform.SetParent(finishList.transform, false);
                courseObj.GetComponent<Button>().onClick.AddListener(() => {
                    SceneManager.LoadScene("Course");
                    CurrentProfile.CurrentCourse = courseProfile;
                });
            }
        }

        
        //var line = indicator.transform.GetChild(0);
        /* Цилк перебора */

    }

    private IEnumerator LoadTextureFromServer(GameObject courseObj, ProfileCourse course)
    {
        var request = UnityWebRequestTexture.GetTexture(course.Course.PathIcon);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            var texture = DownloadHandlerTexture.GetContent(request);
            var sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            course.Course.Icon = sprite;
            courseObj.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = course.Course.Icon;
        }
        
        request.Dispose();
    }
}

