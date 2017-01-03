using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonGenerator : NotifyPropChanged, INotifyPropChanged {
    public string ROOMIN = "dungeon_roomIn";
    public string ROOMOUT = "dungeon_roomOut";

    static DungeonGenerator instance = null;
    public static DungeonGenerator Instance
    {
        get
        {
            if (instance == null)
                instance = new DungeonGenerator();

            return instance;
        }
    }

    List<RoomInfo> m_rooms = new List<RoomInfo>();
    public List<RoomInfo> DungeonRooms
    {
        get
        {
            return m_rooms;
        }
    }

    List<RoadInfo> m_roads = new List<RoadInfo>();
    public List<RoadInfo> DungeonRoads
    {
        get
        {
            return m_roads;
        }
    }
    int m_xSize;
    public int XSize
    {
        get
        {
            return m_xSize;
        }
    }
    int m_ySize;
    public int YSize
    {
        get
        {
            return m_ySize;
        }
    }

    int m_roomSize;

    CommonDefine.DungeonCellType[] m_dungeonCells = null;

    private StateDef.DungeonType m_currentDungeonType = StateDef.DungeonType.cove;
    public StateDef.DungeonType CurrentDungeon
    {
        get
        {
            return m_currentDungeonType;
        }
    }

    public bool IsInRoom = true;

    public int CurrentX
    {
        get;
        set;
    }

    public int CurrentY
    {
        get;
        set;
    }

    public DungeonPoint CurrentPoint = null;

    public CommonDefine.MoveDirection m_currentDir = CommonDefine.MoveDirection.None;
    public CommonDefine.MoveDirection CurrentDir
    {
        get
        {
            return m_currentDir;
        }
        set
        {
            m_currentDir = value;
        }
    }

    public void Reset()
    {
        if (m_dungeonCells != null)
        {
            m_dungeonCells = null;
            m_roads.Clear();
            m_rooms.Clear();
            m_xSize = 0;
            m_ySize = 0;
            m_roomSize = 0;
        }
    }

    /// <summary>
    /// 创建一个随机的地牢地图
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="rooms">多少间房</param>
    /// <returns></returns>
	public bool CreateDungeon(int width, int height, int rooms, StateDef.DungeonType type)
    {
        bool ret = false;
        
        Reset();

        m_currentDungeonType = type;
        m_dungeonCells = new CommonDefine.DungeonCellType[width * height];
        m_roomSize = rooms;
        m_xSize = width;
        m_ySize = height;
        Initialize();

        int startX = Random.Range(0, 100) % m_xSize;
        int startY = Random.Range(0, 100) % m_ySize;
        CurrentX = startX;
        CurrentY = startY;
        
        MakeRoom(startX, startY);
        m_rooms[0].Type = CommonDefine.RoomType.entrance;
        ChangeCurrentPoint(m_rooms[0]);
        ret = CreateMap(startX, startY);

        return ret;
    }

    public void ChangeCurrentPoint(DungeonPoint point)
    {
        if (CurrentPoint == point)
            return;

        if (CurrentPoint != null)
        {
            CurrentPoint.IsHere = false;
        }

        CurrentPoint = point;
        CurrentPoint.IsHere = true;
    }
    /// <summary>
    /// 判断地图是否创建完成
    /// </summary>
    /// <returns></returns>
    bool CheckCreateMapFinished()
    {
        if (m_rooms.Count >= m_roomSize)
            return true;

        return false;
    }

    /// <summary>
    /// 递归创建随机地图
    /// </summary>
    /// <param name="previousX"></param>
    /// <param name="previousY"></param>
    /// <returns></returns>
    private bool CreateMap(int previousX, int previousY)
    {
        for (int i = 0; i < 30; i++)
        {
            if (CheckCreateMapFinished())
                return true;
            CommonDefine.MoveDirection dir = (CommonDefine.MoveDirection)(Random.Range(0, 100) % 4);
            int tmpX = previousX;
            int tmpY = previousY;
            switch (dir)
            {
                case CommonDefine.MoveDirection.East:
                    tmpX += 1;
                    break;
                case CommonDefine.MoveDirection.South:
                    tmpY -= 1;
                    break;
                case CommonDefine.MoveDirection.West:
                    tmpX -= 1;
                    break;
                case CommonDefine.MoveDirection.North:
                    tmpY += 1;
                    break;
            }

            if (TryMakeRoom(tmpX, tmpY))
            {
                RoomInfo prevRoom = GetRoomByXY(previousX, previousY);
                RoadInfo road = MakeRoad(previousX, previousY, dir);
                prevRoom.AddNeibourRoads(road);
                road.AddNeibourRoom(prevRoom);
                RoomInfo curRoom = MakeRoom(tmpX, tmpY);
                curRoom.AddNeibourRoads(road);
                road.AddNeibourRoom(curRoom);
                CreateMap(tmpX, tmpY);
            }
        }

        return false;
    }

    /// <summary>
    /// 根据X,Y取得房间
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public RoomInfo GetRoomByXY(int x, int y)
    {
        foreach(RoomInfo room in m_rooms)
        {
            if (room.X == x && room.Y == y)
                return room;
        }
        return null;
    }

    /// <summary>
    /// 根据X,Y,DIR取得路
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="dir"></param>
    /// <returns></returns>
    public RoadInfo GetRoadByXYAndDir(int x, int y, CommonDefine.MoveDirection dir)
    {
        foreach(RoadInfo r in m_roads)
        {
            if (x == r.X && y == r.Y && dir == r.BuildDir)
            {
                return r;
            }

            if (Mathf.Abs(r.BuildDir - dir) == 2)
            {
                int tmpX = -1;
                int tmpY = -1;
                switch (r.BuildDir)
                {
                    case CommonDefine.MoveDirection.East:
                        tmpX = r.X + 1;
                        tmpY = r.Y;
                        break;
                    case CommonDefine.MoveDirection.North:
                        tmpX = r.X;
                        tmpY = r.Y + 1;
                        break;
                    case CommonDefine.MoveDirection.South:
                        tmpX = r.X;
                        tmpY = r.Y - 1;
                        break;
                    case CommonDefine.MoveDirection.West:
                        tmpX = r.X - 1;
                        tmpY = r.Y;
                        break;
                }

                if (tmpX == x && tmpY == y)
                    return r;
            }
        }
        return null;
    }

    /// <summary>
    /// 在指定的x,y下创建房间
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>成功返回true继续创建房间，失败或者创建完毕返回false</returns>
    private RoomInfo MakeRoom(int x, int y)
    {
        if (x >= 0 && x < m_xSize && y >= 0 && y < m_ySize && m_rooms.Count <= m_roomSize)
        {
            if (m_dungeonCells[x + y * m_xSize] == CommonDefine.DungeonCellType.Unused)
            {
                m_dungeonCells[x + y * m_xSize] = CommonDefine.DungeonCellType.Room;
                RoomInfo room = new RoomInfo();
                room.X = x;
                room.Y = y;
                room.Type = RandomRoomType();
                m_rooms.Add(room);
                return room;
            }
        }

        return null;
    }
    /// <summary>
    /// 检测这个位置是否可以创建房间
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool TryMakeRoom(int x, int y)
    {
        bool ret = false;
        if (x >= 0 && x < m_xSize && y >= 0 && y < m_ySize && m_rooms.Count < m_roomSize)
        {
            ret = m_dungeonCells[x + y * m_xSize] == CommonDefine.DungeonCellType.Unused;
        }
        return ret;
    }

    /// <summary>
    /// 在指定的x,y和dir下创建一条路
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="dir"></param>
    /// <returns>成功返回true，失败返回false</returns>
    private RoadInfo MakeRoad(int x, int y, CommonDefine.MoveDirection dir)
    {
        RoadInfo road = new RoadInfo();
        road.X = x;
        road.Y = y;
        road.BuildDir = dir;

        m_roads.Add(road);
        return road;
    }
    /// <summary>
    /// 初始化，让所有的cell都设置为unused
    /// </summary>
    private void Initialize()
    {
        for (int x = 0; x < m_xSize; x++)
        {
            for(int y = 0; y < m_ySize; y++)
            {
                SetCell(x, y, CommonDefine.DungeonCellType.Unused);
            }
        }
    }

    /// <summary>
    /// 设置指定cell的类型
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="cellType"></param>
    private void SetCell(int x, int y, CommonDefine.DungeonCellType cellType)
    {
        m_dungeonCells[x + m_xSize * y] = cellType;
    }
    /// <summary>
    /// 产生随机的房间类型
    /// </summary>
    /// <returns></returns>
    private CommonDefine.RoomType RandomRoomType()
    {
        List<RandomObject> objs = new List<RandomObject>();
        foreach (KeyValuePair<int, RoomData> kv in RoomData.dataMap)
        {
            RandomObject ro = new RandomObject();
            ro.ItemId = kv.Key;
            ro.Weight = kv.Value.weight;
            objs.Add(ro);
        }

        List<RandomObject> retObjs = ProjectHelper.GetRandomList<RandomObject>(objs, 1);
        if (retObjs == null)
            Debug.logger.LogError("DungeonPoint", "random objet occurs error");

        CommonDefine.RoomType rt = (CommonDefine.RoomType)retObjs[0].ItemId;

        return rt;
    }

    #region Dungeon Operations
    public void IntoRoom(RoomInfo roomInfo)
    {
        IsInRoom = true;
        CurrentX = roomInfo.X;
        CurrentY = roomInfo.Y;
        CurrentDir = CommonDefine.MoveDirection.None;
        ChangeCurrentPoint(roomInfo);
        //roomInfo.IsHere = true;

        OnPropertyChanged<RoomInfo>(ROOMIN, roomInfo);
    }

    /// <summary>
    /// 离开此房间，去往下一个房间
    /// </summary>
    /// <param name="targetRoom"></param>
    public void OutRoom(RoomInfo targetRoom)
    {
        //在路上时不能操纵去哪个房间
        if (!IsInRoom)
            return;

        //只能去隔壁房间，不能一下子去很远的房子
        RoomInfo curRoom = GetRoomByXY(CurrentX, CurrentY);
        int distance = Mathf.Abs(targetRoom.X - curRoom.X) + Mathf.Abs(targetRoom.Y - curRoom.Y);
        if (distance != 1)
            return;

        //判断去的房间的方向，从而取得去的路
        IsInRoom = false;
        if (targetRoom.X > CurrentX)
            CurrentDir = CommonDefine.MoveDirection.East;
        else if (targetRoom.Y > CurrentY)
            CurrentDir = CommonDefine.MoveDirection.North;
        else if (targetRoom.X < CurrentX)
            CurrentDir = CommonDefine.MoveDirection.West;
        else if (targetRoom.Y < CurrentY)
            CurrentDir = CommonDefine.MoveDirection.South;
        else
            CurrentDir = CommonDefine.MoveDirection.None;

        RoadInfo currentRoad = GetRoadByXYAndDir(CurrentX, CurrentY, CurrentDir);

        //通知更新UI
        OnPropertyChanged<RoadInfo>(ROOMOUT, currentRoad);
    }
    #endregion
}

