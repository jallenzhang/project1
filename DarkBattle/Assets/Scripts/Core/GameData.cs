using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameData{

    private static GameData s_instance = null;
    public static GameData Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = new GameData();

            return s_instance;
        }
    }

    public CommonFlag BattleSceneActionFlag = new CommonFlag();

    private UserInfo m_currentUser = null;

    public bool FetchData()
    {
        bool ret = false;
        ret = GetCurrentUserInfo() != null;

        return ret;
    }

    public UserInfo GetCurrentUserInfo()
    {
        if (m_currentUser != null)
            return m_currentUser;

        m_currentUser = GameDB.Instance.GetPlayerById(SettingManager.Instance.UserID);
        if (m_currentUser != null)
        {
            m_currentUser.FetchRoleData();
            return m_currentUser;
        }

        int id = GameDB.Instance.AddUserInfo(1, string.Empty, 0, string.Empty, "0:0:0:0");
        if (id != -1)
        {
            m_currentUser = new UserInfo(id, 1, null, 10, string.Empty, null);
            SettingManager.Instance.UserID = id;
            return m_currentUser;
        }

        return m_currentUser;
    }

    #region Role Info Operation
    public RoleInfo GetRoleInfo(int id)
    {
        if (m_currentUser == null)
            GetCurrentUserInfo();

        return m_currentUser.GetRoleInfo(id);
    }
    /// <summary>
    /// 创建一个新的角色给这个用户，放入DB
    /// </summary>
    /// <param name="type">6001</param>
    /// <param name="style">1</param>
    /// <param name="name">zzz</param>
    /// <param name="usedSkill">"1:3:4"</param>
    /// <returns></returns>
    public RoleInfo AddRoleIntoDB(CommonDefine.RoleType type)
    {
        if (m_currentUser == null)
            GetCurrentUserInfo();

        return m_currentUser.AddRole(type);
    }

    public void RemoveRoleInfo(int roleId)
    {
        m_currentUser.RemoveRoleInfoFromUserInfo(roleId);
    }
    #endregion
}
