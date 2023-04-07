using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;

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
    [Header("Header")]
    [SerializeField] private Text titleCourse;
    [SerializeField] private GameObject icon;

    [Header("Lessons")]
    [SerializeField] private PrefabsLesson lessons;
    [SerializeField] private GameObject contentLessons;

    [Header("Description")]
    [SerializeField] private Text titleLesson;
    [SerializeField] private GameObject objVideoPlayer;
    [SerializeField] private VideoPlayer vPlayer;
    [SerializeField] private GameObject windowLoading;
    [SerializeField] private TextMeshProUGUI descriptionLesson;
    [SerializeField] private Scrollbar scrollbar;

    [Header("Windows")]
    [SerializeField] private GameObject congratulations;

    [Space(10f)]
    [SerializeField] private ObjectProfiles _dataProfiles;

    private static bool windowsActive = false;
    private AssetBundle lessonsSprites;
    private MemoryMappedFile sharedMemory;
    private void Awake()
    {
        icon.GetComponentInChildren<Image>().sprite = CurrentProfile.Profile.Icon;
        icon.GetComponentInChildren<Text>().text = CurrentProfile.Profile.Name;

        lessonsSprites = AssetBundle.LoadFromFile(Path.Combine(CurrentProfile.Profile.PathFolder, CurrentProfile.CurrentCourse.Title, "lessons"));

        titleCourse.text = CurrentProfile.CurrentCourse.Title;
        var lessonFirstUnfinish = CurrentProfile.CurrentCourse.LastLesson;

        for (int i = 0; i < CurrentProfile.CurrentCourse.Lessons.Count; i++)
        {
            Lesson lesson = CurrentProfile.CurrentCourse.Lessons[i];
            GameObject lessonObj = null;
            if (!lesson.Finished)
            {
                if (i == 0)
                {
                    lessonObj = Instantiate(lessons.prefabCardFirst);
                }
                else if (i == CurrentProfile.CurrentCourse.Lessons.Count - 1)
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
                else if (i == CurrentProfile.CurrentCourse.Lessons.Count - 1)
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

            if (lesson == lessonFirstUnfinish)
            {
                TryWriteFileToMemory(i.ToString());
            }
        }

        if (windowsActive && CurrentProfile.CurrentCourse.Finished)
        {
            congratulations.SetActive(true);
            windowsActive = false;
        }
        if (CurrentProfile.CurrentCourse.Finished)
        {
            SelectCourse(CurrentProfile.CurrentCourse.Lessons[0]);
        }
        else
        {
            SelectCourse(lessonFirstUnfinish);
        }

    }

    private void SelectCourse(Lesson lesson)
    {
        if (lesson == null)
            return;


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
        
        if (lessonsSprites != null)
        {
            for (int i = 0; i < CurrentProfile.CurrentCourse.Lessons.Count; i++)
            {
                if (lesson == CurrentProfile.CurrentCourse.Lessons[i])
                {
                    contentLessons.transform.GetChild(i).GetComponent<Button>().interactable = false;
                    var spriteAsset = lessonsSprites.LoadAsset<TMP_SpriteAsset>("lesson" + (i + 1));
                    descriptionLesson.spriteAsset = spriteAsset;
                    break;
                }
            }
            
        }

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

        scrollbar.value = 1;
    }



    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            if (TryReadFileToMemory(out string message))
            {
                if (int.TryParse(message, out int index))
                {
                    CurrentProfile.CurrentCourse.Lessons[index].Finished = true;

                    var course = CurrentProfile.Profile.Courses.FirstOrDefault(c => c.Title == CurrentProfile.CurrentCourse.Title);
                    if (course != null)
                    {
                        course.Lessons[index].Finished = true;
                    }
                    _dataProfiles.SaveData();
                    //lessonsSprites.Unload(false);
                    if (CurrentProfile.CurrentCourse.Finished)
                    {
                        windowsActive = true;
                    }
                    SceneManager.LoadScene("Course");
                }
            }

        }
    }
    private bool TryReadFileToMemory(out string result)
    {
        try
        {
            char[] message;
            int size;

           sharedMemory = MemoryMappedFile.OpenExisting("test");

            using (MemoryMappedViewAccessor reader = sharedMemory.CreateViewAccessor(0, 4, MemoryMappedFileAccess.Read))
            {
                size = reader.ReadInt32(0);
            }

            using (MemoryMappedViewAccessor reader = sharedMemory.CreateViewAccessor(4, size * 2, MemoryMappedFileAccess.Read))
            {
                //Массив символов сообщения
                message = new char[size];
                reader.ReadArray<char>(0, message, 0, size);
            }
            result = new string(message);
            return true;

        }
        catch (Exception) { }

        result = null;
        return false;
    }

    private bool TryWriteFileToMemory(string text)
    {
        try
        {
            char[] message = text.ToString().ToCharArray();
            int size = message.Length;
            sharedMemory = MemoryMappedFile.CreateOrOpen("lesson", size * 2 + 4);

            using (MemoryMappedViewAccessor writer = sharedMemory.CreateViewAccessor(0, size * 2 + 4))
            {
                writer.Write(0, size);
                writer.WriteArray<char>(4, message, 0, message.Length);
            }

            return true;

        }
        catch (Exception) { }
        return false;
    }

    private void OnDestroy()
    {
        lessonsSprites.Unload(false);
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene("AllCources");
    }

    public void DoToStart()
    {
        var unityProcess = Process.GetProcessesByName("unity").FirstOrDefault();
        if (unityProcess == null)
        {
            Process.Start(@"C:\Program Files\Unity Hub\Unity Hub.exe");
        }
        Application.OpenURL("Unity Hub.exe");
    }
}


