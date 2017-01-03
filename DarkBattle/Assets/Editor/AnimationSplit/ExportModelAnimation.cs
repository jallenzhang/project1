using UnityEditor;
using UnityEngine;
using System.IO;
//%代表ctrl，#代表Shift，&代表Alt
/// <summary>
/// 导出模型动画，选中模型，右键鼠标，选择Export Model Animation，
/// 就会把模型的动画导出到模型同一目录下的AnimationClips文件夹里
/// </summary>
public class ExportModelAnimation
{

    [MenuItem("Muffin/pause &l")]
    //[MenuItem("Assets/Custom/断开关联到Resource &p", false, 0)]
    public static void pause()
    {
        EditorApplication.isPaused = !EditorApplication.isPaused;
    }
    [MenuItem("Muffin/下一步 &;")]
    //[MenuItem("Assets/Custom/断开关联到Resource &p", false, 0)]
    public static void Step()
    {
        EditorApplication.Step();// = !EditorApplication.isPaused;
    }
    /// <summary>
    /// 动画文件目录
    /// </summary>
    private const string ANIMATIONCLIPS_FOLDER = "AnimationClips";

    const string duplicatePostfix = "_c";


    [MenuItem("Assets/Export Model Animation/导出当前选中的动作")]
    private static void ExportThis()
    {
        ExportModelAnimation a = new ExportModelAnimation();
        a.Export();
    }
    //private   ModelImporterAnimationType mainModelImporter;
    //[MenuItem("Assets/Export Model Animation")]
    public void Export()
    {
        // Get selected AnimationClip
        Object activeObj = Selection.activeObject;
        string objPath = AssetDatabase.GetAssetPath(activeObj);

        ModelImporter modelImport = AssetImporter.GetAtPath(objPath) as ModelImporter;
        if (null == modelImport)
        {
            return;
        }

        //        ExportAnimations(modelImport, modelImport.animationType);

        //原来是可以合到一起的，因为模型对应的ImportObject会自动带上所有动画文件的clip//
        string objFolder = objPath.Substring(0, objPath.LastIndexOf('/'));
        string objName = objPath.Substring(objPath.LastIndexOf('/') + 1);

        //切分一下，看下是导出的动画，还是模型//
        string[] splitName = objName.Split('@');
        // Debug.Log(objName);
        if (splitName.Length > 1)
        {
            //带@的，即动画文件//
            ExportAnimations(modelImport, modelImport.animationType,modelImport.globalScale);
        }
        else
        {
            string folderPath = CreateAnimationClipsFolder(modelImport);
            if (null == folderPath)
            {
                return;
            }
            DirectoryInfo info = new DirectoryInfo(folderPath);
            if (info.Exists)
            {
                FileInfo[] fis = info.GetFiles();
                foreach (FileInfo fi in fis)
                {
                    if (fi.Exists)
                    {
                        fi.Delete();
                    }
                }
            }
            //mainModelImporter = modelImport.animationType;
            //不带@的，即模型//
            string searchPattern = objName.Substring(0, objName.LastIndexOf('.')) + "@*.FBX";
            string[] fileArr = Directory.GetFiles(objFolder, searchPattern);
            for (int i = 0; i < fileArr.Length; ++i)
            {
                ModelImporter tmpImport = AssetImporter.GetAtPath(fileArr[i]) as ModelImporter;
                tmpImport.animationType = modelImport.animationType;
                if (null != tmpImport)
                {
                    ExportAnimations(tmpImport, modelImport.animationType,modelImport.globalScale);
                }
            }
        }

        Debug.Log("Export animation clips is done");
    }

    //    private   void CopyAnimationClip()

    /// <summary>
    /// 创建模块里的AnimationClips目录，如果存在，不创建
    /// </summary>
    /// <param name="modelImport"></param>
    private string CreateAnimationClipsFolder(ModelImporter modelImport)
    {
        if (null == modelImport)
        {
            //路径非模型，返回//
            return null;
        }

        string modelPath = modelImport.assetPath;
        string parentFolder = modelPath.Substring(0, modelPath.LastIndexOf('/'));

        if (Directory.Exists(parentFolder))
        {
            return parentFolder + '/' + ANIMATIONCLIPS_FOLDER;
        }
        else
        {
            string guid = AssetDatabase.CreateFolder(parentFolder, ANIMATIONCLIPS_FOLDER);
            string folderPath = AssetDatabase.GUIDToAssetPath(guid);
            AssetDatabase.Refresh();

            return folderPath;
        }
    }

