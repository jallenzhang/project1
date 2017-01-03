using UnityEngine;
using System.Collections;

public class CheckPointData : XMLData<CheckPointData>
{
    public static readonly string fileName = "xml/checkpoint";

    public static CheckPointData GetCheckPointDataByID(int id)
    {
        if (!CheckPointData.dataMap.ContainsKey(id))
            return null;

        CheckPointData checkpointData = CheckPointData.dataMap[id];

        return checkpointData;
    }

    /// <summary>
    /// 人物名
    /// </summary>
    public string name { get; protected set; }

    public int weight { get; protected set; }

}