public class DungeonPoint : NotifyPropChanged, INotifyPropChanged
{
    public string ISHERE = "dungeon_isHere";
    public string ISDONE = "dungeon_isDone";
    public string CHECKPOINTTYPE = "dungeon_checkPointType";

    private bool m_isHere = false;
    public bool IsHere
    {
        get
        {
            return m_isHere;
        }
        set
        {
            m_isHere = value;
            OnPropertyChanged(ISHERE, m_isHere);
        }
    }

    private bool m_isDone = false;
    public bool IsDone
    {
        get
        {
            return m_isDone;
        }
        set
        {
            m_isDone = value;
            OnPropertyChanged(ISDONE, m_isDone);
        }
    }

    private CommonDefine.CheckPointType m_checkPointType = CommonDefine.CheckPointType.None;
    public CommonDefine.CheckPointType CheckPointType
    {
        get
        {
            return m_checkPointType;
        }
        set
        {
            m_checkPointType = value;
            OnPropertyChanged(CHECKPOINTTYPE, m_checkPointType);
        }
    }

    /// <summary>
    /// 绘制小图标在map上
    /// </summary>
    /// <returns></returns>
    public virtual string DrawIcon()
    {
        return string.Empty;
    }

    /// <summary>
    /// 绘制图片在场景里
    /// </summary>
    /// <returns></returns>
    public virtual string DrawTexture()
    {
        if (IsDone)
            return GetCheckPointPath() + string.Format(CommonDefine.DungeoncheckPointDoneIcon, CheckPointType.ToString());
        else
            return GetCheckPointPath() + string.Format(CommonDefine.DungeonCheckPointIcon, CheckPointType.ToString());
    }

