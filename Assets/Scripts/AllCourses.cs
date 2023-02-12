using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AllCourses : MonoBehaviour
{
    [Header("Cards Course")]
    [SerializeField] private GameObject cardNotStart;
    [SerializeField] private GameObject cardStart;
    [SerializeField] private GameObject cardComplite;

    [Space(10)]
    [SerializeField] private GameObject icon;
    [SerializeField] private GameObject content;
    [SerializeField] private InputField inputSearch;
    [SerializeField] private ObjectProfiles objectProfiles;
    private readonly List<GameObject> listCourse = new List<GameObject>();
    private void Awake()
    {
        DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine(Application.streamingAssetsPath, "Courses"));
        foreach (var (folder, file) in dirInfo.GetDirectories().SelectMany(folder => folder.GetFiles().Where(file => file.Extension == ".json").Select(file => (folder, file))))
        {
            string jsonString = File.ReadAllText(file.FullName);
            var course = JsonUtility.FromJson<Course>(jsonString);
            string fullPathIconCourse = Path.Combine(folder.FullName, course.PathIcon);
            GameObject objCard = null;
            if (CurrentProfile.Profile.Courses.Exists(x => x.Course.Title.Contains(course.Title)))
            {
                var myCourse = CurrentProfile.Profile.Courses.Find(x => x.Course.Title.Contains(course.Title));
                /* Если курс уже прошли */
                if (myCourse.Finished)
                {
                    objCard = Instantiate(cardComplite);
                    var infoCourse = objCard.transform.GetChild(1);
                    infoCourse.GetChild(0).GetComponentInChildren<Text>().text = course.Title;
                    infoCourse.GetChild(1).GetComponentInChildren<Text>().text = course.Lessons.Count.ToString() + " уроков";
                }
                /* Если курс уже начали */
                else
                {
                    objCard = Instantiate(cardStart);
                    var infoCourse = objCard.transform.GetChild(1);
                    infoCourse.GetChild(0).GetComponentInChildren<Text>().text = course.Title;
                    infoCourse.GetChild(1).GetChild(0).GetComponentInChildren<Text>().text = course.Lessons.Count.ToString() + " уроков";
                    infoCourse.GetChild(1).GetChild(1).GetComponentInChildren<Text>().text = myCourse.Course.Lessons.FindAll(x => x.Finished).Count.ToString() + " уроков";
                }
            }
            /* Если не было ещё такого курса */
            else
            {
                objCard = Instantiate(cardNotStart);
                var infoCourse = objCard.transform.GetChild(1);
                infoCourse.GetChild(0).GetComponentInChildren<Text>().text = course.Title;
                infoCourse.GetChild(1).GetComponentInChildren<Text>().text = course.Lessons.Count.ToString() + " уроков";
                objCard.transform.GetChild(2).GetComponentInChildren<Button>().onClick.AddListener(() => objectProfiles.AddCourse(course));
            }
            StartCoroutine(LoadTextureFromServer(fullPathIconCourse, objCard, course));

            objCard.GetComponent<Button>().onClick.AddListener(() =>
            {
                CurrentProfile.CurrentCourse = new ProfileCourse(course);
                SceneManager.LoadScene("CourseDescription");
            });
            objCard.transform.GetChild(2).GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                CurrentProfile.CurrentCourse = new ProfileCourse(course);
                SceneManager.LoadScene("Course");
            });
            objCard.transform.SetParent(content.transform, false);
            listCourse.Add(objCard);

            }
        inputSearch.onValueChanged.AddListener((value) => FilterCard(value));
    }
    void Start()
    {
        icon.GetComponent<Image>().sprite = CurrentProfile.Profile.Icon;
        icon.GetComponentInChildren<Text>().text = CurrentProfile.Profile.Name;
    }

    private void FilterCard(string value)
    {
        var list = listCourse.FindAll(x => !x.transform.GetChild(1).GetChild(0).GetComponentInChildren<Text>().text.Contains(value, StringComparison.CurrentCultureIgnoreCase));
        listCourse.ForEach(x => x.SetActive(true));
        list.ForEach(x => x.SetActive(false));
    }

    IEnumerator LoadTextureFromServer(string url, GameObject objCard, Course course)
    {
        var request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        var texture = DownloadHandlerTexture.GetContent(request);
        SetSprite(objCard, texture, course);
        request.Dispose();
    }
    private void SetSprite(GameObject objCard, Texture2D texture, Course course)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        course.Icon = sprite;
        objCard.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = sprite;
    }
}
