using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[AddComponentMenu("UI/Legacy/Input Field Extended", 103)]
public class ExtendedInputField : InputField
{
    private Image extendedImage;
    public enum StateInput
    {
        Error,
        Normal
    }

    [SerializeField]
    [FormerlySerializedAs("right Image")]
    protected Image m_RightImage;

    [SerializeField]
    protected StateInput m_StateInput = StateInput.Normal;


    public Image RightImage
    {
        get => m_RightImage;
        set => m_RightImage = value;
    }

    public StateInput stateInput
    {
        get => m_StateInput;
        set => m_StateInput = value;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        transition = Transition.SpriteSwap;
        if (Application.isPlaying)
        {
            extendedImage = GetComponent<Image>();
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
        spriteState.highlightedSprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Resources/White/Input/Normal/Highlighted.png");
        spriteState.selectedSprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Resources/White/Input/Normal/Selected.png");
        this.spriteState = spriteState;
        selectionColor = new Color(0, 0, 0, .5f);
    }
#endif

    public void OnChangeTheme()
    {
        extendedImage.sprite = LoadSprite(extendedImage.sprite);
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
        Sprite mewSprite = Resources.Load<Sprite>(Path.Combine(Theme.Instance.nameTheme, "Input", m_StateInput.ToString(), sprite.name));
        if (mewSprite == null)
            throw new ArgumentException(gameObject.name);
        return mewSprite;
    }

    protected override void OnDisable()
    {
        if (Application.isPlaying)
        {
            Theme.Instance.OnChangeImage -= OnChangeTheme;
        }
    }
}
