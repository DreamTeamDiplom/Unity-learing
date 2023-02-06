using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ChangingText : MonoBehaviour
{
    [MenuItem("GameObject/Change/Text", false, -2)]
    public static void ChangeText()
    {
        var gameObject = Selection.activeGameObject;
        var oldComponent = gameObject.GetComponent<Text>();
        DestroyImmediate(oldComponent);
        gameObject.AddComponent<ExtendedText>();
    }

    [MenuItem("GameObject/Change/Image", false, -2)]
    public static void ChangeImage()
    {
        var gameObject = Selection.activeGameObject;
        var oldComponent = gameObject.GetComponent<Image>();
        DestroyImmediate(oldComponent);
        gameObject.AddComponent<ExtendedImage>();
    }

    [MenuItem("GameObject/Change/Button", false, -2)]
    public static void ChangeButton()
    {
        var gameObject = Selection.activeGameObject;
        var oldComponent = gameObject.GetComponent<Button>();
        DestroyImmediate(oldComponent);
        gameObject.AddComponent<ExtendedButton>();
    }

    [MenuItem("GameObject/Change/Button State", false, -2)]
    public static void ChangeButtonState()
    {
        var gameObject = Selection.activeGameObject;
        var oldComponent = gameObject.GetComponent<Button>();
        DestroyImmediate(oldComponent);
        gameObject.AddComponent<ExtendedButtonState>();
    }

    [MenuItem("GameObject/Change/Input Field", false, -2)]
    public static void ChangeInputField()
    {
        var gameObject = Selection.activeGameObject;
        var oldComponent = gameObject.GetComponent<InputField>();
        DestroyImmediate(oldComponent);
        gameObject.AddComponent<ExtendedInputField>();
    }
}
