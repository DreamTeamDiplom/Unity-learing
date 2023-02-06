using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("UI/Button Extended", 31)]
public class ExtendedButton : Button
{
    private Vector2 pos;
    private Tween click, hover;

    protected override void OnEnable()
    {
        if (Application.isPlaying)
        {
            base.OnEnable();
            StartCoroutine(Sleep());
        }
    }
    protected override void OnDisable()
    {
        hover.Pause();
    }

#if UNITY_EDITOR
    protected override void Reset()
    {
        transition = Transition.None;
    }
#endif

    private IEnumerator Sleep()
    {
        yield return new WaitForSeconds(0.5f);
        pos = GetComponent<RectTransform>().localPosition;
        hover = transform.DOLocalMove(pos + Vector2.up, 0.2f).SetLoops(-1, LoopType.Yoyo).Pause();
        click = transform.DOScale(new Vector2(0.8f, 0.8f), 0.2f).SetLoops(2, LoopType.Yoyo).SetAutoKill(false).Pause();
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        hover.Play();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        hover.Pause();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if (!click.IsPlaying())
            click.Restart();
    }

    //public override void OnSelect(BaseEventData eventData)
    //{
    //    base.OnSelect(eventData);
    //    hover.Play();
    //}

    //public override void OnDeselect(BaseEventData eventData)
    //{
    //    base.OnDeselect(eventData);
    //    hover.Pause();
    //}


}