    /// <summary>
    /// 触发check point上的物件
    /// </summary>
    public virtual void Perform()
    {
        Debug.logger.Log(GetType().Name);
    }

    protected string GetCheckPointPath()
    {
        return "Textures/Dungeons/checkpoints/";   
    }
}

public class RoomInfo :DungeonPoint
{
    public int X;
    public int Y;
    public CommonDefine.RoomType Type;
    public int RoomID;
    private List<RoadInfo> m_neibourRoads = new List<RoadInfo>();
    public List<RoadInfo> NeighbourRoads
    {
        get
        {
            return m_neibourRoads;
        }
    }

    public RoomInfo()
    {
        RoomID = Random.Range(1, 9);
    }

    public void AddNeibourRoads(RoadInfo road)
    {
        if (m_neibourRoads.Count == 0)
        {
            m_neibourRoads.Add(road);
            return;
        }

        foreach(RoadInfo r in m_neibourRoads)
        {
            //方向相同，坐标一样，表示已经存在
            if (r.X == road.X && r.Y == road.Y && r.BuildDir == road.BuildDir)
                return;

            //方向相反，坐标经过转换如果也一样，表示已存在
            if (Mathf.Abs(r.BuildDir - road.BuildDir) == 2)
            {
                int tmpX = -1;
                int tmpY = -1;
                switch(r.BuildDir)
                {
                    case CommonDefine.MoveDirection.East:
                        tmpX = r.X + 1;
                        tmpY = r.Y;
                        break;
                    case CommonDefine.MoveDirection.North:
                        tmpX = r.X;
                        tmpY = r.Y + 1;
                        break;
                    case CommonDefine.MoveDirection.South:
                        tmpX = r.X;
                        tmpY = r.Y - 1;
                        break;
                    case CommonDefine.MoveDirection.West:
                        tmpX = r.X - 1;
                        tmpY = r.Y;
                        break;
                }

                if (tmpX == road.X && tmpY == road.Y)
                    return;
            }
        }

        m_neibourRoads.Add(road);
    }
}

