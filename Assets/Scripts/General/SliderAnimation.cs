using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SliderSample))]
public class SliderAnimation : MonoBehaviour
{
    [SerializeField] private GameObject _text;

    private MenuScript _menuScript;
    private SliderSample _sliderSampleScript;
    private void Awake()
    {
        _sliderSampleScript = GetComponent<SliderSample>();
        _sliderSampleScript.OnEnableSlider += SliderSample_OnEnableSlider;
        _sliderSampleScript.OnDisableSlider += SliderSample_OnDisableSlider;

        if (CurrentProfile.Profile.Setting.Animation)
        {
            _sliderSampleScript.InitSlider(SliderSample.State.On);
            SliderSample_OnEnableSlider();
        }
        else
        {
            _sliderSampleScript.InitSlider(SliderSample.State.Off);
            SliderSample_OnDisableSlider();
        }

        _menuScript = FindObjectOfType<MenuScript>();
    }

    private void SliderSample_OnDisableSlider()
    {
        if (_menuScript != null)
        {
            _menuScript.IsAnimation = false;
        }
        PlayerPrefs.SetInt("Animation", 0);
        OffTurnAnimation();
    }

    private void SliderSample_OnEnableSlider()
    {
        if (_menuScript != null)
        {
            _menuScript.IsAnimation = true;
        }
        PlayerPrefs.SetInt("Animation", 1);
        OnTurnAnimation();
    }

    private void OffTurnAnimation()
    {
        _text.GetComponent<Text>().text = "Анимация выключена";
        CurrentProfile.Profile.Setting.Animation = false;
    }

    private void OnTurnAnimation()
    {
        _text.GetComponent<Text>().text = "Анимация включена";
        CurrentProfile.Profile.Setting.Animation = true;
    }

    private void OnDestroy()
    {
        _sliderSampleScript.OnEnableSlider -= SliderSample_OnEnableSlider;
        _sliderSampleScript.OnDisableSlider -= SliderSample_OnDisableSlider;
    }
}
