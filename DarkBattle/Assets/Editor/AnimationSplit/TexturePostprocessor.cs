using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

class TexturePostprocessor : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        //cDebug.LogError("aaaaaaaaaaaaaaaaa")
        string path = assetPath.ToLower();

        TextureImporter ti = (TextureImporter)assetImporter;
        if (ti.filterMode != FilterMode.Bilinear)
        {
            //ti.filterMode = FilterMode.Bilinear;
        }

        if (path.Contains("fighters/"))
        {
//             if (path.Contains("ui/"))
//             {
//                 ti.maxTextureSize = 2048;
//                 ti.textureType = TextureImporterType.GUI;
//                 //ti.textureFormat = TextureImporterFormat.RGBA32;
//                 if (path.EndsWith("c"))
//                 {
//                     ti.textureFormat = TextureImporterFormat.RGBA16;
//                 }
//             }
        }
    }
                /*
            else if (path.EndsWith(".png") == true)
            {
                if (path.Contains("/public/") == true)
                {
                    ti.textureType = TextureImporterType.Advanced;
                    ti.npotScale = TextureImporterNPOTScale.ToNearest;
                    ti.generateCubemap = TextureImporterGenerateCubemap.None;
                    ti.isReadable = true;
                    ti.normalmap = false;
                    ti.lightmap = false;
                    ti.mipmapEnabled = false;
                    ti.spriteImportMode = SpriteImportMode.None;
                    ti.wrapMode = TextureWrapMode.Repeat;
                    ti.filterMode = FilterMode.Bilinear;
                    ti.anisoLevel = 0;
                    
                    if (path.Contains("/public/public") == true)
                    {
                        if (path.Contains("/public/publicui.png") == true)
                        {
                            ti.maxTextureSize = 4096;
                        } 
                        else
                        {
                            ti.maxTextureSize = 2048;
                        }
                        if (path.Contains("/public/publicui") == true)
                        {
                            ti.textureFormat = TextureImporterFormat.AutomaticTruecolor;
                        }
                        else
                        {
                            ti.textureFormat = TextureImporterFormat.AutomaticCompressed;
                        }
                    } 
                    else
                    {
                        ti.maxTextureSize = 2048;
                        ti.textureFormat = TextureImporterFormat.AutomaticCompressed;
                    }
                }
                else
                {
                    ti.textureType = TextureImporterType.Sprite;
                    ti.filterMode = FilterMode.Bilinear;
                    //ti.maxTextureSize = 2048;
                    ti.textureFormat = TextureImporterFormat.AutomaticCompressed;
                }
            }
            else if (path.EndsWith(".jpg") == true || path.EndsWith(".jpeg") == true)
            {
                ti.textureType = TextureImporterType.Advanced;
                ti.npotScale = TextureImporterNPOTScale.ToNearest;
                if (path.Contains("Map"))
                {
                    ti.npotScale = TextureImporterNPOTScale.ToSmaller;
                }
                ti.generateCubemap = TextureImporterGenerateCubemap.None;
                ti.isReadable = true;
                ti.normalmap = false;
                ti.lightmap = false;
                ti.mipmapEnabled = false;
                ti.spriteImportMode = SpriteImportMode.None;
                ti.wrapMode = TextureWrapMode.Repeat;
                ti.filterMode = FilterMode.Bilinear;
                ti.anisoLevel = 0;
                ti.maxTextureSize = 2048;
                ti.textureFormat = TextureImporterFormat.AutomaticCompressed;
            }
            */

    void OnPreprocessAudio()
    {
        string path = assetPath.ToLower();
        Debug.Log(path);
        AudioImporter ti = (AudioImporter)assetImporter;
        if (path.EndsWith(".wav"))
        {
            ti.threeD = false;
            //ti.format = AudioImporterFormat.Compressed;
            //ti.loadType = AudioClipLoadType.CompressedInMemory;
        }
    }
}
