using UnityEngine;
using System.Collections;

public class DungeonRoadCheckPointLogic : UILogic {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Initialize(DungeonPoint checkPoint, DungeonRoadCheckPointView view)
    {
        ItemSource = checkPoint;

        SetBinding<bool>(checkPoint.ISHERE, view.IsHere);
        SetBinding<bool>(checkPoint.ISDONE, view.IsDone);
    }
}
