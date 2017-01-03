using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroOnBattleInventory : MonoBehaviour {

    public GameObject[] cells;

    private bool m_isFind = false;

    void Start()
    {
        Init();
    }

    void Init()
    {
        foreach(KeyValuePair<int, RoleBase> kv in RoleManager.Instance.RoleInBattleDic)
        {
            if (kv.Value.IsHero())
            {
                AddItem(kv.Value, kv.Value.m_playerPosition);
            }
        }
    }

    public void AddItem(RoleBase roleBase, int pos)
    {
        if (pos > cells.Length - 1)
            return;

        if (cells[pos].transform.childCount == 0)
        {
            GameObject go = NGUITools.AddChild(cells[pos], (GameObject)ResMgr.Instance.LoadAssetFromResource("Prefabs/UI/Inventory/SimpleHeroAvatarItem"));
            go.GetComponent<UISprite>().spriteName = CommonDefine.RoleNameDic[roleBase.m_roleType];
            go.GetComponent<HeroOnBattleInventoryView>().roleBase = roleBase;
            go.GetComponent<HeroOnBattleInventoryView>().roleInfo = roleBase.m_roleInfo;
        }
    }
}
