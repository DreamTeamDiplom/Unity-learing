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
    private bool _isAnimation;

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
        yield return new WaitForSeconds(0.5f);
        _position = GetComponent<RectTransform>().localPosition;
        _tweenHover = transform.DOLocalMove(_position + Vector2.up, 0.2f).SetLoops(-1, LoopType.Yoyo).Pause();
        _tweenClick = transform.DOScale(new Vector2(0.8f, 0.8f), 0.2f).SetLoops(2, LoopType.Yoyo).SetAutoKill(false).Pause();
    }

    private bool GetAnimation()
    {
        if (PlayerPrefs.HasKey("Animation"))
        {
            return PlayerPrefs.GetInt("Animation") == 1;
        }
        else
        {
            return true;
        }
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        if (GetAnimation())
        {
            _tweenHover.Play();
        }
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

