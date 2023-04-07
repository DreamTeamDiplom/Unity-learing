using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangePasswordWindow : ModalWindow
{
    [Header("Inputs")]
    [SerializeField] private InputField _inputFieldEmail;
    [SerializeField] private InputField _inputPassword;
    [SerializeField] private InputField _inputDoublePassword;

    [Space(10f)]
    [SerializeField] private ObjectProfiles objectProfiles;

    [HideInInspector] public Profile profile;

    /// <summary>
    /// ����� ��������� Email � �������� ������ ������������ � ������ ������ ������ �� �����
    /// </summary>
    public  void ChangePassword()
    {
        bool error = false;
        error |= _inputFieldEmail.GetComponentInParent<CheckingError>().CheckingInput(_inputFieldEmail.text == profile.Email);
        _inputDoublePassword.GetComponentInParent<CheckingError>().CheckingInput(true);

        if (error)
        {
            return;
        }

        error |= _inputDoublePassword.GetComponentInParent<CheckingError>().CheckingInput(_inputDoublePassword.text == _inputPassword.text);

        if (!error)
        {
            profile.Password = profile.HashData(_inputDoublePassword.text);
            objectProfiles.SaveData();

            CurrentProfile.Profile = profile;
            PlayerPrefs.SetString("Theme", profile.Setting.Theme.ToString());
            Theme.Instance.NameTheme = profile.Setting.Theme.ToString();
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
