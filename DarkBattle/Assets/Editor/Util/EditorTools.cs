using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

public class EditorTools
{
    #region 复制所有的层和Tag
    private static Transform ms_srcTagObj = null;
    [MenuItem("GameObject/Copy Layer And Tag")]
    private static void CopyLayerTagMenu()
    {
        ms_srcTagObj = Selection.activeTransform;
    }

    [MenuItem("GameObject/Paste Layer And Tag", true)]
    private static bool CopyLayerTagMenuV()
    {
        return null != ms_srcTagObj;
    }

    [MenuItem("GameObject/Paste Layer And Tag")]
    private static void PasteLayerTagMenu()
    {
        if (null == ms_srcTagObj)
        {
            return;
        }

        Transform dstObj = Selection.activeTransform;
        if (null == dstObj)
        {
            return;
        }

        CopyLayerTagInChildren(ms_srcTagObj, dstObj);

        ms_srcTagObj = null;
    }

    /// <summary>
    /// 把源对象的Layer和Tag拷贝到目标对象（包括子对象）
    /// </summary>
    /// <param name="src"></param>
    /// <param name="dst"></param>
    private static void CopyLayerTagInChildren(Transform src, Transform dst)
    {
        if (null == src || null == dst)
        {
            return;
        }

        //拷贝自己的//
        CopyLayerTag(src, dst);

        //拷贝孩子的//
        int len = src.childCount;
        for (int i = 0; i < len; ++i)
        {
            Transform srcChild = src.GetChild(i);
            if (null == srcChild)
            {
                continue;
            }

            Transform dstChild = dst.FindChild(srcChild.name);
            if (null == dstChild)
            {
                continue;
            }

            CopyLayerTagInChildren(srcChild, dstChild);
        }
    }

    /// <summary>
    /// 把源对象的Layer和Tag拷贝到目标对象（不包括子对象）
    /// </summary>
    /// <param name="src"></param>
    /// <param name="dst"></param>
    private static void CopyLayerTag(Transform src, Transform dst)
    {
        if (null == src || null == dst)
        {
            return;
        }

        dst.gameObject.layer = src.gameObject.layer;
        dst.tag = src.tag;
    }
    #endregion

    #region 复制所有部件
    /// <summary>
    /// 需要过滤，不复制的部件类型
    /// </summary>
    private static readonly Type[] FILTER_TYPES = new Type[]{ typeof(Transform), typeof(Animation)};
    private static Transform ms_srcObj = null;
    [MenuItem("GameObject/Copy Components")]
    private static void CopyComponentsMenu()
    {
        ms_srcObj = Selection.activeTransform;
    }

    [MenuItem("GameObject/Paste Components As New", true)]
    private static bool PasteComponentMenuV()
    {
        return null != ms_srcObj;
    }

    [MenuItem("GameObject/Paste Components As New")]
    private static void PasteComponentMenu()
    {
        if (null == ms_srcObj)
        {
            return;
        }

        Transform dstObj = Selection.activeTransform;
        if (null == dstObj)
        {
            return;
        }

        CopyAllComponentsInChildren(ms_srcObj, dstObj);

        ms_srcObj = null;
    }

    /// <summary>
    /// 把源对象的所有部件拷贝到目标对象（包括子对象）
    /// </summary>
    /// <param name="src"></param>
    /// <param name="dst"></param>
    private static void CopyAllComponentsInChildren(Transform src, Transform dst)
    {
        if (null == src || null == dst)
        {
            return;
        }

        //拷贝自己的//
        CopyAllComponents(src, dst);

        //拷贝孩子的//
        int len = src.childCount;
        for (int i = 0; i < len; ++i)
        {
            Transform srcChild = src.GetChild(i);
            if (null == srcChild)
            {
                continue;
            }

            Transform dstChild = dst.FindChild(srcChild.name);
            if (null == dstChild)
            {
                Debug.Log("EditorTools::CopyComponents->目标没有对应的骨骼，骨骼名=" + srcChild.name);
                continue;
            }

            CopyAllComponentsInChildren(srcChild, dstChild);
        }
    }

    /// <summary>
    /// 把源对象的所有部件拷贝到目标对象（不包括子对象）
    /// </summary>
    /// <param name="src"></param>
    /// <param name="dst"></param>
    private static void CopyAllComponents(Transform src, Transform dst)
    {
        if (null == src || null == dst)
        {
            return;
        }

        Component[] srcComps = src.GetComponents<Component>();
        for (int i = 0; i < srcComps.Length; ++i)
        {
            Component srcComp = srcComps[i];
            if (null == srcComp)
            {
                continue;
            }

            Type srcCmpType = srcComp.GetType();
            if (NeedFilter(srcCmpType))
            {
                continue;
            }

            //直接加个新的，因为有可能存在多个相同类型的部件//
            Component dstComp = dst.gameObject.AddComponent(srcCmpType);
            if (null == dstComp)
            {
                //加不上控件，说明unity不给加。。。//
                Debug.LogError("EditorTools::CopyAllComponents->增加控件失败！ObjectName=" + dst.name + " AddType=" + srcCmpType);
                continue;
            }

            EditorUtility.CopySerialized(srcComp, dstComp);
        }
    }

    /// <summary>
    /// 判断某类型是否需要过滤
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static bool NeedFilter(Type type)
    {
        if (null == type)
        {
            return true;
        }

        for (int i = 0; i < FILTER_TYPES.Length; ++i)
        {
            Type filterType = FILTER_TYPES[i];
            if (null != filterType && filterType == type)
            {
                return true;
            }
        }

        return false;
    }

    #endregion

    

    #region 根据名字获取该游戏对象下子物体
    /// <summary>
    ///  根据名字获取该游戏对象下子物体
    /// </summary>
    /// <param name="tan">主要对象</param>
    /// <param name="name">当前的名字</param>
    /// <returns></returns>
    public static Transform FindTransformInChild(Transform tan, string name)
    {
        if (tan == null)
        {
            Debug.LogError("EditorTools.FindTransformInChild -> tan Is Null + name =" + name);
            return null;
        }

        if (tan.name.Equals(name))
        {
            return tan;
        }

        int len = tan.childCount;
        if (len == 0)
        {
            return null;
        }

        for (int i = 0; i < len; i++)
        {
            Transform a = FindTransformInChild(tan.GetChild(i), name);
            if (a != null)
            {
                return a;
            }
        }

        return null;
    }
    #endregion
}
