using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class UserInfo {
    public int UserId;

    private List<RoleInfo> m_roleInfos = new List<RoleInfo>();
    public List<RoleInfo> RoleInfos
    {
        get
        {
            return m_roleInfos;
        }
    }

    private int m_level;
    public int Level
    {
        get
        {
            return m_level;
        }
        set
        {
            if (m_level != value)
            {
                m_level = value;
                UpdateUserInfo();
            }
        }
    }

    public List<string> Roles;

    private List<string> m_rolesOnBattle = new List<string>();
    public List<string> RolesOnBattle
    {
        get
        {
            return m_rolesOnBattle;
        }
        set
        {
            m_rolesOnBattle = value;
            UpdateUserInfo();
        }
    }

    private int m_heroLimited;
    public int HeroLimited
    {
        get
        {
            return m_heroLimited;
        }
        set
        {
            if (m_heroLimited != value)
            {
                m_heroLimited = value;
                UpdateUserInfo();
            }
        }
    }

    private string m_name;
    public string Name
    {
        get
        {
            return m_name;
        }
        set
        {
            if (m_name != value)
            {
                m_name = value;
                UpdateUserInfo();
            }
        }
    }



    public UserInfo(int userId, int level, List<string> roles, int heroLimited, string name, List<string> rolesOnBattle)
    {
        UserId = userId;
        m_level = level;
        if (roles != null)
        {
            foreach (string role in roles)
            {
                if (role == string.Empty)
                    roles.Remove(role);
            }
        }
        
        Roles = roles;

        if (rolesOnBattle != null)
        {
            foreach(string role in rolesOnBattle)
            {
                if (role == string.Empty)
                    rolesOnBattle.Remove(role);
            }
            m_rolesOnBattle = rolesOnBattle;
        }

        m_heroLimited = heroLimited;
        m_name = name;
    }

    /// <summary>
    /// 同步数据userinfo to player db, 重组Roles to string
    /// </summary>
    private void UpdateUserInfo()
    {
        StringBuilder sb = new StringBuilder();
        if (Roles != null && Roles.Count > 0)
        {
            sb.Append(Roles[0]);
            for (int i = 1; i < Roles.Count; i++)
            {
                sb.Append(":");
                sb.Append(Roles[i]);
            }
        }

        StringBuilder sbOnBattle = new StringBuilder();
        List<string> finalList = new List<string>() {"0", "0", "0", "0"};
        foreach(string str in m_rolesOnBattle)
        {
            RoleBase rb = RoleManager.Instance.GetRoleById(int.Parse(str));
            if (rb != null)
                finalList[rb.m_playerPosition] = str;
        }

        if (finalList != null && finalList.Count > 0)
        {
            sbOnBattle.Append(finalList[0]);
            for (int i = 1; i < finalList.Count; i++)
            {
                sbOnBattle.Append(":");
                sbOnBattle.Append(finalList[i]);
            }
        }

        GameDB.Instance.UpdateUserInfo(UserId, Level, sb.ToString(), HeroLimited, Name, sbOnBattle.ToString());
    }

    #region Roles Operation

    public List<RoleInfo> FetchRoleData()
    {
        if (Roles == null)
            return null;

        if (m_roleInfos.Count == 0)
        {
            Debug.logger.Log("Roles count is: " + Roles.Count);
            for (int i = 0; i < Roles.Count; i++)
            {
                RoleInfo role = GameDB.Instance.GetRoleById(int.Parse(Roles[i]));
                m_roleInfos.Add(role);
            }
        }

        return m_roleInfos;
    }

    public RoleInfo GetRoleInfo(int id)
    {
        if (m_roleInfos == null)
            return null;

        foreach(RoleInfo role in m_roleInfos)
        {
            if (role.id == id)
                return role;
        }

        return null;
    }

    /// <summary>
    /// 增加一个role在这个用户下
    /// </summary>
    /// <param name="type"></param>
    /// <param name="style"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public RoleInfo AddRole(CommonDefine.RoleType type)
    {
        RoleInfo roleInfo = RoleFactory.CreateRoleInfo(type);
        roleInfo = InsertRoleInfoIntoDB(roleInfo);
        AddRoleIntoUserInfo(roleInfo);

        return roleInfo;
    }

    /// <summary>
    /// 创建一个新的RoleInfo，并将其加入到role db表内
    /// </summary>
    /// <param name="type"></param>
    /// <param name="style"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    private RoleInfo InsertRoleInfoIntoDB(RoleInfo roleInfo)
    {
        int id = GameDB.Instance.AddRole(0, roleInfo.type, roleInfo.style, 1, roleInfo.Name, roleInfo.Skill1,
            roleInfo.Skill2, roleInfo.Skill3, roleInfo.Skill4, roleInfo.Skill5, roleInfo.Skill6, roleInfo.Skill7, roleInfo.Skills);
        roleInfo.id = id;

        return roleInfo;
    }

    /// <summary>
    /// 从userinfo里删除一个role，并更新Role和User表
    /// </summary>
    /// <param name="id"></param>
    public void RemoveRoleInfoFromUserInfo(int id)
    {
        RoleInfo info = GetRoleInfo(id);
        if(info != null)
        {
            m_roleInfos.Remove(info);
            GameDB.Instance.DeleteRoleById(id);
            foreach(string strRole in Roles)
            {
                if (strRole == id.ToString())
                {
                    Roles.Remove(strRole);
                }
            }

            UpdateUserInfo();
        }
    }

    /// <summary>
    /// 将role加入到userInfo下，并且更新Player DB表
    /// </summary>
    /// <param name="roleInfo"></param>
    private void AddRoleIntoUserInfo(RoleInfo roleInfo)
    {
        m_roleInfos.Add(roleInfo);
        if (Roles == null)
            Roles = new List<string>();

        Roles.Add(roleInfo.id.ToString());
        UpdateUserInfo();
    }
    #endregion
}