public class RoadCheckPoint : DungeonPoint
{
    public RoadCheckPoint()
    {
        CheckPointType = (CommonDefine.CheckPointType)Random.Range(0, 4);
    }
}

public class RoadInfo
{
    public int X;
    public int Y;
    /// <summary>
    /// 创建路时的方向
    /// </summary>
    public CommonDefine.MoveDirection BuildDir;
    public List<int> WallIDs = new List<int>();
    private List<DungeonPoint> m_roadCheckPoints = new List<DungeonPoint>();
    public List<DungeonPoint> RoadCheckPoints
    {
        get
        {
            return m_roadCheckPoints;
        }
    }
    private List<RoomInfo> m_neighbourRooms = new List<RoomInfo>();
    public List<RoomInfo> NeighbourRooms
    {
        get
        {
            return m_neighbourRooms;
        }
    }

    public RoadInfo()
    {
        WallIDs = new List<int>();
        WallIDs.Add(Random.Range(0, 8));
        WallIDs.Add(Random.Range(0, 8));
        WallIDs.Add(Random.Range(0, 8));
        WallIDs.Add(Random.Range(0, 8));
        WallIDs.Add(Random.Range(0, 8));
        WallIDs.Add(Random.Range(0, 8));
        WallIDs.Add(Random.Range(0, 8));
        WallIDs.Add(Random.Range(0, 8));
        WallIDs.Add(Random.Range(0, 8));

        m_roadCheckPoints.Add(CheckPointGenerator.CreateCheckPoint());
        m_roadCheckPoints.Add(CheckPointGenerator.CreateCheckPoint());
        m_roadCheckPoints.Add(CheckPointGenerator.CreateCheckPoint());
        m_roadCheckPoints.Add(CheckPointGenerator.CreateCheckPoint());
    }

    public void AddNeibourRoom(RoomInfo room)
    {
        if (m_neighbourRooms.Count == 0)
        {
            m_neighbourRooms.Add(room);
            return;
        }

        foreach(RoomInfo r in m_neighbourRooms)
        {
            if (r.X == room.X && r.Y == room.Y)
                return;
        }

        m_neighbourRooms.Add(room);
    }

    public RoomInfo GetNeibourRoom(CommonDefine.MoveDirection dir)
    {
        int tmpX = -1;
        int tmpY = -1;

        switch(dir)
        {
            case CommonDefine.MoveDirection.East:
                tmpX = DungeonGenerator.Instance.CurrentX + 1;
                tmpY = DungeonGenerator.Instance.CurrentY;
                break;
            case CommonDefine.MoveDirection.North:
                tmpX = DungeonGenerator.Instance.CurrentX;
                tmpY = DungeonGenerator.Instance.CurrentY + 1;
                break;
            case CommonDefine.MoveDirection.South:
                tmpX = DungeonGenerator.Instance.CurrentX;
                tmpY = DungeonGenerator.Instance.CurrentY - 1;
                break;
            case CommonDefine.MoveDirection.West:
                tmpX = DungeonGenerator.Instance.CurrentX - 1;
                tmpY = DungeonGenerator.Instance.CurrentY;
                break;
        }

        foreach(RoomInfo room in m_neighbourRooms)
        {
            if (tmpX == room.X && tmpY == room.Y)
                return room;
        }

        return null;
    }
}
