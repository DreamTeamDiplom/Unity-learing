using DG.Tweening;
using SFB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChangeIcon : MonoBehaviour
{
    [SerializeField] private GameObject iconGameObject;
    [SerializeField] private Image mainIcon;
    public Sprite[] icons;
    //public ObjectProfiles objectProfiles;

    private int index;

    private void OnEnable()
    {
        index = -1;
        for (int i = 0; i < 3; i++)
        {
            iconGameObject.transform.GetChild(i).GetComponent<Image>().sprite = mainIcon.sprite;
        }
        iconGameObject.GetComponentInParent<CheckingError>().CheckingIcon(true);
        for (int i = 0; i < icons.Length; i++)
        {
            if (icons[i] == mainIcon.sprite)
            {
                index = i;
                break;
            }
        }
        if (index == -1 && icons.Length == 11)
        {
            var tempList = icons.ToList();
            tempList.Insert(icons.Length - 1, mainIcon.sprite);
            icons = tempList.ToArray();
            index = 10;
        }
        transform.GetChild(0).DOPunchScale(Vector3.one / 10, .5f, 1, 0);

    }

    private IEnumerator LoadTextureFromServer(string[] url, Action<Sprite> action)
    {
        if (url.Length != 0)
        {
            var request = UnityWebRequestTexture.GetTexture(url[0]);
            yield return request.SendWebRequest();
            var texture = DownloadHandlerTexture.GetContent(request);
            request.Dispose();

            Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            sprite.name = url[0];

            action(sprite);
        }
    }

    private void LoadTexture(Sprite sprite)
    {
        icons[^1] = sprite;
        iconGameObject.transform.GetChild(0).GetComponent<Image>().sprite = icons.Last();
        iconGameObject.transform.GetChild(2).GetComponent<Image>().sprite = icons.Last();
    }

    /// <summary>
    /// Функция для добавления пути иконки пользователя
    /// </summary>
    public void SelectPathIcon()
    {
        if (index < icons.Length - 1)
            return;
        var extensions = new[] {
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
            new ExtensionFilter("All Files", "*" ),
        };
        var pathIcon = StandaloneFileBrowser.OpenFilePanel("Выберите иконку для профиля", "", extensions, false);
        StartCoroutine(LoadTextureFromServer(pathIcon, LoadTexture));
    }

    public void LeftMove(int lengthShift)
    {
        iconGameObject.transform.localPosition = Vector3.zero;
        iconGameObject.transform.GetChild(1).GetComponent<Image>().sprite = icons[index];

        index = index == icons.Length - 1 ? 0 : index + 1;
        iconGameObject.transform.GetChild(2).GetComponent<Image>().sprite = icons[index];

        iconGameObject.transform.DOLocalMoveX(lengthShift, 0.5f).Restart();
    }

    public void RightMove(int lengthShift)
    {
        iconGameObject.transform.localPosition = Vector3.zero;
        iconGameObject.transform.GetChild(1).GetComponent<Image>().sprite = icons[index];

        index = index == 0 ? icons.Length - 1 : index - 1;
        iconGameObject.transform.GetChild(0).GetComponent<Image>().sprite = icons[index];

        iconGameObject.transform.DOLocalMoveX(lengthShift, 0.5f).Restart();
    }

    public void Change()
    {
        bool error = iconGameObject.GetComponentInParent<CheckingError>().CheckingIcon(icons[index].name != "AddPhoto");
        if (!error)
        {
            iconGameObject.transform.GetChild(1).GetComponent<Image>().sprite = icons[index];
            mainIcon.sprite = icons[index];
            gameObject.SetActive(false);
        }
    }
}
