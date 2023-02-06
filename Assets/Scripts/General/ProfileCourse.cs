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

//[Serializable]
//public class MyLesson
//{
//    public MyLesson(Lesson lesson)
//    {
//        this.lesson = lesson;
//        //title = lesson.title;
//        //description = lesson.description;
//        //url = lesson.url;
//    }

//    //public string title;
//    //public string description;
//    //public string url;
//    public Lesson lesson;
//    public bool finish = false;
    
//    public override string ToString()
//    {
//        return string.Format("{0}, {1}, {2}, {3}", lesson.Title, lesson.Description, lesson.url, finish);
//    }
//}