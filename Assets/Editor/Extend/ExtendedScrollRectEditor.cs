using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(ExtendedScrollRect), true)]
public class ExtendedScrollRectEditor : ScrollRectEditor
{
    SerializedProperty isClickKey;

    protected override void OnEnable()
    {
        base.OnEnable();
        isClickKey = serializedObject.FindProperty("clickKey");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(isClickKey);
        serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();
    }
}
