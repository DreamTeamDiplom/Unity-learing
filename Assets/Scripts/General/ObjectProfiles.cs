using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO.Compression;
using UnityEditor;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Create Data", order = 51)]
public class ObjectProfiles : ScriptableObject
{
    [SerializeField] private List<Profile> _profiles;
    private string _pathSaveData;
    private AssetBundle loadedAssetBundle = null;

    public List<Profile> Profiles => _profiles;

    /// <summary>
    /// Загрузка данных
    /// </summary>
    public void LoadData()
    {
        _pathSaveData = Path.Combine(Application.persistentDataPath, "SaveData.dat");
        if (File.Exists(_pathSaveData))
            using (FileStream file = File.Open(_pathSaveData, FileMode.Open))
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    _profiles = (List<Profile>)bf.Deserialize(file);
                }
                catch (Exception) 
                {
                    file.Close();
                    File.Delete(_pathSaveData);
                }
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

        DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine(Application.streamingAssetsPath, "Courses"));

        FileInfo fileInfo = dirInfo.GetFiles().Where(f => !f.Name.EndsWith(".meta") && 
            f.Name.Contains(course.Title, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
        if (fileInfo == null)
        {
            /* Обработка незагруженного курса */
            return;
        }
        try
        {
            loadedAssetBundle = AssetBundle.LoadFromFile(fileInfo.FullName);
            var project = loadedAssetBundle.LoadAsset<TextAsset>("project");

            string archiveFilePath = Path.Combine(Application.temporaryCachePath, "Project.zip");
            string toDir = Path.Combine(CurrentProfile.Profile.PathFolder, course.Title);

            File.WriteAllBytes(archiveFilePath, project.bytes);
            if (File.Exists(archiveFilePath))
            {
                ZipFile.ExtractToDirectory(archiveFilePath, toDir);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        finally
        {
			if (loadedAssetBundle != null)
			{
				loadedAssetBundle.Unload(false);
			}
        }

        SaveData();
    }

    /// <summary>
    /// Удаление пользователя
    /// </summary>
    /// <param name="profile"></param>
    public void DeleteProfile(Profile profile)
    {
        _profiles.Remove(profile);
        Directory.Delete(profile.PathFolder, true);
        SaveData();
    }
}
