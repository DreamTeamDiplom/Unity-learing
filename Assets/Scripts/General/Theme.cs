using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Unity.VisualScripting;
using UnityEngine;

public interface ITheme
{
    public delegate void ChangeImage();
    public event ChangeImage OnChangeImage;

    public void ChangeTheme();
}

public class Theme : ITheme
{
    private static Theme _instance;
    public string NameTheme = "White";
    public static Theme Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Theme();
            }
            return _instance;
        }
    }
    public void Change()
    {
        if (!PlayerPrefs.HasKey("Theme"))
        {
            PlayerPrefs.SetString("Theme", "Black");
            NameTheme = "Black";
        }
        ChangeTheme();
    }

    public event ITheme.ChangeImage OnChangeImage;

    public void ChangeTheme()
    {
        OnChangeImage?.Invoke();
    }
}