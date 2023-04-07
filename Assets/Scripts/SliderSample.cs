using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSample: MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private GameObject _circle;
    [SerializeField] private GameObject _background;
    public enum State
    {
        On,
        Off
    }

    private State state = State.Off;

    public delegate void Slider();

    public event Slider OnDisableSlider;
    public event Slider OnEnableSlider;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => ChangeState());
    }

    /// <summary>
    /// Инициализация Slider с начальным состоянием
    /// </summary>
    /// <param name="state"></param>
    public void InitSlider(State state)
    {
        if (state == State.On)
        {
            EnableSlider(0);
        }
        else
        {
            DisableSlider(0);
        }
        this.state = state;
    }

    /// <summary>
    /// Изменение состояния Slider
    /// </summary>
    public void ChangeState()
    {
        if (state == State.On)
        {
            state = State.Off;
            DisableSlider(.5f);
        }
        else
        {
            state = State.On;
            EnableSlider(.5f);
        }
    }

    private void DisableSlider(float time)
    {
        _circle.transform.DOLocalMoveX(-8, time);
        _background.GetComponent<Image>().DOColor(Color.white, time).SetEase(Ease.Linear);
        if (OnDisableSlider != null)
        {
            OnDisableSlider();
        }
    }

    private void EnableSlider(float time)
    {
        _circle.transform.DOLocalMoveX(8, time);
        _background.GetComponent<Image>().DOColor(new Color(0.1490196f, 0.9960784f, 0.3372549f), time).SetEase(Ease.Linear);
        if (OnEnableSlider != null)
        {
            OnEnableSlider();
        }
    }
}
