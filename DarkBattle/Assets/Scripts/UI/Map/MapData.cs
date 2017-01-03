using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 路上数据，包括背景，路上的陷阱，开门进入的房间，出去的房间等等信息
/// </summary>
public class RoadData {
    /// <summary>
    /// 这条路的ID
    /// </summary>
    public int RoadID;
    /// <summary>
    /// 起点坐标
    /// </summary>
    public Vector2 Begin;
    /// <summary>
    /// 终点坐标
    /// </summary>
    public Vector2 End;

    /// <summary>
    /// 路起点所在的房间背景ID
    /// </summary>
    public int BeginRoomID;
    /// <summary>
    /// 路终点所在的房间背景ID
    /// </summary>
    public int EndRoomID;
    /// <summary>
    /// 路上的背景序列
    /// </summary>
    public List<int> Backgrounds;
    /// <summary>
    /// 路上的道具序列
    /// </summary>
    public List<RoadProp> Props;
    /// <summary>
    /// 进入这条路有几条路可以进入
    /// </summary>
    public List<int> WayIns;
    /// <summary>
    /// 出去这条路可以去几条路
    /// </summary>
    public List<int> WayOuts;
}

public class RoadProp
{
    /// <summary>
    /// 道具ID，可以从道具表里根据这个ID，取图片名，名字等等信息
    /// </summary>
    public int PropID;
    /// <summary>
    /// 这个道具在这条路上的位置
    /// </summary>
    public int Pos;
    /// <summary>
    /// 是否已开过
    /// </summary>
    public bool IsOpened;
    /// <summary>
    /// 
    /// </summary>
    public virtual void DealWith(CommonDefine.GoodType goodType)
    {

    }
}

public class MapData
{
    /// <summary>
    /// 这个地图的所有路信息
    /// </summary>
    public List<RoadData> Roads;
    /// <summary>
    /// 地图ID，可以从地图表里根据这个ID，取地图的大小，地图名字等信息
    /// </summary>
    public int MapId;

    public void GeneratorRoads()
    {

    }
}
