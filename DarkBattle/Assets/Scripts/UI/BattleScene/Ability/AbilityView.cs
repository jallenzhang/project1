using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilityView : MonoBehaviour {
    public Transform pos1;
    public Transform pos2;
    public Transform pos3;
    public Transform pos4;

    public GameObject btnExchange;
    public GameObject btnCancel;

    private AbilityLogic m_logic = null;
	// Use this for initialization
	void Start () {
        if (m_logic == null)
            m_logic = new AbilityLogic();

        UIEventListener.Get(btnExchange).onClick = onExchange;
        UIEventListener.Get(btnCancel).onClick = onCancel;
        EventService.Instance.GetEvent<AddEnemyInBattleEvent>().Subscribe(EnemyAddOrRemoveTrigger);
        m_logic.Initialize(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void onExchange(GameObject go)
    {
        GameData.Instance.BattleSceneActionFlag.AddFlag((long)StateDef.BattleActionFlag.OnExchanging);
        foreach (KeyValuePair<int, RoleBase> kv in RoleManager.Instance.RoleInBattleDic)
        {
            kv.Value.OverlayItemModel.IsChangable = true;
        }
    }

    private void onCancel(GameObject go)
    {
        
    }

    void EnemyAddOrRemoveTrigger(RoleBase role, bool needShow)
    {
        UpdateAbilitiesView(RoleManager.Instance.SelectedHero);
    }

    public void UpdateAbilitiesView(RoleBase role)
    {
        int i = 0;
        foreach(AbilityBase ab in role.m_abilities)
        {
            if (ab.InUse)
            {
                if (i == 0)
                {
                    FillPos1(ab);
                    i++;
                }
                else if (i == 1)
                {
                    FillPos2(ab);
                    i++;
                }
                else if (i == 2)
                {
                    FillPos3(ab);
                    i++;
                }
                else if (i == 3)
                {
                    FillPos4(ab);
                    i++;
                }
            }
        }
    }

    private void FillPos(AbilityBase ab, Transform parent)
    {
        if (parent.childCount == 0)
        {
            GameObject go = (GameObject)GameObject.Instantiate(ResMgr.Instance.LoadAssetFromResource("Prefabs/UI/HeroAbility/abilityIcon"));
            go.GetComponent<UISprite>().spriteName = string.Format(CommonDefine.RoleAbilityDic[RoleManager.Instance.SelectedHero.m_roleInfo.type], ab.Index);
            go.transform.parent = parent;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
        }
        else
        {
            parent.GetChild(0).GetComponent<UISprite>().spriteName = string.Format(CommonDefine.RoleAbilityDic[RoleManager.Instance.SelectedHero.m_roleInfo.type], ab.Index);
        }

        //判断这个技能是否可用，如果不可用就灰态
        if (ab.IsValid())
        {
            parent.GetChild(0).GetComponent<UISprite>().color = Color.white;
            parent.GetChild(0).GetComponent<BoxCollider>().enabled = true;
        }
        else
        {
            parent.GetChild(0).GetComponent<UISprite>().color = Color.black;
            parent.GetChild(0).GetComponent<BoxCollider>().enabled = false;
        }

        UIEventListener.Get(parent.GetChild(0).gameObject).onClick = (go) => { m_logic.onSelectAbility(ab); };
    }

    private void FillPos1(AbilityBase ab)
    {
        FillPos(ab, pos1);
    }

    private void FillPos2(AbilityBase ab)
    {
        FillPos(ab, pos2);
    }

    private void FillPos3(AbilityBase ab)
    {
        FillPos(ab, pos3);
    }

    private void FillPos4(AbilityBase ab)
    {
        FillPos(ab, pos4);
    }

    void OnDestroy()
    {
        if (m_logic != null)
        {
            m_logic.Release();
            m_logic = null;
        }

        EventService.Instance.GetEvent<AddEnemyInBattleEvent>().Unsubscribe(EnemyAddOrRemoveTrigger);
    }
}
