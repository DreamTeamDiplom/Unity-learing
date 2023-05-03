using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ViewProfile : MonoBehaviour
{
    [SerializeField] private new TextMeshProUGUI name;
    [SerializeField] private Image icon;
    [SerializeField] private Button button;

    public void Init(Profile profile, UnityAction call)
    {
        name.text = profile.Name;
        if (icon != null)
            icon.sprite = profile.Icon;

        button.onClick.AddListener(call);
    }
}
