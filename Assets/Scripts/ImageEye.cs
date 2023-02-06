using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ImageEye : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;

#if UNITY_EDITOR
    protected void Reset()
    {
        sprites = new Sprite[2];
        sprites[0] = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Resources/White/eye-off.png");
        sprites[1] = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Resources/White/eye-on.png");
    }
#endif
    public void OnClick()
    {
        var inputField = gameObject.GetComponent<InputField>();
        if (inputField.contentType == InputField.ContentType.Password)
        {
            inputField.contentType = InputField.ContentType.Standard;
            gameObject.transform.GetChild(3).GetComponent<Image>().sprite = Resources.Load<Sprite>(Path.Combine(PlayerPrefs.GetString("Theme"), sprites[0].name));
        }
        else
        {
            inputField.contentType = InputField.ContentType.Password;
            gameObject.transform.GetChild(3).GetComponent<Image>().sprite = Resources.Load<Sprite>(Path.Combine(PlayerPrefs.GetString("Theme"), sprites[1].name));
        }
        inputField.ForceLabelUpdate();
    }
}
