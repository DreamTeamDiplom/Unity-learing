using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewProfileCourse : MonoBehaviour
{
    [Serializable]
    private class Indicator
    {
        public GameObject prefabUncomplete;
        public GameObject prefabComplete;
    }

    [SerializeField] private Text title;
    [SerializeField] private Image icon;
    [SerializeField] private Text titleLastLesson;
    [SerializeField] private Transform indicator;
    [SerializeField] private Indicator line;
    public void Init(string title, Sprite icon, int numberIncomplete, int numberCompleted, string titleLastLesson)
    {
        this.title.text = title;
        if (icon != null)
            this.icon.sprite = icon;

        for (int i = 0; i < numberCompleted; i++)
        {
            GameObject gameObjectIndicator = Instantiate(line.prefabComplete);
            gameObjectIndicator.transform.SetParent(indicator, false);
        }

        for (int i = 0; i < numberIncomplete; i++)
        {
            GameObject gameObjectIndicator = Instantiate(line.prefabUncomplete);
            gameObjectIndicator.transform.SetParent(indicator, false);
        }

        this.titleLastLesson.text = titleLastLesson;
    }

    public void Init(string title, Sprite icon)
    {
        this.title.text = title;
        if (icon != null)
            this.icon.sprite = icon;
    }
}
