using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class ProfileCourse
{
    public ProfileCourse(Course course)
    {
        title = course.Title;
        lessons = course.Lessons.Select(l => new ProfileLesson(l)).ToList();
    }

    private string title;
    private List<ProfileLesson> lessons;
    public string Title
    {
        get => title;
    }

    public List<ProfileLesson> Lessons
    {
        get => lessons;
        //set => lessons = value;
    }
    public ProfileLesson LastLesson
    {
        get => lessons.FirstOrDefault(l => !l.Finished);
    }
    public bool Finished
    {
        get => LastLesson.IsUnityNull();
    }

    public override string ToString()
    {
        return string.Format("{0}, {1}, {2}", title, LastLesson.Title, Finished);
    }
}

[Serializable]
public class ProfileLesson
{
    public ProfileLesson(Lesson lesson)
    {
        title = lesson.Title;
    }

    private string title;
    private bool finished = false;

    public string Title
    {
        get => title;
        //set => title = value;
    }

    public bool Finished
    {
        get => finished;
        set => finished = value;
    }
}