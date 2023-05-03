using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewLesson : MonoBehaviour
{
    [SerializeField] private Text number;
    [SerializeField] private Text title;

    public void Init(int number, string title)
    {
        this.number.text = string.Format("Урок {0}", number);
        this.title.text = title;
    }
}
