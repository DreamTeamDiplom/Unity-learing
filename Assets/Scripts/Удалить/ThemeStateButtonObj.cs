using System;
using UnityEngine;
using UnityEngine.UI;

public class ThemeStateButtonObj : MonoBehaviour
{
    [SerializeField] protected ThemeState _stateNormal;
    [SerializeField] protected ThemeState _stateHighlighted;
    [SerializeField] protected ThemeState _statePressed;
    [SerializeField] protected ThemeState _stateSelected;

    protected void Awake()
    {
        Theme_OnChangeState();
        _stateNormal.OnChangeState += Theme_OnChangeState;
    }

    public void Theme_OnChangeState()
    {
        GetComponent<Image>().sprite = LoadSprite(_stateNormal);

        var spriteState = new SpriteState();
        spriteState.highlightedSprite = LoadSprite(_stateHighlighted);
        spriteState.pressedSprite = LoadSprite(_statePressed);
        spriteState.selectedSprite = LoadSprite(_stateSelected);
        GetComponent<Button>().spriteState = spriteState;
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
