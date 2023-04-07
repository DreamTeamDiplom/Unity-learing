using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class LoadProfileWindow : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _prefabProfile;

    [Header("Windows")]
    [SerializeField] private GameObject _windowAddProfile;
    [SerializeField] private GameObject _windowAuthProfile;

    [Space(10f)]
    [SerializeField] private GameObject _content;
    [SerializeField] private ObjectProfiles _dataProfiles;

    private AuthProfileWindow _authProfileScript;

    private void Awake()
    {
        _authProfileScript = _windowAuthProfile.GetComponentInChildren<AuthProfileWindow>();
        LoadData();
    }
    private void LoadData()
    {
        _dataProfiles.LoadData();

        foreach (var profile in _dataProfiles.Profiles)
        {
            var profileObj = Instantiate(_prefabProfile);
            Sprite iconProfile = Resources.Load<Sprite>("Icons/" + profile.PathIcon);
            if (iconProfile.IsUnityNull())
            {
                StartCoroutine(LoadTexture(profileObj, profile));
            }
            else
            {
                profile.Icon = iconProfile;
                SetSprite(profileObj, profile);
            }
            profileObj.GetComponentInChildren<TextMeshProUGUI>().text = profile.Name;
            profileObj.transform.SetParent(_content.transform, false);
        }
        StartCoroutine(ChangeSize());
    }
    private IEnumerator LoadTexture(GameObject profileObj, Profile profile)
    {
        var request = UnityWebRequestTexture.GetTexture(profile.PathIcon);
        yield return request.SendWebRequest();
        var texture = DownloadHandlerTexture.GetContent(request);
        var sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        sprite.name = profile.PathIcon;
        profile.Icon = sprite;
        SetSprite(profileObj, profile);
        request.Dispose();
    }
    private void SetSprite(GameObject profileObj, Profile profile)
    {
        profileObj.transform.GetChild(0).GetComponent<Image>().sprite = profile.Icon;
        profileObj.GetComponentInChildren<Button>().onClick.AddListener(() => AuthProfile(profile));
    }

    private void AuthProfile(Profile profile)
    {
        _authProfileScript.profile = profile;
        _windowAuthProfile.SetActive(true);
    }

    private IEnumerator ChangeSize()
    {
        yield return new WaitForEndOfFrame();

        Canvas.ForceUpdateCanvases();
        float heigthContent = _content.GetComponent<RectTransform>().sizeDelta.y;
        RectTransform rectList = _content.transform.parent.parent.GetComponent<RectTransform>();
        Vector2 size = rectList.sizeDelta;
        rectList.sizeDelta = heigthContent > 400f ? new Vector2(size.x, 400f) : new Vector2(size.x, heigthContent);
    }
}