    /// <summary>
    /// 导出一个模型的所有动画
    /// </summary>
    /// <param name="modelImport">模型</param>
    /// <param name="animationType">导出的动画类型，如果为None，则用模型自己的动画类型</param>
    /// <returns>导出的动画</returns>
    private void ExportAnimations(ModelImporter modelImport, ModelImporterAnimationType animationType ,float scal)
    {
        if (null == modelImport)
        {
            //路径非模型，返回//
            return;
        }

        if (!modelImport.importAnimation)
        {
            //没有动画，也返回//
            return;
        }

        string folderPath = CreateAnimationClipsFolder(modelImport);
        if (null == folderPath)
        {
            return;
        }
        DirectoryInfo info = new DirectoryInfo(folderPath);
        if (!info.Exists)
        {
            info.Create();
        }
        //先把模型的动画类型保存//
        ModelImporterAnimationType modelAnimationType = modelImport.animationType;

        //转为Legacy，这样模型对应的import object才会有animations//
        modelImport.animationType = ModelImporterAnimationType.Legacy;
        modelImport.globalScale = scal;
        //Apply//
        AssetDatabase.ImportAsset(modelImport.assetPath, ImportAssetOptions.ImportRecursive);
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        //取到对应的import object//
        GameObject importObject = AssetDatabase.LoadMainAssetAtPath(modelImport.assetPath) as GameObject;
        if (null != importObject)
        {
            AnimationClip[] clips = AnimationUtility.GetAnimationClips(importObject);
            for (int i = 0; i < clips.Length; ++i)
            {
                AnimationClip dstClip = CopyClip(clips[i], folderPath);
                if (null != dstClip)
                {
#if UNITY_5
                    //DO NOTHING
#else
                    AnimationUtility.SetAnimationType(dstClip, animationType);
#endif
                }
            }
        }

        //还原//
        modelImport.animationType = modelAnimationType;
        //Apply//
        AssetDatabase.ImportAsset(modelImport.assetPath, ImportAssetOptions.ForceSynchronousImport);
    }

    /// <summary>
    /// 复制Clip到目录copyPath里，并返回复制后的clip
    /// </summary>
    /// <param name="srcClip">源</param>
    /// <param name="copyFolder">目标的文件夹</param>
    /// <returns></returns>
    private AnimationClip CopyClip(AnimationClip srcClip, string copyFolder)
    {
        AnimationClip dstClip = null;
        string copyPath = copyFolder + '/' + srcClip.name + ".anim";
        if (File.Exists(copyPath))
        {
            AssetDatabase.DeleteAsset(copyPath);
        }
        dstClip = new AnimationClip();
        dstClip.name = srcClip.name;
        AssetDatabase.CreateAsset(dstClip, copyPath);
        AssetDatabase.Refresh();
        if (null == dstClip)
        {
            return null;
        }

        // Copy curves from imported to copy
        //先拷贝浮点数据//
        EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(srcClip);
        for (int i = 0; i < bindings.Length; i++)
        {
            AnimationUtility.SetEditorCurve(dstClip, bindings[i],
                AnimationUtility.GetEditorCurve(srcClip, bindings[i]));
        }

        //再拷贝引用数据//
        bindings = AnimationUtility.GetObjectReferenceCurveBindings(srcClip);
        for (int i = 0; i < bindings.Length; ++i)
        {
            AnimationUtility.SetObjectReferenceCurve(dstClip, bindings[i],
                AnimationUtility.GetObjectReferenceCurve(srcClip, bindings[i]));
        }
        dstClip.frameRate = 30.0f;
        SetAnimationEvents(dstClip);
        return dstClip;
    }

    public virtual void SetAnimationEvents(AnimationClip dstClip)
    {
        AnimationEventManager animationEventManager = new AnimationEventManager(dstClip);
        animationEventManager.AddAnimationEvent(dstClip.length, "AnimationEventEnd");
        animationEventManager.SaveAnimationEvent();
    }

}