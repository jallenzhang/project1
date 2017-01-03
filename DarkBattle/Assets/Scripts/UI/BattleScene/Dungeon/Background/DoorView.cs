using UnityEngine;
using System.Collections;

public class DoorView : MonoBehaviour {
    private int m_x;
    private int m_y;
	// Use this for initialization
	void Start () {
        UIEventListener.Get(gameObject).onClick = onIntoRoom;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Init(int x, int y)
    {
        m_x = x;
        m_y = y;
    }

    void onIntoRoom(GameObject go)
    {
        CameraFade.StartAlphaFade(Color.black, false, 3, 0, () => { DungeonGenerator.Instance.IntoRoom(DungeonGenerator.Instance.GetRoomByXY(m_x, m_y)); });
    }
}
