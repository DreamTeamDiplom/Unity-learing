using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
[AddComponentMenu("UI/Legacy/Text Extended", 101)]
public class ExtendedText : Text
{
    protected override void OnEnable()
    {
        base.OnEnable();
        OnChangeTheme();
        Theme.Instance.OnChangeImage += OnChangeTheme;
    }

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();
        font = UnityEditor.AssetDatabase.LoadAssetAtPath<Font>("Assets/Fonts/Inter.ttf");
        material = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>("Assets/Resources/White/Text.mat");
        raycastTarget = false;
    }
#endif
    private void OnChangeTheme()
    {
        if (m_Material != null)
        {
            Material material = Resources.Load<Material>(Path.Combine(Theme.Instance.NameTheme, m_Material.name));
            if (material == null)
                throw new ArgumentException(gameObject.name);
            this.material = material;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Theme.Instance.OnChangeImage -= OnChangeTheme;
    }
}
