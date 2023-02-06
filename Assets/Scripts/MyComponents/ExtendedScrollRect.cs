using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

[AddComponentMenu("UI/Scroll Rect Exrended", 37)]
public class ExtendedScrollRect : ScrollRect
{
    private GameObject scroll;

    private Coroutine co;
    private IEnumerator Active()
    {
        yield return new WaitForSeconds(2);
        scroll.GetComponent<ExtendedImage>().DOFade(0, 0.5f).OnComplete(() => scroll.SetActive(false));
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (Application.isPlaying)
        {
            if (verticalScrollbar == null)
            {
                return;
            }
            scroll = verticalScrollbar.handleRect.gameObject;
        }
    }

    public override void OnScroll(PointerEventData eventData)
    {
        base.OnScroll(eventData);
        OnBeginDrag(eventData);
        OnEndDrag(eventData);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        scroll.SetActive(true);
        scroll.GetComponent<ExtendedImage>().DOFade(1, 0.5f);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        if (!co.IsUnityNull())
            StopCoroutine(co);
        co = StartCoroutine(Active());
    }

    

}
