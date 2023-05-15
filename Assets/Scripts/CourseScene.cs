using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO.MemoryMappedFiles;

public class CourseScene : MonoBehaviour
{
    [Serializable]
    private class PrefabsLesson
    {
        public GameObject prefabCardFirst;
        public GameObject prefabCard;
        public GameObject prefabCardLast;
        public GameObject prefabCardCompleteFirst;
        public GameObject prefabCardComplete;
        public GameObject prefabCardCompleteLast;
    }

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

        lessonsSprites = GetAssetBundleLessons();

        titleCourse.text = CurrentProfile.CurrentCourse.Title;
        var lessonFirstUnfinish = CurrentProfile.CurrentCourse.LastLesson;

        for (int i = 0; i < CurrentProfile.CurrentCourse.Lessons.Count; i++)
        {
            Lesson lesson = CurrentProfile.CurrentCourse.Lessons[i];
            GameObject gameObjectLesson = GetGameObjectLesson(lesson, i);
            gameObjectLesson.GetComponent<ViewLesson>().Init(i + 1, lesson.Title);

            gameObjectLesson.GetComponentInChildren<Button>().onClick.AddListener( () => 
            { 
                SelectCourse(lesson); 
                gameObjectLesson.GetComponentInChildren<Button>().interactable = false; 
            });

            gameObjectLesson.transform.SetParent(contentLessons.transform, false);

            if (lesson == lessonFirstUnfinish)
            {
                string message = string.Format("{0}\n{1}\n{2}", i, CurrentProfile.Profile.PathFolder, lesson == CurrentProfile.CurrentCourse.Lessons[CurrentProfile.CurrentCourse.Lessons.Count - 1]);

                TryWriteFileToMemory(message);
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

    private GameObject GetGameObjectLesson(Lesson lesson, int index)
    {
        if (index == 0)
        {
            return !lesson.Finished ? Instantiate(lessons.prefabCardFirst) : Instantiate(lessons.prefabCardCompleteFirst);
        }
        else if (index == CurrentProfile.CurrentCourse.Lessons.Count - 1)
        {
            return !lesson.Finished ? Instantiate(lessons.prefabCardLast) : Instantiate(lessons.prefabCardCompleteLast);
        }
        else
        {
            return !lesson.Finished ? Instantiate(lessons.prefabCard) : Instantiate(lessons.prefabCardComplete);

        }
    }

    private AssetBundle GetAssetBundleLessons()
    {
        DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine(Application.streamingAssetsPath, "Courses"));

        FileInfo fileInfo = dirInfo.GetFiles().Where(f => !f.Name.EndsWith(".meta") &&
            f.Name.Contains(CurrentProfile.CurrentCourse.Title, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

        if (fileInfo == null)
        {
            /* Обработка незагруженного курса */
            return null;
        }

        AssetBundle loadedAssetBundle = AssetBundle.LoadFromFile(fileInfo.FullName);
        var assetBundleLesson = loadedAssetBundle.LoadAsset<TextAsset>("lessons");
        string assetBundlePath = Path.Combine(Application.temporaryCachePath, "lesson");
        File.WriteAllBytes(assetBundlePath, assetBundleLesson.bytes);

        loadedAssetBundle.Unload(false);

        return AssetBundle.LoadFromFile(assetBundlePath);
    }

    private string GetDescription(Lesson lesson)
    {
        if (lesson.Content.Length < 1)
        {
            return "Нет описания!";
        }
        else if (lesson.Content.Length == 1)
        {
            return lesson.Content[0];
        }
        else
        {
            return lesson.Content.Aggregate((x, y) => x + "\n" + y);
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
                    var spriteAsset = lessonsSprites.LoadAsset<TMP_SpriteAsset>("lesson" + (i + 1));
                    descriptionLesson.spriteAsset = spriteAsset;
                    break;
                }
            }
            
        }

        descriptionLesson.text = GetDescription(lesson);

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
                string[] messages = message.Split('\n');

                if (messages[1] != CurrentProfile.Profile.PathFolder)
                {
                    return;
                }

                if (int.TryParse(messages[0], out int index))
                {

                    if (index < 0 || CurrentProfile.CurrentCourse.Lessons.Count <= index)
                    {
                        return;
                    }
                    CurrentProfile.CurrentCourse.Lessons[index].Finished = true;

                    var course = CurrentProfile.Profile.Courses.FirstOrDefault(c => c.Title == CurrentProfile.CurrentCourse.Title);
                    if (course != null)
                    {
                        course.Lessons[index].Finished = true;
                    }
                    _dataProfiles.SaveData();
                    if (CurrentProfile.CurrentCourse.Finished)
                    {
                        windowsActive = true;
                    }
                    sharedMemory = null;
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
            char[] message = text.ToCharArray();
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
        if (lessonsSprites != null)
        {
            lessonsSprites.Unload(false);
        }
    }
}


