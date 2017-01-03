using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class ProjectHelper {

	public static System.Type GetTypeFromBinary(string type)
    {
        //TextAsset ta = (TextAsset)Resources.Load("Bin/MyLogic.bytes");

        using (FileStream fStream = new FileStream("./Assets/Resources/Bin/LogicLib.bytes", FileMode.Open))
        {
            byte[] buffer = new byte[fStream.Length];
            fStream.Read(buffer, 0, (int)fStream.Length);

            System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(buffer);
            return assembly.GetType("BTreeWin");
        }
    }

    /// <summary>
    /// 算法：
    /// 1.每个广告项权重+1命名为w，防止为0情况。
    /// 2.计算出总权重n。
    /// 3.每个广告项权重w加上从0到(n-1)的一个随机数（即总权重以内的随机数），得到新的权重排序值s。
    /// 4.根据得到新的权重排序值s进行排序，取前面s最大几个。
    /// </summary>
    /// <param name="list">原始列表</param>
    /// <param name="count">随机抽取条数</param>
    /// <returns></returns>
    public static List<T> GetRandomList<T>(List<T> list, int count) where T : RandomObject
    {
        if (list == null || list.Count <= count || count <= 0)
        {
            return list;
        }

        //计算权重总和
        int totalWeights = 0;
        for (int i = 0; i < list.Count; i++)
        {
            totalWeights += list[i].Weight;  //权重
        }

        List<KeyValuePair<int, int>> wlist = new List<KeyValuePair<int, int>>();    //第一个int为list下标索引、第一个int为权重排序值
        for (int i = 0; i < list.Count; i++)
        {
            int w = (list[i].Weight) + Random.Range(0, totalWeights * (list[i].Weight));   // （权重） + 从0到（总权重 * 权重 -1）的随机数
            wlist.Add(new KeyValuePair<int, int>(i, w));
        }

        //排序
        wlist.Sort(
          delegate(KeyValuePair<int, int> kvp1, KeyValuePair<int, int> kvp2)
          {
              return kvp2.Value - kvp1.Value;
          });

        //根据实际情况取排在最前面的几个
        List<T> newList = new List<T>();
        for (int i = 0; i < count; i++)
        {
            T entiy = list[wlist[i].Key];
            newList.Add(entiy);
        }

        //随机法则
        return newList;
    }

    public static Vector3 NGUIPosToWorldPos(Camera NGUICamera, Camera worldCamera, Vector3 uiPos)
    {
        Vector3 pos = NGUICamera.WorldToScreenPoint(uiPos);
        pos.z = 1;
        pos = worldCamera.ScreenToWorldPoint(pos);
        return pos;
    }

    public static Vector3 WorldCameraToNGUIPos(Camera worldCamera, Camera NGUICamera, Vector3 worldPos)
    {
        Vector3 pos = worldCamera.WorldToScreenPoint(worldPos);
        pos = NGUICamera.ScreenToWorldPoint(pos);
        pos.z = 0;
        return pos;
    }

    /// <summary>
    /// default skill levels parse, the defaultUsedSkill should read from configure
    /// </summary>
    /// <param name="defaultUsedSkill"></param>
    /// <returns></returns>
    public static int[] InitSkillLevel(string defaultUsedSkill)
    {
        string[] skills = defaultUsedSkill.Split(':');
        int[] levels = new int[CommonDefine.RoleSkillCount];

        foreach (string skill in skills)
        {
            int index = int.Parse(skill);
            levels[index] = 1;
        }

        return levels;
    }

}
