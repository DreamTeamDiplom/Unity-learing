using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckingInput : MonoBehaviour
{
    private RectTransform rectTransf;
    private ExtendedInputField extendedInput;
    public GameObject warningText;
    //public Sprite[] sprites;
    public int minHeight = 40, maxHeight = 55;

    private void Start()
    {
        rectTransf = gameObject.GetComponent<RectTransform>();
        extendedInput = gameObject.GetComponentInChildren<ExtendedInputField>();
    }

    //Check - нет ошибок
    /// <summary>
    /// Проверка input на ошибки
    /// </summary>
    /// <param name="check">Условие для проверки input</param>
    /// <returns>Есть ли ошибка</returns>
    public bool Checking(bool check)
    {
        if (rectTransf == null)
        {
            return false;
        }
        var sizeDelta = rectTransf.sizeDelta;
        if (check)
        {
            rectTransf.sizeDelta = new Vector2(sizeDelta.x, minHeight);
            extendedInput.stateInput = ExtendedInputField.StateInput.Normal;
        }
        else
        {
            rectTransf.sizeDelta = new Vector2(sizeDelta.x, maxHeight);
            extendedInput.stateInput = ExtendedInputField.StateInput.Error;

        }
        extendedInput.OnChangeTheme();
        warningText.SetActive(!check);
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
        return !check;
    }

}
