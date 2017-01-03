using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour {
    public GameObject btnIntoRoom;
    public GameObject btnAddSoldier;
    public GameObject btnAddNun;
    public GameObject btnAddYeCha;
    public GameObject btnShowGuiMian;
    public GameObject btnSunYuan;

    private static test s_instance = null;
    public static test Instance
    {
        get
        {
            return s_instance;
        }
    }

    void Awake()
    {
        s_instance = this;
        InitComp();
    }

	// Use this for initialization
	void Start () {
    }

    void InitComp()
    {
        UIEventListener.Get(btnIntoRoom).onClick = OnIntoRoom;
        UIEventListener.Get(btnAddNun).onClick = OnAddNun;
        UIEventListener.Get(btnAddSoldier).onClick = OnAddSoldier;
        UIEventListener.Get(btnAddYeCha).onClick = OnAddYeCha;
        UIEventListener.Get(btnShowGuiMian).onClick = OnShowGuiMian;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnIntoRoom(GameObject go)
    {
        SceneManager.LoadScene("Entrance");
    }

    void OnAddNun(GameObject go)
    {
        RoleInfo info = GameData.Instance.AddRoleIntoDB(CommonDefine.RoleType.Nun);
        HeroInventory.Instance.AddItem(info);
    }

    void OnAddSoldier(GameObject go)
    {
        RoleInfo info = GameData.Instance.AddRoleIntoDB(CommonDefine.RoleType.Soldier);
        HeroInventory.Instance.AddItem(info);
    }

    void OnAddYeCha(GameObject go)
    {
        RoleInfo info = GameData.Instance.AddRoleIntoDB(CommonDefine.RoleType.YeCha);
        HeroInventory.Instance.AddItem(info);
    }

    void OnAddSunYuan(GameObject go)
    {
        RoleInfo info = GameData.Instance.AddRoleIntoDB(CommonDefine.RoleType.SunYuan);
        HeroInventory.Instance.AddItem(info);
    }

    void OnShowGuiMian(GameObject go)
    {
        Enemy enemy = EnemyGenerator.Instance.Generator(CommonDefine.RoleType.GuiMianJunZi, UnityEngine.Random.Range(4, 8));
    }
}
