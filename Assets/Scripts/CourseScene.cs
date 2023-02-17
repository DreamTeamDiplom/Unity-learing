using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.IO;
using UnityEditor;
using TMPro;

[Serializable]
internal class PrefabsLesson
{
    public GameObject prefabCardFirst;
    public GameObject prefabCard;
    public GameObject prefabCardLast;
    public GameObject prefabCardCompleteFirst;
    public GameObject prefabCardComplete;
    public GameObject prefabCardCompleteLast;
}

public class CourseScene : MonoBehaviour
{
    [SerializeField] private Text titleCourse;
    [SerializeField] private GameObject icon;
    [SerializeField] private PrefabsLesson lessons;
    [SerializeField] private GameObject contentLessons;
    [SerializeField] private Text titleLesson;
    [SerializeField] private GameObject objVideoPlayer;
    [SerializeField] private VideoPlayer vPlayer;
    [SerializeField] private GameObject windowLoading;
    [SerializeField] private TextMeshProUGUI descriptionLesson;

    private void Awake()
    {
        icon.GetComponentInChildren<Image>().sprite = CurrentProfile.Profile.Icon;
        icon.GetComponentInChildren<Text>().text = CurrentProfile.Profile.Name;

        titleCourse.text = CurrentProfile.CurrentCourse.Course.Title;

        for (int i = 0; i < CurrentProfile.CurrentCourse.Course.Lessons.Count; i++)
        {
            Lesson lesson = CurrentProfile.CurrentCourse.Course.Lessons[i];
            GameObject lessonObj = null;
            if (!lesson.Finished)
            {
                if (i == 0)
                {
                    lessonObj = Instantiate(lessons.prefabCardFirst);
                }
                else if (i == CurrentProfile.CurrentCourse.Course.Lessons.Count - 1)
                {
                    lessonObj = Instantiate(lessons.prefabCardLast);
                }
                else
                {
                    lessonObj = Instantiate(lessons.prefabCard);
                }
            }
            else
            {
                if (i == 0)
                {
                    lessonObj = Instantiate(lessons.prefabCardCompleteFirst);
                }
                else if (i == CurrentProfile.CurrentCourse.Course.Lessons.Count - 1)
                {
                    lessonObj = Instantiate(lessons.prefabCardCompleteLast);
                }
                else
                {
                    lessonObj = Instantiate(lessons.prefabCardComplete);
                }
            }
            var textInfo = lessonObj.transform.GetChild(1);
            textInfo.GetChild(0).GetComponent<Text>().text = string.Format("Урок {0}.", i + 1);
            textInfo.GetChild(1).GetComponent<Text>().text = lesson.Title;
            lessonObj.GetComponentInChildren<Button>().onClick.AddListener(
                () => { 
                    SelectCourse(lesson); 
                    lessonObj.GetComponentInChildren<Button>().interactable = false; 
                });
            lessonObj.transform.SetParent(contentLessons.transform, false);
        }

        var lessonFirstUnfinish = CurrentProfile.CurrentCourse.LastLesson;
        SelectCourse(lessonFirstUnfinish);
    }

    private void SelectCourse(Lesson lesson)
    {
        foreach (Transform child in contentLessons.transform) {
            child.GetComponentInChildren<Button>().interactable = true;
        }
        vPlayer.Stop();
        windowLoading.SetActive(true);
        windowLoading.transform.GetChild(0).gameObject.SetActive(true);
        windowLoading.transform.GetChild(1).gameObject.SetActive(false);
        if (!(lesson.VideoUrl == null || lesson.VideoUrl == ""))
        {
            objVideoPlayer.transform.localScale = Vector2.one;
            vPlayer.url = lesson.VideoUrl;
            vPlayer.Prepare();
        }
        else
        {
            objVideoPlayer.transform.localScale = Vector2.right;
        }
        titleLesson.text = lesson.Title;
        if (lesson.Description.Length < 1)
        {
            descriptionLesson.text = "Нет описания!";
        }
        else if (lesson.Description.Length == 1)
        {
            descriptionLesson.text = lesson.Description[0];
        }
        else
        {
            descriptionLesson.text = lesson.Description.Aggregate((x, y) => x + "\n" + y);

        }
        LayoutRebuilder.MarkLayoutForRebuild((RectTransform)objVideoPlayer.transform.parent);

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.MarkLayoutForRebuild((RectTransform)descriptionLesson.gameObject.transform);
    }

    //public void CreateInputFile(){
    //    var pathInput = Path.Combine(Application.persistentDataPath, "testInput.txt");
    //    var countLessonsFinish = CurrentProfile.currentCourse.lessons.Where(l => l.finish).Count();
    //    using (var sw = new StreamWriter(pathInput))
    //    {
    //        sw.WriteLine(countLessonsFinish < CurrentProfile.currentCourse.lessons.Count() ? countLessonsFinish : countLessonsFinish - 1 );
    //    }
    //}
}


