using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitWindow : ModalWindow
{
    public void Cancel()
    {
        _close.onClick.Invoke();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
