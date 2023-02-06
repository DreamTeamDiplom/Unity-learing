using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthProfile : MonoBehaviour
{
    [SerializeField] private GameObject inputPassword;

    [HideInInspector] public Profile profile;

    private InputField textPassword;
    private CheckingInput checkingInput;
    private void Awake()
    {
        checkingInput = inputPassword.GetComponent<CheckingInput>();
        textPassword = inputPassword.GetComponentInChildren<InputField>();
    }
    private void OnEnable()
    {
        textPassword.text = "";
        transform.GetChild(0).DOPunchScale(Vector3.one / 10, .5f, 1, 0);
        checkingInput.Checking(true);
    }

    public void Auth()
    {
        var hash = new Hash128();
        hash.Append(textPassword.text);
        bool error = false;

        error |= checkingInput.Checking(hash.ToString() == profile.Password);
        if (!error)
        {
            CurrentProfile.Profile = profile;
            SceneManager.LoadScene("ProfileCourses");
        }

    }
}
