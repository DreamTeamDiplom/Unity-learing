using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

[Serializable]
public class ProfileCourse
{
    public ProfileCourse(Course course)
    {
        this.course = course;
    }
    private Course course;

    public Course Course
    {
        get => course;
    }
    public Lesson LastLesson
    {
        get => course.Lessons.Where(l => !l.Finished).FirstOrDefault();
    }
    public bool Finished
    {
        get => LastLesson.IsUnityNull();
    }

    public override string ToString()
    {
        return string.Format("{0}, {1}, {2}", course.Title, LastLesson.Title, Finished);
    }
}