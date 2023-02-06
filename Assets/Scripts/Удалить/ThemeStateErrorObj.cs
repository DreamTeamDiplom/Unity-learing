using UnityEngine;
using UnityEngine.UI;

public class ThemeStateErrorObj : ThemeStateObj
{
    [SerializeField] protected ThemeState _stateError;
    [SerializeField] protected ThemeState _stateHighlightedError;
    [SerializeField] protected ThemeState _stateSelectedError;

    protected new void Awake()
    {
        Theme_OnChangeState();
    }

    public new void Theme_OnChangeState()
    {
        _stateNormal.OnChangeState -= Theme_OnChangeStateWarning;
        _stateNormal.OnChangeState += Theme_OnChangeState;
        base.Theme_OnChangeState();
    }

    public void Theme_OnChangeStateWarning()
    {
        _stateNormal.OnChangeState -= Theme_OnChangeState;
        _stateNormal.OnChangeState += Theme_OnChangeStateWarning;
        GetComponent<Image>().sprite = LoadSprite(_stateError);

        var spriteState = new SpriteState();
        spriteState.highlightedSprite = LoadSprite(_stateHighlightedError);
        spriteState.selectedSprite = LoadSprite(_stateSelectedError);
        GetComponent<InputField>().spriteState = spriteState;
    }
    protected new void OnDestroy()
    {
        _stateNormal.OnChangeState -= base.Theme_OnChangeState;
        _stateNormal.OnChangeState -= Theme_OnChangeState;
        _stateNormal.OnChangeState -= Theme_OnChangeStateWarning;
    }
}
