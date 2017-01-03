using UnityEngine;
using System.Collections;
using System;
using Mono.Data.Sqlite;
using System.Collections.Generic;

public class GameDB {
    #if UNITY_EDITOR || UNITY_STANDALONE
	public static string appDBPath = Application.dataPath + "/GameDB.db";
	DbAccess db = new DbAccess("Data Source=" + appDBPath);
	#elif UNITY_IOS
	public static string appDBPath = Application.persistentDataPath + "/avatar.db";
	DbAccess db = new DbAccess("Data Source=" + appDBPath);
	#elif UNITY_ANDROID
	public static string appDBPath = Application.persistentDataPath + "/avatar.db";
	DbAccess db = new DbAccess("URI=file:" + appDBPath);
	#endif

	private static GameDB instance = null;

	public static GameDB Instance
	{
		get
		{
			if (instance == null)
				instance = new GameDB();

			return instance;
		}
	}

    public GameDB()
	{
		Init();
	}
	
	public void Init()
	{
		try
		{
            db.CreateTable("Player", new string[] { "id", "lv", "roles", "hero_limit", "name", "rolesOnBattle" }, new string[] { "integer PRIMARY KEY", "text", "text", "text", "text", "text" });
            db.CreateTable("Role", new string[] { "id", "type", "style", "level", "name", "skill1", "skill2", "skill3", "skill4", "skill5", "skill6", "skill7", "usedSkill" },
                new string[] { "integer PRIMARY KEY", "text", "text", "text", "text", "text", "text", "text", "text", "text", "text", "text", "text" });
		}
		catch(Exception ex)
		{
            Debug.logger.Log("exception is: " + ex.Message);
		}
	}

    #region Player Table
    public void UpdateUserInfo(int id, int lv, string heros, int hero_limit, string name, string rolesOnBattle)
	{
        db.UpdateInto("Player", new string[] { "lv", "roles", "hero_limit", "name", "rolesOnBattle" }, new string[] { lv.ToString(), heros, hero_limit.ToString(), name, rolesOnBattle }, "id", id.ToString());
	}
	
	public UserInfo GetPlayerById(int id)
	{

        UserInfo userInfo = null;
        SqliteDataReader sqReader = db.SelectWhere("Player", new string[] { "lv", "roles", "hero_limit", "name", "rolesOnBattle" }, new string[] { "id" }, new string[] { "=" }, new string[] { id.ToString() });
		
		while (sqReader.Read())
		{
            //userInfo.UserId = int.Parse(sqReader.GetString(sqReader.GetOrdinal("id")));
			string strLv = sqReader.GetString(sqReader.GetOrdinal("lv"));
            int level = int.Parse(strLv);
            string strRoles = sqReader.GetString(sqReader.GetOrdinal("roles"));
            string[] tmp = strRoles.Split(':');
            List<string> roles = new List<string>();
            for (int i = 0; i < tmp.Length; i++ )
            {
                if (string.IsNullOrEmpty(tmp[i]))
                {
                    continue;
                }
                roles.Add(tmp[i]);
            }
            
            string strLimit = sqReader.GetString(sqReader.GetOrdinal("hero_limit"));
            int heroLimited = int.Parse(strLimit);
            string name = sqReader.GetString(sqReader.GetOrdinal("name"));

            string strRolesOnBattle = sqReader.GetString(sqReader.GetOrdinal("rolesOnBattle"));
            tmp = strRolesOnBattle.Split(':');
            List<string> rolesOnBattle = new List<string>();
            for (int i = 0; i < tmp.Length; i++)
            {
                if (string.IsNullOrEmpty(tmp[i]))
                    continue;

                rolesOnBattle.Add(tmp[i]);
            }

            userInfo = new UserInfo(id, level, roles, heroLimited, name, rolesOnBattle);
		}

        return userInfo;
    }

    public int AddUserInfo(int level, string heros, int limited, string name, string rolesOnBattle)
    {
        int id = -1;
        object sqReader = db.InsertIntoSpecific("Player", new string[] { "lv", "roles", "hero_limit", "name", "rolesOnBattle" },
            new string[] { level.ToString(), heros.ToString(), limited.ToString(), name, rolesOnBattle });

        if (sqReader != null)
            id = Convert.ToInt32(sqReader.ToString());

        return id;
    }
    #endregion

