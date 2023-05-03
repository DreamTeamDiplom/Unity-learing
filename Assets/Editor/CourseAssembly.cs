using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class CourseAssembly : ScriptableObject
{
    [MenuItem("Assets/Create Course")]
    private static void CreateCourse()
    {
        string pathCourse = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
        string outputPath = Path.Combine("Assets", "AssetBundles");

        AssetBundleBuild[] buildMap = new AssetBundleBuild[1];

        string[] courseAssets = Directory.GetFileSystemEntries(pathCourse).Where(s => !s.EndsWith(".meta")).ToArray();
        string nameCourse = Path.GetFileNameWithoutExtension(courseAssets.Where(c => c.EndsWith(".json")).First());
        buildMap[0].assetBundleName = nameCourse;

        buildMap[0].assetNames = courseAssets;

        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }
        BuildPipeline.BuildAssetBundles(outputPath, buildMap, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/Create Course", true)]
    public static bool CreateCourseValidate()
    {
        string pathCourse = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
        return Directory.Exists(pathCourse);
    }
}
