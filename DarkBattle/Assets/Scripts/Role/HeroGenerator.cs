using UnityEngine;
using System.Collections;
using System;

public class HeroGenerator : MonoBehaviour {
    private static HeroGenerator s_instance = null;
    public static HeroGenerator Instance
    {
        get
        {
            return s_instance;
        }
    }

	// Use this for initialization
    void Awake()
    {
        s_instance = this;
    }

    public Hero Generator(CommonDefine.RoleType roleType, int pos = -1)
    {
        Hero ret = null;
        RoleInfo info = GameData.Instance.AddRoleIntoDB(roleType);//RoleFactory.CreateRoleInfo(roleType);

        if (info != null && pos != -1)
        {
            ret = RoleManager.Instance.CreateHero(info, pos);
            RoleManager.Instance.UpdateRolesOnBattleInDB();
        }

        return ret;
    }

    public Hero Generator(int roleDBId, int pos)
    {
        Hero ret = null;
        RoleInfo info = GameData.Instance.GetCurrentUserInfo().GetRoleInfo(roleDBId);

        if (info != null)
        {
            ret = RoleManager.Instance.CreateHero(info, pos);
            RoleManager.Instance.UpdateRolesOnBattleInDB();
        }

        return ret;
    }
}
