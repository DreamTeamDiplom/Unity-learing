using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthProfileWindow : ModalWindow
{
    [SerializeField] private GameObject _inputPassword;
    [SerializeField] private TextMeshProUGUI _forgotPassword;
    [Header("Windows")]
    [SerializeField] private GameObject _changePasswordWindow;

    private InputField _inputFieldPassword;
    private CheckingError _checkingError;
    private ChangePasswordWindow _changePasswordScript;

    [HideInInspector] public Profile profile;
    private new void Awake()
    {
        base.Awake();
        _checkingError = _inputPassword.GetComponent<CheckingError>();
        _inputFieldPassword = _inputPassword.GetComponentInChildren<InputField>();
        _changePasswordScript = _changePasswordWindow.GetComponent<ChangePasswordWindow>();
    }
    private new void OnEnable()
    {
        base.OnEnable();
        _inputFieldPassword.text = "";
        _checkingError.CheckingInput(true);
    }

    /// <summary>
    /// ����������� ������������ � ���� "�����������"
    /// </summary>
    public void AuthUser()
    {
        var hash = new Hash128();
        hash.Append(_inputFieldPassword.text);
        bool error = false;

        error |= _checkingError.CheckingInput(hash.ToString() == profile.Password);
        if (!error)
        {
            CurrentProfile.Profile = profile;
            PlayerPrefs.SetString("Theme", profile.Setting.Theme.ToString());
            PlayerPrefs.SetInt("Animation", Convert.ToInt32(profile.Setting.Animation));
            Theme.Instance.NameTheme = profile.Setting.Theme.ToString();
            Theme.Instance.Change();
            if (CurrentProfile.Profile.Courses.Count == 0)
            {
                SceneManager.LoadScene("AllCourses");
            }
            else
            {
                SceneManager.LoadScene("ProfileCourses");
            }
        }
    }

    /// <summary>
    /// ��� ��������� �� "������ ������"
    /// </summary>
    public void OnPointerEnter()
    {
        _forgotPassword.fontStyle = FontStyles.Bold;
    }

    /// <summary>
    /// ��� �������� � "������ ������"
    /// </summary>
    public void OnPointerExit()
    {
        _forgotPassword.fontStyle = FontStyles.Normal;
    }

    /// <summary>
    /// ������� ���� "������ ������"
    /// </summary>
    public void OnActiveWindowForgotPassword()
    {
        _changePasswordWindow.SetActive(true);
        _changePasswordScript.profile = profile;
    }
}
