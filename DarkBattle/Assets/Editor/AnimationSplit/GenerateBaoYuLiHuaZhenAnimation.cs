using UnityEngine;
using System.Collections;
using UnityEditor;

public class GenerateBaoYuLiHuaZhenAnimation 
{

    [MenuItem("Assets/Generate AnimationClip /Generate BaoYuLiHuaZhen Animation")]
    public static void ExportYuLuoCha()
    {
        UnityEngine.Object obj = Selection.activeObject;
        string path = AssetDatabase.GetAssetPath(obj);
        AnimationClip clip = AssetDatabase.LoadMainAssetAtPath(path) as AnimationClip;
        AnimationEventManager manager = new AnimationEventManager(clip);
        manager.AddAnimationEvent(4.3f, "OnBgFadeOutFinished");
        manager.SaveAnimationEvent();
    }
}
