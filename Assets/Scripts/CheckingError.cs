using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckingError : MonoBehaviour
{
    [SerializeField] private GameObject _warningText;
    [SerializeField] private int _minHeight = 40, _maxHeight = 55;

    private RectTransform _rectTransform;
    private ExtendedInputField _extendedInputField;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _extendedInputField = GetComponentInChildren<ExtendedInputField>();
    }

    //Check - нет ошибок
    /// <summary>
    /// Проверка input на наличие ошибок
    /// </summary>
    /// <param name="check">Условие для проверки input</param>
    /// <returns>Есть ли ошибка</returns>
    public bool CheckingInput(bool check)
    {
        if (_rectTransform == null)
        {
            return false;
        }
        var sizeDelta = _rectTransform.sizeDelta;
        if (check)
        {
            _rectTransform.sizeDelta = new Vector2(sizeDelta.x, _minHeight);
            _extendedInputField.stateInput = ExtendedInputField.StateInput.Normal;
        }
        else
        {
            _rectTransform.sizeDelta = new Vector2(sizeDelta.x, _maxHeight);
            _extendedInputField.stateInput = ExtendedInputField.StateInput.Error;

        }
        _extendedInputField.OnChangeTheme();
        _warningText.SetActive(!check);
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
        return !check;
    }

    /// <summary>
    /// Проверка Icon на наличие картинки
    /// </summary>
    /// <param name="check">Условие для проверки icon</param>
    /// <returns></returns>
    public bool CheckingIcon(bool check)
    {
        if (_rectTransform == null)
        {
            return false;
        }
        var sizeDelta = _rectTransform.sizeDelta;
        if (check)
        {
            _rectTransform.sizeDelta = new Vector2(sizeDelta.x, _minHeight);
        }
        else
        {
            _rectTransform.sizeDelta = new Vector2(sizeDelta.x, _maxHeight);

        }
        _warningText.SetActive(!check);
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
        return !check;
    }

}
