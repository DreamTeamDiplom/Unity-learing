using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEditor;

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

            onValueChanged.AddListener((vector) =>
            {
                ShowScroll();
                HideScroll();
            });
            verticalNormalizedPosition = 1;
        }

    }

    private void Update()
    {
        if (Application.isPlaying)
        {
            if (Input.GetKeyUp(KeyCode.End))
            {
                this.DOVerticalNormalizedPos(0, .5f);
            }
            else if (Input.GetKeyUp(KeyCode.Home))
            {
                this.DOVerticalNormalizedPos(1, .5f);
            }
            else if (Input.GetKeyUp(KeyCode.PageDown))
            {
                float viewportHeight = viewport.rect.height;
                float contentHeight = content.rect.height - viewportHeight;
                var newVerticalNormalizedPosition = verticalNormalizedPosition - viewportHeight / contentHeight;
                this.DOVerticalNormalizedPos(newVerticalNormalizedPosition < 0 ? 0 : newVerticalNormalizedPosition, .5f);

            }
            else if (Input.GetKeyUp(KeyCode.PageUp))
            {
                float viewportHeight = viewport.rect.height;
                float contentHeight = content.rect.height - viewportHeight;
                var newVerticalNormalizedPosition = verticalNormalizedPosition + viewportHeight / contentHeight;
                this.DOVerticalNormalizedPos(newVerticalNormalizedPosition > 1 ? 1 : newVerticalNormalizedPosition, .5f);
            }
            else
            {
                float verticalInput = Input.GetAxis("Vertical");
                float contentHeight = content.rect.height;
                float scrollAmount = Mathf.Min(scrollSensitivity / contentHeight, 1f);
                verticalNormalizedPosition -= verticalInput * scrollAmount;
            }
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
        ShowScroll();
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        HideScroll();
    }

    private void ShowScroll()
    {
        scroll.SetActive(true);
        scroll.GetComponent<ExtendedImage>().DOFade(1, 0.5f);
    }

    private void HideScroll()
    {
        if (!co.IsUnityNull())
            StopCoroutine(co);
        co = StartCoroutine(Active());
    }
}
