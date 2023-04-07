using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using UnityEngine.Networking;
using SFB;
using System.Text.RegularExpressions;

public class AddProfileWindow : ModalWindow
{
    [Header("GameObjects with error")]
    [SerializeField] private GameObject _inputName;
    [SerializeField] private GameObject _inputEmail;
    [SerializeField] private GameObject _iconGameObject;

    [Header("Inputs")]
    [SerializeField] private GameObject _inputPassword;
    [SerializeField] private GameObject _inputPath;

    [Space(10f)]
    [SerializeField] private ObjectProfiles _objectProfiles;
    public Sprite[] icons;

    private InputField _inputFieldName;
    private InputField _inputFiledEmail;
    private InputField _inputFieldPassword;
    private InputField _inputFieldPath;

    private int _index = 0;

    private new void Awake()
    {
        base.Awake();
        _inputFieldName = _inputName.GetComponentInChildren<InputField>();
        _inputFiledEmail = _inputEmail.GetComponentInChildren<InputField>();

        _inputFieldPassword = _inputPassword.GetComponentInChildren<InputField>();
        _inputFieldPath = _inputPath.GetComponentInChildren<InputField>();

        _inputFieldPath.text = Application.streamingAssetsPath;
    }
    private IEnumerator LoadTexture(string[] url, Action<Sprite> action)
    {
        if (url.Length != 0)
        {
            var request = UnityWebRequestTexture.GetTexture(url[0]);
            yield return request.SendWebRequest();
            var texture = DownloadHandlerTexture.GetContent(request);
            request.Dispose();

            Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            sprite.name = url[0];

            action(sprite);
        }
    }

    private void LoadTexture(Sprite sprite)
    {
        icons[^1] = sprite;
        _iconGameObject.transform.GetChild(0).GetComponent<Image>().sprite = icons.Last();
        _iconGameObject.transform.GetChild(2).GetComponent<Image>().sprite = icons.Last();
    }

    /// <summary>
    /// �������� ������������
    /// </summary>
    public void AddProfile()
    {
        bool error = false;
        error |= _iconGameObject.GetComponentInParent<CheckingError>().CheckingIcon(icons[_index].name != "AddPhoto");

        error |= _inputName.GetComponent<CheckingError>().CheckingInput(_inputFieldName.text != "");

        bool isEmail = Regex.IsMatch(_inputFiledEmail.text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        error |= _inputEmail.GetComponent<CheckingError>().CheckingInput(isEmail);

        if (!error)
        {
            Profile newProfile = new Profile(icons[_index].name, _inputFieldName.text, _inputFieldPassword.text, _inputFiledEmail.text, _inputFieldPath.text);
            _objectProfiles.AddProfile(newProfile);

            CurrentProfile.Profile = newProfile;
            CurrentProfile.Profile.Icon = icons[_index];
            PlayerPrefs.SetInt("Animation", 1);
            if (Directory.Exists(CurrentProfile.Profile.PathFolder))
            {
                int number = 2;
                while (Directory.Exists(CurrentProfile.Profile.PathFolder + $" ({number})"))
                    number++;
                Directory.CreateDirectory(CurrentProfile.Profile.PathFolder + $" ({number})");
                CurrentProfile.Profile.PathFolder = CurrentProfile.Profile.PathFolder + $" ({number})";
            }
            else
            {
                Directory.CreateDirectory(CurrentProfile.Profile.PathFolder);
            }
            SceneManager.LoadScene("AllCourses");
        }
    }

    /// <summary>
    /// ������� ��� ���������� ���� ��� ���������� ��������
    /// </summary>
    public void AddPath()
    {
        var path = StandaloneFileBrowser.OpenFolderPanel("�������� ���� ��� �������� ����� ��������", _inputFieldPath.text, false);
        _inputFieldPath.text = path.Length != 0 ? path[0] : _inputFieldPath.text;
    }


    /// <summary>
    /// ������� ��� ���������� ���� ������ ������������
    /// </summary>
    public void AddPathIcon()
    {
        if (_index < icons.Length - 1)
            return;
        var extensions = new[] {
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
            new ExtensionFilter("All Files", "*" ),
        };
        var pathIcon = StandaloneFileBrowser.OpenFilePanel("�������� ������ ��� �������", "", extensions, false);
        StartCoroutine(LoadTexture(pathIcon, LoadTexture));
    }

    

    public void LeftMove(int lengthShift)
    {
        _iconGameObject.transform.localPosition = Vector3.zero;
        _iconGameObject.transform.GetChild(1).GetComponent<Image>().sprite = icons[_index];

        _index = _index == icons.Length - 1 ? 0 : _index + 1;
        _iconGameObject.transform.GetChild(2).GetComponent<Image>().sprite = icons[_index];

        _iconGameObject.transform.DOLocalMoveX(lengthShift, 0.5f).Restart();
    }

    public void RightMove(int lengthShift)
    {
        _iconGameObject.transform.localPosition = Vector3.zero;
        _iconGameObject.transform.GetChild(1).GetComponent<Image>().sprite = icons[_index];

        _index = _index == 0 ? icons.Length - 1 : _index - 1;
        _iconGameObject.transform.GetChild(0).GetComponent<Image>().sprite = icons[_index];

        _iconGameObject.transform.DOLocalMoveX(lengthShift, 0.5f).Restart();
    }
}
