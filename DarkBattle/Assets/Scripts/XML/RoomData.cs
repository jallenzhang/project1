using UnityEngine;
using System.Collections;

public class RoomData : XMLData<RoomData>
{

    public static readonly string fileName = "xml/room";

    public static RoomData GetRoomDataByID(int id)
    {
        if (!RoomData.dataMap.ContainsKey(id))
            return null;

        RoomData roomData = RoomData.dataMap[id];

        return roomData;
    }

    /// <summary>
    /// 人物名
    /// </summary>
    public string name { get; protected set; }

    public int weight { get; protected set; }
}
