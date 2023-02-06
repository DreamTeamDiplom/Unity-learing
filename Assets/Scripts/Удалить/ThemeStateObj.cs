using System;
using UnityEngine;
using UnityEngine.UI;

public class ThemeStateObj : MonoBehaviour
{
    [SerializeField] protected ThemeState _stateNormal;
    [SerializeField] protected ThemeState _stateHighlighted;
    [SerializeField] protected ThemeState _stateSelected;

    protected void Awake()
    {
        _stateNormal.OnChangeState += Theme_OnChangeState;
        Theme_OnChangeState();
    }

    public void Theme_OnChangeState()
    {
        GetComponent<Image>().sprite = LoadSprite(_stateNormal);

        var spriteState = new SpriteState();
        spriteState.highlightedSprite = LoadSprite(_stateHighlighted);
        spriteState.selectedSprite = LoadSprite(_stateSelected);
        GetComponent<InputField>().spriteState = spriteState;
    }


    protected Sprite LoadSprite(ThemeState nameSprite)
    {
        Sprite sprite = Resources.Load<Sprite>(nameSprite);
        if (sprite == null)
            throw new ArgumentException(gameObject.name);
        return sprite;
    }

    protected void OnDestroy()
    {
        _stateNormal.OnChangeState -= Theme_OnChangeState;
    }
}
