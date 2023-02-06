using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ThemeImageObj : MonoBehaviour
{
    public ThemeImage themeString;
    
    private void Awake()
    {
        Theme_OnChangeImage();
        themeString.OnChangeImage += Theme_OnChangeImage;
    }

    private void Theme_OnChangeImage()
    {
        Sprite sprite = Resources.Load<Sprite>(themeString);
        if (sprite == null)
            throw new ArgumentException(gameObject.name);
        GetComponent<Image>().sprite = sprite;
    }

    private void OnDestroy()
    {
        themeString.OnChangeImage -= Theme_OnChangeImage;
    }
}
