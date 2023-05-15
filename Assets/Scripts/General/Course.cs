using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Course
{
    [SerializeField][HideInInspector] private string title;
    [SerializeField][HideInInspector] private string description;
    [SerializeField][HideInInspector] private string pathIcon;
    [SerializeField][HideInInspector] private List<Lesson> lessons;
    [NonSerialized] private Sprite icon;

    public string Title
    {
        get => title;
    }
    public string Description
    {
        get => description;
    }
    public string PathIcon
    {
        get => pathIcon;
    }
    public List<Lesson> Lessons
    {
        get => lessons;
    }
    public Sprite Icon
    {
        get => icon;
        set
        {
            if (value != null)
            {
                icon = value;
            }
        }
    }

    public Lesson LastLesson
    {
        get => lessons.Where(l => !l.Finished).FirstOrDefault();
    }
    public bool Finished
    {
        get => LastLesson.IsUnityNull();
    }

    public override string ToString()
    {
        return string.Format("{0}, {1}", title, Finished);
    }
}

[Serializable]
public class Lesson
{
    [SerializeField][HideInInspector] private string title;
    [SerializeField][HideInInspector] private string[] content;
    [SerializeField][HideInInspector] private string videoUrl;
    private bool finished = false;

    public string Title
    {
        get => title;
    }
    public string[] Content
    {
        get => content;
    }
    public string VideoUrl
    {
        get => videoUrl;
    }
    public bool Finished
    {
        get => finished;
        set => finished = value;
    }
    public override string ToString()
    {
        return string.Format("{0}, {1}", title, finished);
    }
}