    #region Role Table
    public void UpdateRole(string id, string type, string roleId, string level, string name,
        string skill1, string skill2, string skill3, string skill4, string skill5, string skill6, string skill7, string usedSkillIndex)
    {
        db.UpdateInto("Role", new string[] { "type", "style", "level", "name", "skill1", "skill2", "skill3", "skill4", "skill5", "skill6", "skill7", "usedSkill" },
            new string[] { type, roleId, level, name, skill1, skill2, skill3, skill4, skill5, skill6, skill7, usedSkillIndex }, "id", id.ToString());
    }

    public void DeleteRoleById(int id)
    {
        db.Delete("Role", new string[] { "id" }, new string[] { id.ToString() });
    }

    public RoleInfo GetRoleById(int id)
    {
        RoleInfo roleInfo = new RoleInfo();
        roleInfo.Level = -1;
        SqliteDataReader sqReader = db.SelectWhere("Role", new string[] { "type", "style", "level", "name", "skill1", "skill2", "skill3", "skill4", "skill5", "skill6", "skill7", "usedSkill" }, 
            new string[] { "id" }, new string[] { "=" }, new string[] { id.ToString() });

        while (sqReader.Read())
        {
            string strType = sqReader.GetString(sqReader.GetOrdinal("type"));
            roleInfo.type = (CommonDefine.RoleType)int.Parse(strType);
            string strRoleId = sqReader.GetString(sqReader.GetOrdinal("style"));
            roleInfo.style = (CommonDefine.RoleStyle)(int.Parse(strRoleId));
            string strLv = sqReader.GetString(sqReader.GetOrdinal("level"));
            roleInfo.Level = int.Parse(strLv);
            roleInfo.Name = sqReader.GetString(sqReader.GetOrdinal("name"));
            string strSkill1 = sqReader.GetString(sqReader.GetOrdinal("skill1"));
            roleInfo.UpdateSkill1(int.Parse(strSkill1));
            string strSkill2 = sqReader.GetString(sqReader.GetOrdinal("skill2"));
            roleInfo.UpdateSkill2(int.Parse(strSkill2));
            string strSkill3 = sqReader.GetString(sqReader.GetOrdinal("skill3"));
            roleInfo.UpdateSkill3(int.Parse(strSkill3));
            string strSkill4 = sqReader.GetString(sqReader.GetOrdinal("skill4"));
            roleInfo.UpdateSkill4(int.Parse(strSkill4));
            string strSkill5 = sqReader.GetString(sqReader.GetOrdinal("skill5"));
            roleInfo.UpdateSkill5(int.Parse(strSkill5));
            string strSkill6 = sqReader.GetString(sqReader.GetOrdinal("skill6"));
            roleInfo.UpdateSkill6(int.Parse(strSkill6));
            string strSkill7 = sqReader.GetString(sqReader.GetOrdinal("skill7"));
            roleInfo.UpdateSkill7(int.Parse(strSkill7));
            roleInfo.InitUsedSkillIndex(sqReader.GetString(sqReader.GetOrdinal("usedSkill")));
            roleInfo.id = id;
        }

        return roleInfo;
    }

    public int AddRole(int id, CommonDefine.RoleType type, CommonDefine.RoleStyle style, int level, string name,
        int skill1, int skill2, int skill3, int skill4, int skill5, int skill6, int skill7, string usedSkill)
    {
        object sqReader = db.InsertIntoSpecific("Role", new string[] { "type", "style", "level", "name", "skill1", "skill2", "skill3", "skill4", "skill5", "skill6", "skill7", "usedSkill" },
            new string[] {((int)type).ToString(), ((int)style).ToString(), level.ToString(), 
                name, skill1.ToString(), skill2.ToString(), skill3.ToString(), skill4.ToString(), skill5.ToString(), skill6.ToString(), skill7.ToString(), usedSkill });
        
        id = -1;
        if (sqReader != null)
            id = Convert.ToInt32(sqReader.ToString());

        return id;
    }
    #endregion

}
