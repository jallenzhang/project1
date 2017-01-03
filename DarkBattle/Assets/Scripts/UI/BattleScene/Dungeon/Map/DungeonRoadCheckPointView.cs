using UnityEngine;
using System.Collections;

public class DungeonRoadCheckPointView : MonoBehaviour {
    public GameObject isHereObj;
    public UISprite checkPointIcon;

    private DungeonRoadCheckPointLogic m_logic;
    private string defaultName = "hall_clear";
    private string testName = "marker_curio";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Init(DungeonPoint checkPoint)
    {
        m_logic = new DungeonRoadCheckPointLogic();
        m_logic.Initialize(checkPoint, this);
        InitView(checkPoint);
    }

    public void IsHere(bool isHere)
    {
        isHereObj.SetActive(isHere);
    }

    public void IsDone(bool isDone)
    {
        string name = isDone ? defaultName : testName;
    }

    void InitView(DungeonPoint checkPoint)
    {
        if (checkPoint.CheckPointType != CommonDefine.CheckPointType.None)
            checkPointIcon.spriteName = checkPoint.DrawIcon();
        else
            checkPointIcon.spriteName = defaultName;
    }

    void OnDestroy()
    {
        if (m_logic != null)
            m_logic.Release();
    }
}
