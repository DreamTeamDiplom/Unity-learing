using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Create Data", order = 51)]
public class ObjectProfiles : ScriptableObject
{
    private string _pathSaveData;
    [SerializeField] private List<Profile> _profiles;

    public List<Profile> Profiles => _profiles;

    /// <summary>
    /// Загрузка данных
    /// </summary>
    public void LoadData()
    {
        _pathSaveData = Application.persistentDataPath + "/SaveData.dat";
        if (File.Exists(_pathSaveData))
            using (FileStream file = File.Open(_pathSaveData, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                _profiles = (List<Profile>)bf.Deserialize(file);
                file.Close();
            }
        else
        {
            _profiles = new List<Profile>();
        }
    }
    /// <summary>
    /// Добавляем профиль
    /// </summary>
    /// <param name="profile"></param>
    public void AddProfile(Profile profile)
    {
        _profiles.Add(profile);
        SaveData();
    }
    /// <summary>
    /// Сохранение данных
    /// </summary>
    public void SaveData()
    {
        using (FileStream file = File.Open(_pathSaveData, FileMode.OpenOrCreate))
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, _profiles);
            file.Close();
        }
    }
    /// <summary>
    /// Добавление курса пользователю
    /// </summary>
    /// <param name="course">Курс</param>
    public void AddCourse(Course course)
    {
        AddCourse(new ProfileCourse(course));
    }

    /// <summary>
    /// Добавление курса пользователю
    /// </summary>
    /// <param name="course">Курс</param>
    public void AddCourse(ProfileCourse course)
    {
        var profile = _profiles.Find(x => x.PathFolder == CurrentProfile.Profile.PathFolder);
        profile.Courses.Add(course);
        var fromDir = Path.Combine(Application.streamingAssetsPath, "Courses", course.Course.Title);
        var toDir = Path.Combine(CurrentProfile.Profile.PathFolder, course.Course.Title);
        CopyDir(fromDir, toDir);
        SaveData();
    }

    private void CopyDir(string fromDir, string toDir)
    {
        Directory.CreateDirectory(toDir);
        foreach (string s1 in Directory.GetFiles(fromDir).Where(f => f.IndexOf(".meta") == -1))
        {
            string s2 = Path.Combine(toDir, Path.GetFileName(s1));
            File.Copy(s1, s2);
        }
        foreach (string s in Directory.GetDirectories(fromDir))
        {
            CopyDir(s,  Path.Combine(toDir, Path.GetFileName(s)));
        }
    }

    /// <summary>
    /// Удаление пользователя
    /// </summary>
    /// <param name="profile"></param>
    public void DeleteProfile(Profile profile)
    {
        _profiles.Remove(profile);
        SaveData();
    }
}
