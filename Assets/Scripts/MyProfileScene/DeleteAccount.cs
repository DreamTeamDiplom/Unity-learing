using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeleteAccount : ModalWindow
{
    [SerializeField] private ObjectProfiles objectProfiles;

    public void Delete()
    {
        objectProfiles.DeleteProfile(CurrentProfile.Profile);
        SceneManager.LoadScene("MainScene");
    }

    public void Cancel()
    {
        _close.onClick.Invoke();
    }
}
