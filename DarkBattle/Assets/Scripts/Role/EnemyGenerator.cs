using System;
using System.Collections;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {
    private static EnemyGenerator s_instance = null;
    public static EnemyGenerator Instance
    {
        get
        {
            return s_instance;
        }
    }

	// Use this for initialization
	void Start () {
        s_instance = this;
	}
	
    public Enemy Generator(CommonDefine.RoleType roleType, int pos)
    {
        Enemy ret = null;
        
        RoleInfo info = RoleFactory.CreateRoleInfo(roleType);

        if (info != null)
            ret = RoleManager.Instance.CreateEnemy(info, pos);

        return ret;
    }
}
