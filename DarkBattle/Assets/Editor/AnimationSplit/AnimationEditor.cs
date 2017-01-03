using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.IO;


[CustomEditor(typeof(Animation), true)]
public class AnimationEditor : Editor
{
    
    private string psth;
    private Animation ani;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("导入所有动画"))
        {
            string digitAniName = ani.gameObject.name.Substring(0, 5);
            int id = 0;
            int.TryParse(digitAniName, out id);
            psth = EditorUtility.OpenFolderPanel("选择动画存放的文件夹", GetPathByPlayId(id), "");
            if (!string.IsNullOrEmpty(psth))
            {
                NGUISettings.currentPath = psth;
                DirectoryInfo info = new DirectoryInfo(psth);
                FileInfo[] fis = info.GetFiles();
                List<AnimationClip> clipsList = new List<AnimationClip>();
                ani.clip = null;
                for (int i = 0;i<fis.Length;i++)
                {
                    FileInfo fi = fis[i];
                    if (fi.Name.EndsWith(".anim"))
                    {
                        string path = fi.FullName.Replace(Application.dataPath.Replace("/", "\\"), "Assets");
                        AnimationClip clip = AssetDatabase.LoadMainAssetAtPath(path) as AnimationClip;
                        if (clip != null)
                        {
                            if (ani.clip == null)
                            {
                                ani.clip = clip;
                            }
                            clipsList.Add(clip);
                        }
                    }
                }
                AnimationUtility.SetAnimationClips(ani, clipsList.ToArray());
                EditorUtility.UnloadUnusedAssets();
            }
        }
    }
    string GetPathByPlayId(int id)
    {
        switch (id)
        {
            case 20001:
                return Application.dataPath + "/Muffin/ResourcesRaw/Fighters/YinTianChou/AnimationClips";
            case 20002:
                return Application.dataPath + "/Muffin/ResourcesRaw/Fighters/PanGuanJi/AnimationClips";
            case 20003:
                return Application.dataPath + "/Muffin/ResourcesRaw/Fighters/GuiMianJunZi/AnimationClips";
            case 20004:
                return Application.dataPath + "/Muffin/ResourcesRaw/Fighters/SunYuan/AnimationClips";
            case 20005:
                return Application.dataPath + "/Muffin/ResourcesRaw/Fighters/XiaoHongChen/AnimationClips";
            case 20006:
                return Application.dataPath + "/Muffin/ResourcesRaw/Fighters/LuNa/AnimationClips";
            case 20007:
                return Application.dataPath + "/Muffin/ResourcesRaw/Fighters/JinXiaoXi/AnimationClips";
            case 20008:
                return Application.dataPath + "/Muffin/ResourcesRaw/Fighters/OuYangXuanHua/AnimationClips";
            case 20009:
                return Application.dataPath + "/Muffin/ResourcesRaw/Fighters/QuanFaNiang/AnimationClips";
            case 20010:
                return Application.dataPath + "/Muffin/ResourcesRaw/Fighters/YeChaLuo/AnimationClips";
            case 20011:
                return Application.dataPath + "/Muffin/ResourcesRaw/Fighters/JiWuShuang/AnimationClips";
            case 20016:
                return Application.dataPath + "/Muffin/ResourcesRaw/Fighters/ShaYiLong/AnimationClips";
            case 20022:
                return Application.dataPath + "/Muffin/ResourcesRaw/Fighters/MuTouRen/AnimationClips";
            default:
                return NGUISettings.currentPath;
        }
    }
    void OnEnable ()
	{
        if (ani == null)
        {
            ani = target as Animation;
        }
        if (ani != null)
        {
            if (ani.playAutomatically)
            {
                ani.playAutomatically = false;
            }
        }
    }


}
