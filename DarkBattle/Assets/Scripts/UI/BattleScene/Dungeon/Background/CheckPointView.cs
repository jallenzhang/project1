using UnityEngine;
using System.Collections;

public class CheckPointView : MonoBehaviour {
    private DungeonPoint m_dungeonPoint;
	// Use this for initialization
	void Start () {
        UIEventListener.Get(gameObject).onClick = onPerform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void onPerform(GameObject go)
    {
        if (m_dungeonPoint != null)
            m_dungeonPoint.Perform();
    }

    public void UpdateDungeonPoint(DungeonPoint point)
    {
        m_dungeonPoint = point;
    }
}
