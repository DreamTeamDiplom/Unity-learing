using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("UI/Button Extended", 31)]
public class ExtendedButton : Button
{
    private Vector2 _position;
    private Tween _tweenClick, _tweenHover;

    protected override void Awake()
    {
        base.Awake();
        ColorBlock colorBlock = new ColorBlock();
        colorBlock.normalColor = Color.white;
        colorBlock.highlightedColor = new Color(0.8f, 0.8f, 0.8f, 1);
        colorBlock.pressedColor = new Color(0.6f, 0.6f, 0.6f, 1);
        colorBlock.selectedColor = colorBlock.normalColor;
        colorBlock.colorMultiplier = 1;
        colorBlock.fadeDuration = 0.1f;
        colors = colorBlock;
    }

    protected override void OnEnable()
    {
        if (Application.isPlaying)
        {
            base.OnEnable();
            StartCoroutine(InitAnimations());
        }
    }
    protected override void OnDisable()
    {
        _tweenHover.Pause();
    }

#if UNITY_EDITOR
    protected override void Reset()
    {
        transition = Transition.None;
    }
#endif

    private IEnumerator InitAnimations()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        _position = GetComponent<RectTransform>().localPosition;
        _tweenHover = transform.DOLocalMove(_position + Vector2.up, 0.2f).SetLoops(-1, LoopType.Yoyo).Pause();
        _tweenClick = transform.DOScale(new Vector2(0.9f, 0.9f), 0.2f).SetLoops(2, LoopType.Yoyo).SetAutoKill(false).Pause();
    }

    private bool GetAnimation()
    {
        return PlayerPrefs.GetInt("Animation", 1) == 1;
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (GetAnimation())
        {
            _tweenHover.Play();
            this.transition = Selectable.Transition.None;
        }
        else
        {
            this.transition = Selectable.Transition.ColorTint;
        }
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (GetAnimation())
        {
            _tweenHover.Pause();
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if (GetAnimation() && !_tweenClick.IsPlaying())
        {
            _tweenClick.Restart();
        }
            
    }

    
}

