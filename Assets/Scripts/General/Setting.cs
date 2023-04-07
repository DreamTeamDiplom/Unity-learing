using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Setting
{
    private ThemeEnum theme = ThemeEnum.Black;
    private bool animation = true;

    public ThemeEnum Theme
    {
        get => theme;
        set => theme = value; 
    }
    public bool Animation
    {
        get => animation;
        set => animation = value;
    }

}
