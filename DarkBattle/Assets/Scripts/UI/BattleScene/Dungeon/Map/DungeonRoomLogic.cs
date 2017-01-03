using UnityEngine;
using System.Collections;

public class DungeonRoomLogic : UILogic {
    RoomInfo m_roomInfo = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Initialize(int x, int y, DungeonRoomView view)
    {
        m_roomInfo = DungeonGenerator.Instance.GetRoomByXY(x, y);
        ItemSource = m_roomInfo;

        SetBinding<bool>(m_roomInfo.ISHERE, view.IsHere);
    }

    public void onRoomOut()
    {
        DungeonGenerator.Instance.OutRoom(m_roomInfo);
    }
}
