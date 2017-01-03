using UnityEngine;
using System.Collections;

public class BackgroundLogic : UILogic {

    public BackgroundLogic()
    {
    }

	// Use this for initialization
	public void Initialize(BackgroundView view)
    {
        ItemSource = DungeonGenerator.Instance;

        SetBinding<RoomInfo>(DungeonGenerator.Instance.ROOMIN, view.RefreshRoom);
        SetBinding<RoadInfo>(DungeonGenerator.Instance.ROOMOUT, view.RefreshRoad);
    }

    public void IntoRoom()
    {

    }

    public void OutRoom()
    {

    }

    public override void Release()
    {
        base.Release();
    }
}
