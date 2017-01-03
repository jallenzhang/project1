using UnityEngine;
using System.Collections;

public class BackgroundView : MonoBehaviour {
    public GameObject bgArea;
    public GameObject roomArea;
    public GameObject midArea;
    public GameObject WallArea;
    public GameObject DoorArea;
    public GameObject CheckPointArea;

    private float m_maxDistance = 6600;
    private BackgroundLogic m_logic = null;
    private Vector3 originalPos = new Vector3(0, 104, 0);

    private RoadInfo m_currentRoad;
	// Use this for initialization
	void Start () {
        if (m_logic == null)
            m_logic = new BackgroundLogic();

        m_logic.Initialize(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy()
    {
        if (m_logic != null)
        {
            m_logic.Release();
            m_logic = null;
        }
    }

    public void RefreshRoom(RoomInfo info)
    {  
        //UpdateBackground();
        UpdateRoom();
        //UpdateMiddle();
        //UpdateWall();
        //UpdateDoor();
        //UpdateCheckPoints();
        //ResetPositions();
    }

    public void RefreshRoad(RoadInfo info)
    {
        m_currentRoad = info;
        UpdateBackground();
        UpdateRoom();
        UpdateMiddle();
        UpdateWall(info);
        UpdateDoor(info);
        UpdateCheckPoints(info);
        ResetPositions();
    }

    private string GetDungeonPath()
    {
        string ret = string.Empty;

        switch(DungeonGenerator.Instance.CurrentDungeon)
        {
            case StateDef.DungeonType.crypts:
            case StateDef.DungeonType.cove:
                ret = "Textures/Dungeons/" + DungeonGenerator.Instance.CurrentDungeon.ToString() + "/";
                break;
            default:
                Debug.logger.LogError("Dungeon", "找不到对应的路径 " + DungeonGenerator.Instance.CurrentDungeon.ToString());
                break;
        }

        return ret;
    }

    private string GetCheckPointPath()
    {
        string ret = string.Empty;

        ret = "Textures/Dungeons/checkpoints/";

        return ret;
    }

    void UpdateRoom()
    {
        if (DungeonGenerator.Instance.IsInRoom)
        {
            if (roomArea != null && roomArea.transform.childCount > 0)
            {
                switch (DungeonGenerator.Instance.CurrentDungeon)
                {
                    case StateDef.DungeonType.cove:
                    case StateDef.DungeonType.crypts:
                        roomArea.transform.GetChild(0).GetComponent<UITexture>().mainTexture = (Texture)ResMgr.Instance.LoadAssetFromResource(GetDungeonPath() + string.Format(CommonDefine.DungeonRoomBG, DungeonGenerator.Instance.CurrentDungeon.ToString(), Random.Range(1, 8).ToString("d2")));
                        break;
                    default:
                        Debug.logger.LogError("Dungeon", "没有这个类型的房间");
                        break;
                }
            }
            roomArea.SetActive(true);
        }
        else
        {
            roomArea.SetActive(false);
        }
        
    }

    void UpdateBackground()
    {
        if (bgArea != null && bgArea.transform.childCount > 0)
        {
            switch (DungeonGenerator.Instance.CurrentDungeon)
            {
                case StateDef.DungeonType.cove:
                case StateDef.DungeonType.crypts:
                    bgArea.transform.GetChild(0).GetComponent<UITexture>().mainTexture = (Texture)ResMgr.Instance.LoadAssetFromResource(GetDungeonPath() + string.Format(CommonDefine.DungeonBackground, DungeonGenerator.Instance.CurrentDungeon.ToString()));
                    break;
                default:
                    Debug.logger.LogError("Dungeon", "没有这个类型的关卡");
                    break;
            }
        }
        
    }

    void UpdateMiddle()
    {
        if (midArea != null && midArea.transform.childCount > 0)
        {
            switch (DungeonGenerator.Instance.CurrentDungeon)
            {
                case StateDef.DungeonType.cove:
                case StateDef.DungeonType.crypts:
                    midArea.transform.GetChild(0).GetComponent<UITexture>().mainTexture = (Texture)ResMgr.Instance.LoadAssetFromResource(GetDungeonPath() + string.Format(CommonDefine.DungeonMiddle, DungeonGenerator.Instance.CurrentDungeon.ToString()));
                    break;
                default:
                    Debug.logger.LogError("Dungeon", "没有这个类型的中间层");
                    break;
            }
        }
        
    }

    void UpdateWall(RoadInfo info)
    {
        int childCount = WallArea.transform.childCount;
        for (int i = 0; i < childCount; i++ )
        {
            switch (DungeonGenerator.Instance.CurrentDungeon)
            {
                case StateDef.DungeonType.cove:
                case StateDef.DungeonType.crypts:
                    WallArea.transform.GetChild(i).GetComponent<UITexture>().mainTexture = (Texture)ResMgr.Instance.LoadAssetFromResource(GetDungeonPath() + string.Format(CommonDefine.DungeonWall, DungeonGenerator.Instance.CurrentDungeon.ToString(), 
                        DungeonGenerator.Instance.CurrentDir == info.BuildDir ? info.WallIDs[i].ToString("d2") : info.WallIDs[info.WallIDs.Count - 1 - i].ToString("d2")));
                    break;
                default:
                    Debug.logger.LogError("Dungeon", "没有这个类型的Wall " + i.ToString());
                    break;
            }
        }
    }

    void UpdateDoor(RoadInfo info)
    {
        try
        {
            int childCount = DoorArea.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                switch (DungeonGenerator.Instance.CurrentDungeon)
                {
                    case StateDef.DungeonType.cove:
                    case StateDef.DungeonType.crypts:
                        DoorArea.transform.GetChild(i).GetComponent<UITexture>().mainTexture = (Texture)ResMgr.Instance.LoadAssetFromResource(GetDungeonPath() + string.Format(CommonDefine.DungeonDoor, DungeonGenerator.Instance.CurrentDungeon.ToString()));
                        break;
                    default:
                        Debug.logger.LogError("Dungeon", "没有这个类型的门");
                        break;
                }
                RoomInfo neibour = info.GetNeibourRoom(DungeonGenerator.Instance.CurrentDir);

                if (neibour == null)
                {
                    Debug.logger.LogError("Neibour", "road neibour room " + info.NeighbourRooms.Count);
                }
                
                if (info.BuildDir == DungeonGenerator.Instance.CurrentDir)
                {
                    //如果是跟所造路同方向，那么进门处的坐标就是当前路的起始坐标
                    if (i == 0)
                    {
                        DoorArea.transform.GetChild(i).GetComponent<DoorView>().Init(info.X, info.Y);
                    }
                    else if (i == 1)
                    {
                        DoorArea.transform.GetChild(i).GetComponent<DoorView>().Init(neibour.X, neibour.Y);
                    }
                }
                else
                {
                    //如果是跟所造路反方向，那么进门处的坐标就是当前路的出口坐标
                    if (i == 0)
                    {
                        DoorArea.transform.GetChild(i).GetComponent<DoorView>().Init(neibour.X, neibour.Y);
                    }
                    else if (i == 1)
                    {
                        DoorArea.transform.GetChild(i).GetComponent<DoorView>().Init(info.X, info.Y);
                    }
                }
            }
        }
        catch(System.Exception ex)
        {
            Debug.logger.Log(ex.Message);
        }
    }

    void UpdateCheckPoints(RoadInfo info)
    {
        int childCount = CheckPointArea.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            DungeonPoint point = DungeonGenerator.Instance.CurrentDir == info.BuildDir ? info.RoadCheckPoints[i]
                : info.RoadCheckPoints[info.RoadCheckPoints.Count - 1 - i];
            if (point.CheckPointType != CommonDefine.CheckPointType.None)
            {
                CheckPointArea.transform.GetChild(i).GetComponent<UITexture>().mainTexture = (Texture)ResMgr.Instance.LoadAssetFromResource(point.DrawTexture());
                CheckPointArea.transform.GetChild(i).GetComponent<CheckPointView>().UpdateDungeonPoint(point);
            }
            else
            {
                CheckPointArea.transform.GetChild(i).GetComponent<UITexture>().mainTexture = null;
                CheckPointArea.transform.GetChild(i).GetComponent<CheckPointView>().UpdateDungeonPoint(null);
            }
        }
    }

