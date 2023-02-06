using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Button Extended State", 31)]
public class ExtendedButtonState : Button
{
    protected override void OnEnable()
    {
        base.OnEnable();
        if (Application.isPlaying)
        {
            OnChangeTheme();
            Theme.Instance.OnChangeImage += OnChangeTheme;
        }
    }

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();
        transition = Transition.SpriteSwap;
        var spriteState = new SpriteState();
        spriteState.highlightedSprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Resources/White/Button/Highlighted.png");
        spriteState.pressedSprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Resources/White/Button/Pressed.png");
        spriteState.selectedSprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Resources/White/Button/Selected.png");
        this.spriteState = spriteState;
    }
#endif

    protected void OnChangeTheme()
    {
        var spriteState = new SpriteState();
        spriteState.highlightedSprite = LoadSprite(this.spriteState.highlightedSprite);
        spriteState.pressedSprite = LoadSprite(this.spriteState.pressedSprite);
        spriteState.selectedSprite = LoadSprite(this.spriteState.selectedSprite);
        this.spriteState = spriteState;
    }

    protected Sprite LoadSprite(Sprite sprite)
    {
        if (sprite == null)
        {
            return null;
        }
        /* Ќе совсем правильное название, но пока так */
        Sprite mewSprite = Resources.Load<Sprite>(Path.Combine(Theme.Instance.nameTheme, "Button", sprite.name));
        if (mewSprite == null)
            throw new ArgumentException(gameObject.name);
        return mewSprite;
    }

    protected override void OnDisable()
    {
        if (Application.isPlaying)
            Theme.Instance.OnChangeImage -= OnChangeTheme;
    }
}
