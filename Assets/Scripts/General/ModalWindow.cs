using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModalWindow : MonoBehaviour
{
    [SerializeField] protected Button _close;
    private bool isAnumation = true;

    protected void Awake()
    {
        _close.onClick.AddListener(OnClose);
        gameObject.GetComponent<Button>().onClick.AddListener(OnClose);
        isAnumation = PlayerPrefs.GetInt("Animation", 1) == 1;
    }
    protected void OnEnable()
    {
        if (isAnumation)
        {
            transform.GetChild(0).localScale = Vector3.one;
            transform.GetChild(0).DOPunchScale(Vector3.one / 10, .5f, 1, 0);
        }
    }

    protected void OnClose() 
    {
        if (isAnumation)
        {
            transform.GetChild(0).DOScale(Vector3.zero, .2f).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