    private void UpdateCurrentCheckPoint()
    {
        if (DungeonGenerator.Instance.IsInRoom)
            return;

        int index = (int)(Mathf.Abs(CheckPointArea.transform.localPosition.x) / 1000f);
        if (index > 0 && m_currentRoad != null)
        {
            index -= 1;
            DungeonGenerator.Instance.ChangeCurrentPoint(DungeonGenerator.Instance.CurrentDir == m_currentRoad.BuildDir ?
                m_currentRoad.RoadCheckPoints[index]
                : m_currentRoad.RoadCheckPoints[m_currentRoad.RoadCheckPoints.Count - 1 - index]);
        }
    }

    public void Move(float speed)
    {
        if (WallArea.transform.localPosition.x > 0 && speed > 0)
            return;

        if (Mathf.Abs(WallArea.transform.localPosition.x) > m_maxDistance && speed < 0)
            return;

        bgArea.transform.Translate(speed * 0.2f * Time.deltaTime, 0, 0);
        midArea.transform.Translate(speed * 0.5f * Time.deltaTime, 0, 0);
        WallArea.transform.Translate(speed * Time.deltaTime, 0, 0);
        CheckPointArea.transform.Translate(speed * Time.deltaTime, 0, 0);
        this.DoorArea.transform.Translate(speed * Time.deltaTime, 0, 0);
        UpdateCurrentCheckPoint();
    }


    
    private void ResetPositions()
    {
        bgArea.transform.localPosition = originalPos;
        midArea.transform.localPosition = originalPos;
        WallArea.transform.localPosition = originalPos;
        DoorArea.transform.localPosition = originalPos;
        CheckPointArea.transform.localPosition = originalPos;
    }
}
