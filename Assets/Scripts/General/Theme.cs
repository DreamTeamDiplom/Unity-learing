using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public ThemeEnum ThemeType = ThemeEnum.White;
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
            ThemeType = ThemeEnum.Black;
        }
        ChangeTheme();
    }

    public event ITheme.ChangeImage OnChangeImage;

    public void ChangeTheme()
    {
        OnChangeImage?.Invoke();
    }
}