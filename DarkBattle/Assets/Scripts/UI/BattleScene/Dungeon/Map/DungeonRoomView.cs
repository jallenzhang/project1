using UnityEngine;
using System.Collections;

public class DungeonRoomView : MonoBehaviour {
    public GameObject isHereObj;
    private DungeonRoomLogic m_roomLogic = null;
	// Use this for initialization
	void Start () {
        UIEventListener.Get(gameObject).onClick = onOutRoom;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Init(int x, int y)
    {
        m_roomLogic = new DungeonRoomLogic();
        m_roomLogic.Initialize(x, y, this);
    }

    public void IsHere(bool isHere)
    {
        isHereObj.SetActive(isHere);
    }

    void OnDestroy()
    {
        if (m_roomLogic != null)
            m_roomLogic.Release();
    }

    void onOutRoom(GameObject go)
    {
        m_roomLogic.onRoomOut();
    }
}
