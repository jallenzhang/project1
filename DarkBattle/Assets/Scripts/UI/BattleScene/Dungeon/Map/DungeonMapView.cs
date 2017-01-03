using UnityEngine;
using System.Collections;

public class DungeonMapView : MonoBehaviour {
    public Transform[] roomPoints;
    public Transform roadsArea;
	// Use this for initialization
	void Start () {
        DrawMap();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void DrawMap()
    {
        Clear();
        DrawRooms();
        DrawRoads();
    }

    private void DrawRooms()
    {
        foreach(RoomInfo room in DungeonGenerator.Instance.DungeonRooms)
        {
            GameObject roomObj = (GameObject)GameObject.Instantiate(ResMgr.Instance.LoadAssetFromResource("Prefabs/UI/DungeonMap/room"));
            roomObj.transform.parent = roomPoints[room.X + DungeonGenerator.Instance.XSize * room.Y];
            roomObj.transform.localPosition = Vector3.zero;
            roomObj.transform.localScale = Vector3.one;
            roomObj.GetComponent<UISprite>().spriteName = string.Format(CommonDefine.DungeonRoomIcon, room.Type.ToString());
            roomObj.GetComponent<DungeonRoomView>().Init(room.X, room.Y);
        }

        //use this to update UI, because call function changecurrentpoint will not change UI, because the current dungeon is the same;
        DungeonGenerator.Instance.DungeonRooms[0].IsHere = true;
    }

    private void DrawRoads()
    {
        foreach(RoadInfo road in DungeonGenerator.Instance.DungeonRoads)
        {
            GameObject obj = null;
            switch(road.BuildDir)
            {
                case CommonDefine.MoveDirection.East:
                case CommonDefine.MoveDirection.West:
                    obj = (GameObject)GameObject.Instantiate(ResMgr.Instance.LoadAssetFromResource("Prefabs/UI/DungeonMap/road_h"));
                    break;
                case CommonDefine.MoveDirection.North:
                case CommonDefine.MoveDirection.South:
                    obj = (GameObject)GameObject.Instantiate(ResMgr.Instance.LoadAssetFromResource("Prefabs/UI/DungeonMap/road_v"));
                    break;
            }
            obj.transform.parent = roomPoints[road.X + DungeonGenerator.Instance.XSize * road.Y];
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            obj.GetComponent<DungeonRoadView>().Init(road);
            switch (road.BuildDir)
            {
                case CommonDefine.MoveDirection.East:
                    obj.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    obj.transform.localPosition += new Vector3(70, 0, 0);
                    break;
                case CommonDefine.MoveDirection.North:
                    obj.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    obj.transform.localPosition += new Vector3(0, 70, 0);
                    break;
                case CommonDefine.MoveDirection.West:
                    obj.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    obj.transform.localPosition += new Vector3(-70, 0, 0);
                    break;
                case CommonDefine.MoveDirection.South:
                    obj.transform.localRotation = Quaternion.Euler(new Vector3(180, 0, 0));
                    obj.transform.localPosition += new Vector3(0, -70, 0);
                    break;
            }

            obj.transform.parent = roadsArea;

        }
    }

    private void Clear()
    {
        foreach(Transform trans in roomPoints)
        {
            if (trans.childCount > 0)
            {
                GameObject.Destroy(trans.GetChild(0).gameObject);
            }
        }

        for (int i = 0; i < roadsArea.childCount; i++)
        {
            GameObject.Destroy(roadsArea.GetChild(i).gameObject);
        }
    }
}
