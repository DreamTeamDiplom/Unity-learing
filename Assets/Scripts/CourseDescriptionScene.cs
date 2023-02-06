using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CourseDescriptionScene : MonoBehaviour
{
    [SerializeField] private GameObject icon;
    [SerializeField] private Text titleCourse;
    [SerializeField] private Text descriptionCourse;
    [SerializeField] private Image iconCourse;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject prefabLesson;
    [SerializeField] private GameObject button;
    [SerializeField] private ObjectProfiles objectProfiles;
    private Text textButton;

    private void Awake()
    {
        textButton = button.GetComponentInChildren<Text>();

        icon.GetComponentInChildren<Image>().sprite = CurrentProfile.Profile.Icon;
        icon.GetComponentInChildren<Text>().text = CurrentProfile.Profile.Name;

        titleCourse.text = CurrentProfile.CurrentCourse.Course.Title;
        descriptionCourse.text = CurrentProfile.CurrentCourse.Course.Description;
        iconCourse.sprite = CurrentProfile.CurrentCourse.Course.Icon;

        for (int i = 0; i < CurrentProfile.CurrentCourse.Course.Lessons.Count; i++)
        {
            Lesson lesson = CurrentProfile.CurrentCourse.Course.Lessons[i];
            var objLesson = Instantiate(prefabLesson);
            objLesson.GetComponentInChildren<Text>().text = string.Format("Урок {0}. {1}", i + 1, lesson.Title);
            objLesson.transform.SetParent(content.transform, false);
        }

        if (CurrentProfile.Profile.Courses.Exists(x => x.Course.Title.Contains(CurrentProfile.CurrentCourse.Course.Title)))
        {
            var course = CurrentProfile.Profile.Courses.Find(x => x.Course.Title.Contains(CurrentProfile.CurrentCourse.Course.Title));
            if (course.Finished)
                textButton.text = "Просмотреть курс";
            else
                textButton.text = "Продолжить курс";
        }
        else
        {
            textButton.text = "Начать курс";
            Debug.Log(CurrentProfile.CurrentCourse.LastLesson);
            Debug.Log(CurrentProfile.CurrentCourse);
            button.GetComponentInChildren<Button>().onClick.AddListener(() => objectProfiles.AddCourse(CurrentProfile.CurrentCourse));
        }
        button.GetComponentInChildren<Button>().onClick.AddListener(() => SceneManager.LoadScene("Course"));
    }
}
