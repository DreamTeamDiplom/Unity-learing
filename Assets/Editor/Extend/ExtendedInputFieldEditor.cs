using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;

[CanEditMultipleObjects]
[CustomEditor(typeof(ExtendedInputField), true)]
public class ExtendedInputFieldEditor : InputFieldEditor
{
    SerializedProperty m_RightImage;
    SerializedProperty m_StateInput;

    protected override void OnEnable()
    {
        base.OnEnable();
        m_RightImage = serializedObject.FindProperty("m_RightImage");
        m_StateInput = serializedObject.FindProperty("m_StateInput");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_RightImage);
        EditorGUILayout.PropertyField(m_StateInput);

        EditorGUILayout.Space();

        base.OnInspectorGUI();

        serializedObject.ApplyModifiedProperties();
    }
}

