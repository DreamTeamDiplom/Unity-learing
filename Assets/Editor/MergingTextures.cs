using System.IO;
using UnityEditor;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MergingTextures
{
    [MenuItem("Assets/Merge Textures")]
    static void MergeTextures()
    {
        var offset = 25;
        //var offsetY = 5;

        var images = Selection.objects;

        int maxHeight = 0;
        int widthTexture = 0;
        foreach (var image in images) {
            Texture2D texture = (Texture2D)image;
            maxHeight = maxHeight < texture.height ? texture.height: maxHeight;
            widthTexture += texture.width + offset;
            
        }
        Texture2D newTexture =  new Texture2D(2 * offset + widthTexture, 2 * offset + maxHeight);
        for (int x = 0; x < newTexture.width; x++)
        {
            for (int y = 0; y < newTexture.height; y++)
            {
                newTexture.SetPixel(x, y, new Color(0, 0, 0, 0));
            }
        }
        newTexture.name = "Merge Sprite";

        widthTexture = 0;

        foreach (var image in images)
        {
            Texture2D texture = (Texture2D)image;
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    newTexture.SetPixel(offset + widthTexture + x, offset + y, texture.GetPixel(x, y));
                }
            }
            widthTexture += texture.width + offset;
        }

        byte[] bytes = newTexture.EncodeToPNG();
        var filePath = "Assets/Merge Sprite.png";
        FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
        BinaryWriter writer = new BinaryWriter(stream);
        for (int i = 0; i < bytes.Length; i++) {
            writer.Write(bytes[i]);
        }
        writer.Close();
        stream.Close();
        AssetDatabase.Refresh();
    }
}