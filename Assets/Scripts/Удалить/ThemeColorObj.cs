using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeColorObj : MonoBehaviour
{
    public ThemeColor themeColor;
    private void Awake()
    {
        Theme_OnChangeImage();
        themeColor.OnChangeColor += Theme_OnChangeImage;
    }

    private void Theme_OnChangeImage()
    {
        Material material = Resources.Load<Material>(themeColor);
        if (material == null)
            throw new ArgumentException(gameObject.name);
        GetComponent<Text>().material = material;
    }

    private void OnDestroy()
    {
        themeColor.OnChangeColor -= Theme_OnChangeImage;
    }
}
