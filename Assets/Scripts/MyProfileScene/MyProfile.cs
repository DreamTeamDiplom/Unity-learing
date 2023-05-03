using SFB;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Page
{
    MyProfile,
    Password,
    Setting
}

public class MyProfile : MonoBehaviour
{
    [SerializeField] private GameObject tabs;
    [SerializeField] private GameObject pages;

    [Header("Fields Profile")]
    [SerializeField] private Image icon;
    [SerializeField] private GameObject nameProfile;
    [SerializeField] private GameObject email;
    [SerializeField] private GameObject path;

    [Space(10f)]
    [SerializeField] private ObjectProfiles objectProfiles;

    [Header("Password")]
    public InputField inputEmail2;
    public InputField inputPassword;
    public InputField inputPassword2;

    private Text textPath;
    private string tempPath;
    private GameObject inputName;
    private Text textName;
    private GameObject inputEmail;
    private Text textEmail;

    public static Page page = Page.MyProfile;

    private void Awake()
    {
        textPath = path.GetComponent<Text>();
        inputName = nameProfile.transform.GetChild(3).gameObject;
        textName = nameProfile.transform.GetChild(1).GetComponent<Text>();
        inputEmail = email.transform.GetChild(3).gameObject;
        textEmail = email.transform.GetChild(1).GetComponent<Text>();

        icon.sprite = CurrentProfile.Profile.Icon;
        nameProfile.transform.GetChild(1).GetComponent<Text>().text = CurrentProfile.Profile.Name;
        inputName.GetComponentInChildren<InputField>().text = CurrentProfile.Profile.Name;
        email.transform.GetChild(1).GetComponent<Text>().text = CurrentProfile.Profile.Email;
        inputEmail.GetComponentInChildren<InputField>().text = CurrentProfile.Profile.Email;

        textPath.text = CurrentProfile.Profile.PathFolder;
        tempPath = textPath.text;

        PathProfile();
        tabs.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
        {
            page = Page.MyProfile;
            SelectPage();
        });
        tabs.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
        {
            page = Page.Password;
            SelectPage();
        });
        tabs.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
        {
            page = Page.Setting;
            SelectPage();
        });
        SelectPage();
    }

    private void OnDestroy()
    {
        if (PlayerPrefs.GetString("Theme") == "Black")
        {
            CurrentProfile.Profile.Setting.Theme = ThemeEnum.Black;
        }
        else
        {
            CurrentProfile.Profile.Setting.Theme = ThemeEnum.White;
        }
        objectProfiles.SaveData();
    }

    /* ���������� */
    private void PathProfile()
    {
        var settings = textPath.GetGenerationSettings(Vector2.zero);
        textPath.text = "..." + textPath.text;
        var preferredWidth = textPath.cachedTextGeneratorForLayout.GetPreferredWidth(textPath.text, settings) / textPath.pixelsPerUnit;
        while (preferredWidth > path.GetComponent<RectTransform>().sizeDelta.x)
        {
            textPath.text = "..." + textPath.text.Substring(4);
            preferredWidth = textPath.cachedTextGeneratorForLayout.GetPreferredWidth(textPath.text, settings) / textPath.pixelsPerUnit;
        }
    }

    private void SelectPage()
    {
        tabs.transform.GetChild(0).GetComponent<Button>().interactable = page != Page.MyProfile;
        tabs.transform.GetChild(1).GetComponent<Button>().interactable = page != Page.Password;
        tabs.transform.GetChild(2).GetComponent<Button>().interactable = page != Page.Setting;
        pages.transform.GetChild(0).gameObject.SetActive(page == Page.MyProfile);
        pages.transform.GetChild(1).gameObject.SetActive(page == Page.Password);
        pages.transform.GetChild(2).gameObject.SetActive(page == Page.Setting);
    }

    public void AddPath()
    {
        var path = StandaloneFileBrowser.OpenFolderPanel("�������� ���� ��� �������� ����� ��������", tempPath, false);
        textPath.text = path.Length != 0 ? path[0] : textPath.text;
        tempPath = path.Length != 0 ? path[0] : tempPath;
        PathProfile();
    }

    public void CheckingInputName()
    {
        var checkingInput = inputName.GetComponent<CheckingError>();
        var inputField = checkingInput.GetComponentInChildren<InputField>();
        if (!checkingInput.CheckingInput(inputField.text != ""))
        {
            textName.text = inputField.text;
            inputName.SetActive(false);
        }
    }

    public void CheckingInputEmail()
    {
        var checkingInput = inputEmail.GetComponent<CheckingError>();
        var inputField = checkingInput.GetComponentInChildren<InputField>();
        bool isEmail = Regex.IsMatch(textEmail.text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        if (!checkingInput.CheckingInput(isEmail))
        {
            textEmail.text = inputField.text;
            inputEmail.SetActive(false);
        }
    }

    public void SaveProfile()
    {
        var profile = CurrentProfile.Profile;
        profile.Icon = icon.sprite;
        profile.Name = textName.text;
        profile.Email = textEmail.text;
        profile.PathFolder = tempPath;
        objectProfiles.SaveData();
        if (CurrentProfile.Profile.Courses.Count == 0)
        {
            SceneManager.LoadScene("All�ourses");
        }
        else
        {
            SceneManager.LoadScene("ProfileCourses");
        }
    }

    public void ChangePassword()
    {
        bool error = false;
        error |= inputEmail2.GetComponentInParent<CheckingError>().CheckingInput(inputEmail2.text == CurrentProfile.Profile.Email);
        inputPassword2.GetComponentInParent<CheckingError>().CheckingInput(true);

        if (error)
        {
            return;
        }

        error |= inputPassword2.GetComponentInParent<CheckingError>().CheckingInput(inputPassword2.text == inputPassword.text);

        if (!error)
        {
            CurrentProfile.Profile.Password = CurrentProfile.Profile.HashData(inputPassword2.text);
            objectProfiles.SaveData();

            PlayerPrefs.SetString("Theme", CurrentProfile.Profile.Setting.Theme.ToString());
            Theme.Instance.ThemeType = CurrentProfile.Profile.Setting.Theme;
            Theme.Instance.Change();
            if (CurrentProfile.Profile.Courses.Count == 0)
            {
                SceneManager.LoadScene("All�ourses");
            }
            else
            {
                SceneManager.LoadScene("ProfileCourses");
            }
        }

    }

}
