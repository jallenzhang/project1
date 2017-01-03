using UnityEngine;
using System.Collections;

public class HeroInventory : MonoBehaviour {
    private static HeroInventory s_instance = null;
    public static HeroInventory Instance
    {
        get
        {
            return s_instance;
        }
    }
    public GameObject grid;
    void Awake()
    {
        s_instance = this;
    }
	// Use this for initialization
	void Start () {
        Init();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Init()
    {
        foreach(RoleInfo roleInfo in GameData.Instance.GetCurrentUserInfo().RoleInfos)
        {
            if (!RoleManager.Instance.RoleInBattleDic.ContainsKey(roleInfo.id))
                AddItem(roleInfo);
        }
    }

    public void AddItem(RoleInfo roleInfo)
    {
        GameObject go = NGUITools.AddChild(grid, (GameObject)ResMgr.Instance.LoadAssetFromResource("Prefabs/UI/Inventory/HeroAvatarItem"));
        go.GetComponent<UISprite>().spriteName = CommonDefine.RoleNameDic[roleInfo.type];
        go.GetComponent<HeroOnBattleInventoryView>().roleInfo = roleInfo;
        grid.GetComponent<UIGrid>().Reposition();
        if (RoleManager.Instance.RoleInBattleDic.ContainsKey(roleInfo.id))
        {
            go.GetComponent<HeroOnBattleInventoryView>().roleBase = RoleManager.Instance.RoleInBattleDic[roleInfo.id];
            go.GetComponent<UISprite>().color = Color.black;
        }
    }
}
