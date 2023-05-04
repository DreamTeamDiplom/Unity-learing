using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ElementMenuView : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        Theme.Instance.OnChangeImage += ChangeTheme;
    }

    private void OnEnable()
    {
        ChangeTheme();
    }

    private void SliderSample_OnEnableSlider()
    {
        if (GetTheme() == "Black")
        {
            GetComponentInChildren<Text>().color = new Color(225f / 255, 227f / 255, 230f / 255, 1);
        }
        else
        {
            GetComponentInChildren<Text>().color = Color.black;
        }
    }
    private void SliderSample_OnDisableSlider()
    {
        GetComponentInChildren<Text>().color = Color.white;
        ColorBlock colorBlock = new ColorBlock();

        if (GetTheme() == "Black")
        {
            colorBlock.normalColor = new Color(225f / 255, 227f / 255, 230f / 255, 1);
        }
        else

        {
            colorBlock.normalColor = Color.black;
        }
        /* Подобрать коэффициенты */
        colorBlock.highlightedColor = GetColorAbs(colorBlock.normalColor, Color.white * 0.35f);
        colorBlock.pressedColor = GetColorAbs(colorBlock.normalColor, Color.white * 0.4f);
        colorBlock.selectedColor = colorBlock.normalColor;
        colorBlock.colorMultiplier = 1;
        colorBlock.fadeDuration = 0.1f;
        button.colors = colorBlock;
    }
    private void ChangeTheme()
    {
        if (GetAnimation())
        {
            GetComponent<Button>().transition = Selectable.Transition.None;
            SliderSample_OnEnableSlider();
            return;
        }

        GetComponent<Button>().transition = Selectable.Transition.ColorTint;
        SliderSample_OnDisableSlider();
    }

    private bool GetAnimation()
    {
        return PlayerPrefs.GetInt("Animation", 1) == 1;
    }
    private Color GetColorAbs(Color a, Color b)
    {
        return new Color(Mathf.Abs(a.r - b.r), Mathf.Abs(a.g - b.g), Mathf.Abs(a.b - b.b), 1);
    }

    private string GetTheme()
    {
        return PlayerPrefs.GetString("Theme", "Black");
    }

    private void OnDestroy()
    {
        Theme.Instance.OnChangeImage -= ChangeTheme;
    }
}
