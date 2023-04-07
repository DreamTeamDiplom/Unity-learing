using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeleteAccount : MonoBehaviour
{
    [SerializeField] private ObjectProfiles objectProfiles;

    private void OnEnable()
    {
        transform.GetChild(0).DOPunchScale(Vector3.one / 10, .5f, 1, 0);
    }


    public void Delete()
    {
        objectProfiles.DeleteProfile(CurrentProfile.Profile);
        SceneManager.LoadScene("MainScene");
    }
}
