using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

[AddComponentMenu("UI/TextMeshPro Extended-Text (UI)", 11)]
public class ExtendedTextMeshPro : TextMeshProUGUI
{
    protected override void OnEnable()
    {
        base.OnEnable();
        OnChangeTheme();
        Theme.Instance.OnChangeImage += OnChangeTheme;
    }

    private void OnChangeTheme()
    {
        if (m_Material != null)
        {
            Material material = Resources.Load<Material>(Path.Combine(Theme.Instance.ThemeType.ToString(), m_Material.name));
            if (material == null)
                throw new ArgumentException(gameObject.name);
            color = material.color;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Theme.Instance.OnChangeImage -= OnChangeTheme;
    }
}
