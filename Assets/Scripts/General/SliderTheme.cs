using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;

public class SliderTheme : MonoBehaviour
{
    public static Themes theme;


    [SerializeField] GameObject _circle;
    [SerializeField] GameObject _text;

    private void Awake()
    {
        //theme ??= new Themes();
        if (PlayerPrefs.GetString("Theme") == "Black") 
        {
            Theme.Instance.nameTheme = "Black";
            _circle.transform.DOLocalMoveX(-12, 0f);
            _text.GetComponent<Text>().text = "Тёмная";
        }
        else
        {
            _circle.transform.DOLocalMoveX(-28, 0f);
            _text.GetComponent<Text>().text = "Светлая";
            Theme.Instance.nameTheme = "White";
        }
        Theme.Instance.Change();
    }

    private void Init()
    {
        if (PlayerPrefs.GetString("Theme") == "Black")
        {
            PlayerPrefs.SetString("Theme", "White");
            _circle.transform.DOLocalMoveX(-28, .5f);
            _text.GetComponent<Text>().text = "Светлая";
            Theme.Instance.nameTheme = "White";
        }
        else
        {
            PlayerPrefs.SetString("Theme", "Black");
            Theme.Instance.nameTheme = "Black";
            _circle.transform.DOLocalMoveX(-12, .5f);
            _text.GetComponent<Text>().text = "Тёмная";
        }
        Theme.Instance.Change();
    }

    public void OnButtonSlider()
    {
        Init();
    }
}
