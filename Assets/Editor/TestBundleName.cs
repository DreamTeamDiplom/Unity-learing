using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEditor;
using UnityEngine;

public class TestBundleName : MonoBehaviour
{
    [MenuItem("Assets/Testing")]
    public static void Test()
    {
        AssetBundle myLoadedAssetBundle = null;
        try
        {
            myLoadedAssetBundle
            = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, "AssetBundles", "test"));
            foreach (var name in myLoadedAssetBundle.GetAllAssetNames())
            {
                //File file = new File()
                //File file = new File(name);
                //Debug.Log(Path.GetDirectoryName(name));
                ZipFile.ExtractToDirectory(name, Application.streamingAssetsPath);
                //File.Copy(name, Application.streamingAssetsPath);
                //Debug.Log(name);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        finally
        {
            myLoadedAssetBundle.Unload(false);
        }
        
    }
}
