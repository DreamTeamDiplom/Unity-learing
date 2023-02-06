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
    public string nameTheme = "White";
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
            nameTheme = "Black";
        }
        ChangeTheme();
    }

    public event ITheme.ChangeImage OnChangeImage;

    public void ChangeTheme()
    {
        OnChangeImage?.Invoke();
    }
}

public class Themes
{
    public List<ThemeImage> themeImages;
    public List<ThemeColor> themeColors;
    public List<ThemeState> themeStates;


    public void OnButtonSlider()
    {
        foreach (var theme in themeImages)
            theme.ChangeTheme();
        foreach (var theme in themeColors)
            theme.ChangeTheme();
        foreach (var theme in themeStates)
            theme.ChangeTheme();
    }
}


[System.Serializable]
public class ThemeImage
{
    [SerializeField] private string[] _name;

    public delegate void ChangeImage();
    public event ChangeImage OnChangeImage;

    public static implicit operator string(ThemeImage themeString)
    {
        if (!PlayerPrefs.HasKey("Theme"))
        {
            PlayerPrefs.SetString("Theme", "Black");
        }
        SliderTheme.theme ??= new Themes();
        SliderTheme.theme.themeImages ??= new List<ThemeImage>();

        if (!SliderTheme.theme.themeImages.Contains(themeString))
        {
            SliderTheme.theme.themeImages.Add(themeString);

        }
        string path_loc = Path.Combine(PlayerPrefs.GetString("Theme"), Path.Combine(themeString._name));
        return path_loc;
    }
    public void ChangeTheme()
    {
        OnChangeImage?.Invoke();
    }
}

[System.Serializable]
public class ThemeColor
{
    [SerializeField] private string[] _name;

    public delegate void ChangeColor();
    public event ChangeColor OnChangeColor;

    public static implicit operator string(ThemeColor themeColor)
    {
        if (!PlayerPrefs.HasKey("Theme"))
        {
            PlayerPrefs.SetString("Theme", "Black");
        }
        SliderTheme.theme ??= new Themes();
        SliderTheme.theme.themeColors ??= new List<ThemeColor>();

        if (!SliderTheme.theme.themeColors.Contains(themeColor))
        {
            SliderTheme.theme.themeColors.Add(themeColor);

        }
        string path_loc = Path.Combine(PlayerPrefs.GetString("Theme"), Path.Combine(themeColor._name));
        return path_loc;
    }
    public void ChangeTheme()
    {
        OnChangeColor?.Invoke();
    }
}

[System.Serializable]
public class ThemeState
{
    [SerializeField] private string[] _name;

    public delegate void ChangeState();
    public event ChangeState OnChangeState;

    public static implicit operator string(ThemeState themeState)
    {
        if (!PlayerPrefs.HasKey("Theme"))
        {
            PlayerPrefs.SetString("Theme", "Black");
        }
        SliderTheme.theme ??= new Themes();
        SliderTheme.theme.themeStates ??= new List<ThemeState>();

        if (!SliderTheme.theme.themeStates.Contains(themeState))
        {
            SliderTheme.theme.themeStates.Add(themeState);

        }
        string path_loc = Path.Combine(PlayerPrefs.GetString("Theme"), Path.Combine(themeState._name));
        return path_loc;
    }
    public void ChangeTheme()
    {
        OnChangeState?.Invoke();
    }
}
