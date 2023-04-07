using DG.Tweening;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private GameObject _myProfile;
    [SerializeField] private GameObject _allCourses;
    [SerializeField] private GameObject _myCourses;
    [SerializeField] private GameObject _setting;
    [SerializeField] private GameObject _exitProfile;
    [SerializeField] private GameObject _exit;

    //[Header("Text")]
    //[SerializeField] GameObject _message;

    [Header("Windows")]
    [SerializeField] GameObject _menuWindow;

    private Button _myProfileButton;
    private Button _allCoursesButton;
    private Button _myCoursesButton;
    private Button _settingButton;
    private Button _exitProfileButton;
    //Button _exitButton;

    private RectTransform _menuTransform;

    private bool _animation;
    private void Awake()
    {
        var objs = GameObject.FindGameObjectsWithTag("Menu");
        if (objs.Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        
        _animation = GetAnimation();

        _myProfileButton = _myProfile.GetComponentInChildren<Button>();
        _myProfileButton.onClick.AddListener(() => {
            OnButtonClick("MyProfile");
            MyProfile.page = Page.MyProfile;
        });

        _allCoursesButton = _allCourses.GetComponentInChildren<Button>();
        _allCoursesButton.onClick.AddListener(() => OnButtonClick("AllCourses"));

        _myCoursesButton = _myCourses.GetComponentInChildren<Button>();
        _myCoursesButton.onClick.AddListener(() => OnButtonClick("ProfileCourses"));

        _settingButton = _setting.GetComponentInChildren<Button>();
        _settingButton.onClick.AddListener(() => {
            OnButtonClick("MyProfile");
            MyProfile.page = Page.Setting;
        });

        _exitProfileButton = _exitProfile.GetComponentInChildren<Button>();
        _exitProfileButton.onClick.AddListener(() => OnButtonClick("MainScene"));

        _menuTransform = (RectTransform)_menuWindow.transform.GetChild(0);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnButtonClick(string nameScene)
    {
        foreach (var gameObject in new GameObject[] { _myProfile, _allCourses, _myCourses, _setting, _exitProfile })
        {
            OnPointerExit(gameObject.transform.GetChild(0).gameObject);
        }
        _menuTransform.DOAnchorPosX(-_menuTransform.sizeDelta.x, .5f);
        _menuWindow.SetActive(false);
        SceneManager.LoadScene(nameScene);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_menuWindow.activeSelf)
            {
                _menuWindow.SetActive(true);
                _menuTransform.DOAnchorPosX(0, .5f);
            }
            else
            {
                MinimizeMenu();
            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _animation = GetAnimation();
        foreach (var gameObject in new GameObject[] { _myProfile, _allCourses, _myCourses, _setting, _exitProfile })
        {
            gameObject?.SetActive(!(scene.name == "MainScene"));
        }
    }

    public void MinimizeMenu()
    {
        _menuTransform.DOAnchorPosX(-_menuTransform.sizeDelta.x, .5f).OnComplete(() => _menuWindow.SetActive(false));
    }

    public void ApplicationQuit()
    {
        Application.Quit();
    }

    public void OnPointerEnter(GameObject gameObject)
    {
        if (_animation)
        {
            gameObject.transform.DOLocalMoveX(40, .5f);
            gameObject.GetComponentInParent<Button>().transition = Selectable.Transition.None;
        }
        else
        {
            gameObject.GetComponentInParent<Button>().transition = Selectable.Transition.ColorTint;
        }


    }

    public void OnPointerExit(GameObject gameObject)
    {
        if (_animation)
        {
            gameObject.transform.DOLocalMoveX(0, .5f); 
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private bool GetAnimation()
    {
        if (PlayerPrefs.HasKey("Animation"))
        {
            return PlayerPrefs.GetInt("Animation") == 1;
        }
        else
        {
            return true;
        }
    }

}
