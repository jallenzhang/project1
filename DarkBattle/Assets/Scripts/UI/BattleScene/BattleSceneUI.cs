using UnityEngine;
using System.Collections;

public class BattleSceneUI : MonoBehaviour {
    public GameObject inventroyObj;
    public GameObject mapObj;

    private SimpleState m_inventroyState = new SimpleState();
    private SimpleState m_mapState = new SimpleState();

    private SimpleStateMachine m_stateMachine;
	// Use this for initialization
	void Start () {
        
	}

    void OnEnable()
    {
        Init();
    }
	
    void Init()
    {
        UIEventListener.Get(inventroyObj).onClick = onMap;
        UIEventListener.Get(mapObj).onClick = onInventory;

        m_stateMachine = new SimpleStateMachine();
        m_inventroyState.onEnter = () => { inventroyObj.SetActive(true); };
        m_inventroyState.onLeave = () => { inventroyObj.SetActive(false); };
        m_mapState.onEnter = () => { mapObj.SetActive(true); };
        m_mapState.onLeave = () => { mapObj.SetActive(false); };
        m_stateMachine.State = m_mapState;
    }

	// Update is called once per frame
	void Update () {
	
	}

    void onInventory(GameObject go)
    {
        m_stateMachine.State = m_inventroyState;
    }

    void onMap(GameObject go)
    {
        m_stateMachine.State = m_mapState;
    }
}
