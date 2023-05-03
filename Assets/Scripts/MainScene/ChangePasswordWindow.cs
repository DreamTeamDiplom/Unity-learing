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
            Theme.Instance.ThemeType = profile.Setting.Theme;
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
}
