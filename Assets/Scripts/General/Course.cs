using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

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
        //set => title = value;
    }
    public string Description
    {
        get => description;
        //set => description = value;
    }
    public string PathIcon
    {
        get
        {
            return Path.Combine(new string[] { Application.streamingAssetsPath, "Courses", title, pathIcon });
        }
        //set => pathIcon = value;
    }
    public List<Lesson> Lessons
    {
        get => lessons;
        //set => lessons = value;
    }
    public Sprite Icon
    {
        get => icon;
        set => icon = value;
    }

    public override string ToString()
    {
        return string.Format("{0}, {1}", title, description);
    }
}

[Serializable]
public class Lesson
{
    [SerializeField][HideInInspector] private string title;
    [SerializeField][HideInInspector] private string[] description;
    [SerializeField][HideInInspector] private string videoUrl;
    private bool finished = false;

    public string Title
    {
        get => title;
        //set => title = value;
    }
    public string[] Description
    {
        get => description;
        //set => description = value;
    }
    public string VideoUrl
    {
        get => videoUrl;
    }
    public bool Finished
    {
        get => finished;
    }
    public override string ToString()
    {
        return string.Format("{0}, {1}, {2}", title, description[0], finished);
    }
}