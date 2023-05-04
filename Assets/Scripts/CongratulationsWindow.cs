using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CongratulationsWindow : ModalWindow
{
    public void Cancel()
    {
        _close.onClick.Invoke();
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene("AllCourses");
    }
}
