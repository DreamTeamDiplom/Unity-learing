using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ViewAllCourse : MonoBehaviour
{
    [SerializeField] private Text title;
    [SerializeField] private Image icon;
    [SerializeField] private Text numberIncomplete;
    [SerializeField] private Text numberCompleted;
    [SerializeField] private Button button;

    public string Title { get { return title.text; } }
    private string GetWordLesson(int number)
    {
        if (number > 10 && number < 20)
        {
            return string.Format("{0} уроков", number);
        }
        else
        {
            switch (number.ToString()[^1])
            {
                case '1':
                    return string.Format("{0} урок", number);
                case '2':
                case '3':
                case '4':
                    return string.Format("{0} урока", number);
                default:
                    return string.Format("{0} уроков", number);
            }
        }
    }

    public void Init(string title, Sprite icon, int numberIncomplete, int numberCompleted)
    {
        this.title.text = title;
        if (icon != null)
            this.icon.sprite = icon;
        this.numberCompleted.text = GetWordLesson(numberCompleted);
        this.numberIncomplete.text = GetWordLesson(numberIncomplete);
    }

    public void Init(string title, Sprite icon, int numberLesson)
    {
        this.title.text = title;
        if (icon != null)
            this.icon.sprite = icon;
        if (numberCompleted != null)
            numberCompleted.text = GetWordLesson(numberLesson);
        else
            numberIncomplete.text = GetWordLesson(numberLesson);
    }

    public void AddListener(UnityAction call)
    {
        button.onClick.AddListener(call);
    }
}
