using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
[AddComponentMenu("UI/Image Extended", 12)]
public class ExtendedImage : Image
{
    protected override void OnEnable()
    {
        base.OnEnable();
        OnChangeTheme();
        Theme.Instance.OnChangeImage += OnChangeTheme;
    }

    public void OnChangeTheme()
    {
        if (sprite != null)
        {
            Sprite sprite = Resources.Load<Sprite>(Path.Combine(Theme.Instance.nameTheme, "Button", this.sprite.name));
            if (sprite == null)
            {
                sprite = Resources.Load<Sprite>(Path.Combine(Theme.Instance.nameTheme, this.sprite.name));
            }
            if (sprite == null)
                throw new ArgumentException(gameObject.name);
            GetComponent<Image>().sprite = sprite;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Theme.Instance.OnChangeImage -= OnChangeTheme;
    }
}
