using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class CourseDescriptionScene : MonoBehaviour
{
    [SerializeField] private GameObject infoProfile;
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

        infoProfile.GetComponentInChildren<Image>().sprite = CurrentProfile.Profile.Icon;
        infoProfile.GetComponentInChildren<Text>().text = CurrentProfile.Profile.Name;

        titleCourse.text = CurrentProfile.CurrentCourse.Title;
        descriptionCourse.text = CurrentProfile.CurrentCourse.Description;
        if (CurrentProfile.CurrentCourse.Icon != null)
        {
            iconCourse.sprite = CurrentProfile.CurrentCourse.Icon;
        }

        for (int i = 0; i < CurrentProfile.CurrentCourse.Lessons.Count; i++)
        {
            Lesson lesson = CurrentProfile.CurrentCourse.Lessons[i];
            var objLesson = Instantiate(prefabLesson);
            objLesson.GetComponentInChildren<Text>().text = string.Format("Урок {0}. {1}", i + 1, lesson.Title);
            objLesson.transform.SetParent(content.transform, false);
        }

        if (CurrentProfile.Profile.Courses.Exists(x => x.Title.Contains(CurrentProfile.CurrentCourse.Title)))
        {
            var course = CurrentProfile.Profile.Courses.Find(x => x.Title.Contains(CurrentProfile.CurrentCourse.Title));
            if (course.Finished)
                textButton.text = "Просмотреть курс";
            else
                textButton.text = "Продолжить курс";
        }
        else
        {
            textButton.text = "Начать курс";
            button.GetComponentInChildren<Button>().onClick.AddListener(() => objectProfiles.AddCourse(CurrentProfile.CurrentCourse));
        }
        button.GetComponentInChildren<Button>().onClick.AddListener(() => SceneManager.LoadScene("Course"));

    }
}
