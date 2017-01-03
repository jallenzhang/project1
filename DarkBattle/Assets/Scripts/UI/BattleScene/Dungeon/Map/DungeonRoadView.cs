using UnityEngine;
using System.Collections;

public class DungeonRoadView : MonoBehaviour {
    public DungeonRoadCheckPointView point1;
    public DungeonRoadCheckPointView point2;
    public DungeonRoadCheckPointView point3;
    public DungeonRoadCheckPointView point4;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Init(RoadInfo road)
    {
        point1.Init(road.RoadCheckPoints[0]);
        point2.Init(road.RoadCheckPoints[1]);
        point3.Init(road.RoadCheckPoints[2]);
        point4.Init(road.RoadCheckPoints[3]);
    }
}
