using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(SliderSample))]
public class SliderTheme : MonoBehaviour
{
    [SerializeField] GameObject _text;

    private SliderSample _sliderSampleScript;
    private void Awake()
    {
        _sliderSampleScript = GetComponent<SliderSample>();
        _sliderSampleScript.OnEnableSlider += SliderSample_OnEnableSlider;
        _sliderSampleScript.OnDisableSlider += SliderSample_OnDisableSlider;

        if (PlayerPrefs.GetString("Theme") == "Black")
        {
            _sliderSampleScript.InitSlider(SliderSample.State.Off);
            BlackTheme();
        }
        else
        {
            _sliderSampleScript.InitSlider(SliderSample.State.On);
            WhiteTheme();
        }
        
        Theme.Instance.Change();
    }

    private void SliderSample_OnDisableSlider()
    {
        PlayerPrefs.SetString("Theme", "Black");
        BlackTheme();
        Theme.Instance.Change();
    }

    private void SliderSample_OnEnableSlider()
    {
        PlayerPrefs.SetString("Theme", "White");
        WhiteTheme();
        Theme.Instance.Change();
    }

    private void BlackTheme()
    {
        _text.GetComponent<Text>().text = "Тёмная";
        Theme.Instance.NameTheme = "Black";
    }

    private void WhiteTheme()
    {
        _text.GetComponent<Text>().text = "Светлая";
        Theme.Instance.NameTheme = "White";
    }

    private void OnDestroy()
    {
        _sliderSampleScript.OnEnableSlider -= SliderSample_OnEnableSlider;
        _sliderSampleScript.OnDisableSlider -= SliderSample_OnDisableSlider;
    }
}
