using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

public class RoleManager : NotifyPropChanged, INotifyPropChanged  {
    public const string CURRENTHERO = "RoleManager_CurrentHero";
    public const string ROLEADDORREMOVED = "RoleManager_AddOrRemoved";

    private static RoleManager s_instance = null;

    public static RoleManager Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = new RoleManager();

            return s_instance;
        }
    }

    private List<RoleBase> m_roleList = new List<RoleBase>();
    public List<RoleBase> RoleList
    {
        get
        {
            return m_roleList;
        }
    }
    
    private Dictionary<int, RoleBase> m_roleInBattleDic = new Dictionary<int, RoleBase>();
    public Dictionary<int, RoleBase> RoleInBattleDic
    {
        get
        {
            return m_roleInBattleDic;
        }
    }
    private RoleBase m_selectedHero = null;
    private GameObject m_hideRoleArea;

    public RoleManager()
    {
    }
    /// <summary>
    /// 从DB那初始化角色U3D对象，并将其加入RoleManager下
    /// </summary>
    /// <param name="hideRoleArea"></param>
    public void InitHerosData(GameObject hideRoleArea)
    {
        try
        {
            m_hideRoleArea = hideRoleArea;
            for (int i = 0; i < GameData.Instance.GetCurrentUserInfo().RolesOnBattle.Count; i++)
            {
                CreateHero(GameData.Instance.GetCurrentUserInfo().GetRoleInfo(int.Parse(GameData.Instance.GetCurrentUserInfo().RolesOnBattle[i])), i);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
        
    }

    /// <summary>
    /// 创建一个角色的U3D对象，将其加入到RoleManager下
    /// </summary>
    /// <param name="roleInfo"></param>
    /// <returns></returns>
    public Hero CreateHero(RoleInfo roleInfo, int pos = -1)
    {
        if (roleInfo == null)
            return null;
        GameObject heroObj = CreateRoleObject(roleInfo);
        if (pos != -1)
            AddRoleInBattle(roleInfo, pos);

        Hero hero = heroObj.GetComponent<Hero>();
        switch (roleInfo.style)
        {
            case CommonDefine.RoleStyle.Normal:
                hero.skin.sharedMaterial.SetColor("_Color", Color.white);
                break;
            case CommonDefine.RoleStyle.Red:
                hero.skin.sharedMaterial.SetColor("_Color", Color.red);
                break;
            case CommonDefine.RoleStyle.Blue:
                hero.skin.sharedMaterial.SetColor("_Color", Color.blue);
                break;
        }

        if (heroObj != null)
        {
            heroObj.GetComponent<Hero>().Init(roleInfo.id);
            //GameObject heroObj = GameObject.Instantiate(hero.gameObject);
            heroObj.transform.parent = m_hideRoleArea.transform;
            heroObj.transform.localScale = Vector3.one;
            heroObj.GetComponent<Hero>().m_role.RoleObject = heroObj.gameObject;
        }

        return heroObj.GetComponent<Hero>();
    }

    private GameObject CreateRoleObject(RoleInfo roleInfo)
    {
        GameObject heroObj = RoleFactory.CreateRoleObject(roleInfo.type, roleInfo.style);
        return heroObj;
    }

    /// <summary>
    /// 创建一个怪物角色的U3D对象，将其加入到RoleManager下
    /// </summary>
    /// <param name="roleInfo"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Enemy CreateEnemy(RoleInfo roleInfo, int pos)
    {
        GameObject enemyObj = CreateRoleObject(roleInfo);
        RoleBase roleBase = RoleFactory.CreateRole(roleInfo);

        Enemy enemy = enemyObj.GetComponent<Enemy>();
        switch (roleInfo.style)
        {
            case CommonDefine.RoleStyle.Normal:
                enemy.skin.sharedMaterial.SetColor("_Color", Color.white);
                break;
            case CommonDefine.RoleStyle.Red:
                enemy.skin.sharedMaterial.SetColor("_Color", Color.red);
                break;
            case CommonDefine.RoleStyle.Blue:
                enemy.skin.sharedMaterial.SetColor("_Color", Color.blue);
                break;
        }

        if (enemyObj != null)
        {
            enemyObj.GetComponent<Enemy>().Init(roleBase);
            enemyObj.transform.parent = m_hideRoleArea.transform;
            enemyObj.transform.localScale = Vector3.one;
        }

        AddEnemyInBattle(roleBase, pos);
        return enemyObj.GetComponent<Enemy>();
    }

    /// <summary>
    /// 改变当前选中的英雄
    /// </summary>
    public RoleBase SelectedHero
    {
        get
        {
            return m_selectedHero;
        }
        set
        {
            if (value == null)
                Debug.logger.LogError("fucking ", "WTF");

            if (m_selectedHero == value)
                return;

            if (m_selectedHero != null)
                m_selectedHero.onUnSelect();

            m_selectedHero = value;

            //Debug.logger.Log("@@@@@@@@@@ " + m_selectedHero.RoleObject.name);
            m_selectedHero.onSelect();
            OnPropertyChanged(CURRENTHERO, m_selectedHero);
        }
    }

    public void ClearAttackAndAffectFlags()
    {
        foreach(RoleBase role in RolesInBattle())
        {
            role.OverlayItemModel.AffectAttack = false;
            role.OverlayItemModel.AffectHelp = false;
        }
    }

    /// <summary>
    /// 根据roleId(数据库内的ID）来取英雄数据
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public RoleBase GetRoleById(int roleId)
    {
        if (m_roleInBattleDic.ContainsKey(roleId))
        {
            return m_roleInBattleDic[roleId];
        }

        return null;
    }

    /// <summary>
    /// 将指定的Role放入战斗列表
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="player"></param>
    public void AddRoleInBattle(RoleInfo roleInfo, int pos)// RoleBase player)
    {
        if (!m_roleInBattleDic.ContainsKey(roleInfo.id))
        {
            m_roleInBattleDic.Add(roleInfo.id, RoleFactory.CreateRole(roleInfo));
            m_roleInBattleDic[roleInfo.id].m_playerPosition = pos;
        }

        RefreshSelectHero();
    }

    public void UpdateRolesOnBattleInDB()
    {
        List<RoleBase> roleBases = new List<RoleBase>();
        foreach (KeyValuePair<int, RoleBase> kv in m_roleInBattleDic)
        {
            if (kv.Value.IsHero())
            {
                roleBases.Add(kv.Value);
            }
        }

        roleBases.Sort();
        List<string> roleIds = new List<string>();
        foreach (RoleBase rb in roleBases)
        {
            roleIds.Add(rb.m_heroId.ToString());
        }

        GameData.Instance.GetCurrentUserInfo().RolesOnBattle = roleIds;
    }

    public void AddEnemyInBattle(RoleBase roleBase, int pos)
    {
        if (!m_roleInBattleDic.ContainsKey(roleBase.m_heroId))
        {
            m_roleInBattleDic.Add(roleBase.m_heroId, roleBase);
            m_roleInBattleDic[roleBase.m_heroId].m_playerPosition = pos;
        }

        EventService.Instance.GetEvent<AddEnemyInBattleEvent>().Publish(m_roleInBattleDic[roleBase.m_heroId], false);
    }

    /// <summary>
    /// 刷新选中英雄
    /// </summary>
    void RefreshSelectHero()
    {
        foreach (KeyValuePair<int, RoleBase> pair in m_roleInBattleDic)
        {
            SelectedHero = pair.Value;
            break;
        }
    }

    public void RemoveRoleInBattle(int roleId)
    {
        if (m_roleInBattleDic.ContainsKey(roleId))
        {
            m_roleInBattleDic.Remove(roleId);
        }

        RefreshSelectHero();
    }

    public RoleBase GetRoleInBattleById(int roleId)
    {
        if (m_roleInBattleDic.ContainsKey(roleId))
        {
            return m_roleInBattleDic[roleId];
        }

        return null;
    }

    public ICollection<RoleBase> RolesInBattle()
    {
        if (m_roleInBattleDic.Count <= 0)
            return null;

        return m_roleInBattleDic.Values;
    }

    #region Change Positions

    /// <summary>
    /// 让角色往左移动offset个位置
    /// </summary>
    /// <param name="role"></param>
    /// <param name="offset"></param>
    public void MoveRoleToLeft(RoleBase role, int offset)
    {
        if (role.m_playerPosition == 1)
            return;

        int targetPos = role.m_playerPosition - offset;
        int originalPos = role.m_playerPosition;
        foreach(KeyValuePair<int, RoleBase> rb in RoleInBattleDic)
        {
            if (role.m_heroId == rb.Value.m_heroId)
            {
                rb.Value.m_playerPosition -= offset;
                if (rb.Value.m_playerPosition < 1)
                    Debug.logger.LogError("RoleManager", "can't make position to pos " + rb.Value.m_playerPosition);
            }
            else
            {
                if (rb.Value.m_playerPosition >= targetPos && rb.Value.m_playerPosition < originalPos)
                {
                    rb.Value.m_playerPosition += 1;
                }
            }
        }

        EventService.Instance.GetEvent<RolePositionChangeEvent>().Publish();
    }

    /// <summary>
    /// 让角色往右移动offset个位置
    /// </summary>
    /// <param name="role"></param>
    /// <param name="offset"></param>
    public void MoveRoleToRight(RoleBase role, int offset)
    {
        if (role.m_playerPosition == 4)
            return;

        int targetPos = role.m_playerPosition + offset;
        int original = role.m_playerPosition;
        foreach (KeyValuePair<int, RoleBase> rb in RoleInBattleDic)
        {
            if (role.m_heroId == rb.Value.m_heroId)
            {
                rb.Value.m_playerPosition += offset;
                if (rb.Value.m_playerPosition > 4)
                    Debug.logger.LogError("RoleManager", "can't make position to pos " + rb.Value.m_playerPosition);
            }
            else
            {
                if (rb.Value.m_playerPosition <= targetPos && rb.Value.m_playerPosition > original)
                {
                    rb.Value.m_playerPosition -= 1;
                }
            }
        }

        EventService.Instance.GetEvent<RolePositionChangeEvent>().Publish();
    }

    public void ExchangeRoles(RoleBase role1, RoleBase role2)
    {
        int tmp = role1.m_playerPosition;
        role1.m_playerPosition = role2.m_playerPosition;
        role2.m_playerPosition = tmp;

        EventService.Instance.GetEvent<RolePositionChangeEvent>().Publish();
    }
    #endregion

    #region attack others or help team
    private void PlaySkill(RoleBase who, RoleBase toWho)
    {
        if (who == null)
            Debug.logger.Log("who is null");

        if (who.CurrentAbility == null)
            Debug.logger.Log("current ability is null");
        who.CurrentAbility.Perform();
        toWho.PlayOnBeSkilled(who.CurrentAbility);
        toWho.OnBeAffected(who.CurrentAbility, who);
    }

    public void AffectRole(RoleBase role)
    {
        if (role.OverlayItemModel.AffectAttack || role.OverlayItemModel.AffectHelp)
        {
            PlaySkill(SelectedHero, role);
        }
    }
    #endregion

    
}
