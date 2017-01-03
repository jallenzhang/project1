using UnityEngine;
using System.Collections;
using System;

public class RoleFactory {
    protected static string HeroPath = "Prefabs/Roles/Heros/";
    protected static string EnemyPath = "Prefabs/Roles/Enemies/";

    private static string RoleFormat = "{0}Role";
    private static Hashtable lookupType = new Hashtable();
    public static GameObject CreateRoleObject(CommonDefine.RoleType roleType, CommonDefine.RoleStyle roleStyle)
    {
        string tmp = string.Empty;
        if (roleType > CommonDefine.RoleType.Hero && roleType < CommonDefine.RoleType.Enemy)
            tmp = HeroPath;
        else if (roleType > CommonDefine.RoleType.Enemy)
            tmp = EnemyPath;

        GameObject obj = ResMgr.Instance.LoadAssetFromResource(tmp + CommonDefine.RoleNameDic[roleType]) as GameObject;
        GameObject roleObj = GameObject.Instantiate(obj);


        return roleObj;
    }

    public static RoleBase CreateRole(RoleInfo roleInfo)
    {
        RoleBase roleBase = null;
        try
        {
            string name = string.Format(RoleFormat, ((CommonDefine.RoleType)roleInfo.type).ToString());
            var type = (Type)lookupType[name];
            lock(lookupType)
            {
                if (type == null)
                {
                    type = Type.GetType(name);
                    lookupType[name] = type;
                }
                if (type != null)
                {
                    roleBase = Activator.CreateInstance(type, roleInfo) as RoleBase;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("create role error: " + ex);
        }

        return roleBase;
    }

    public static RoleInfo CreateRoleInfo(CommonDefine.RoleType roleType)
    {
        RoleInfo info = new RoleInfo();
        RoleData roleData = RoleData.GetRoleDataByID((int)roleType);
        info.type = roleType;
        info.style = (CommonDefine.RoleStyle)roleData.style;
        info.Name = roleData.name;
        info.Level = 1;
        info.id = Guid.NewGuid().GetHashCode();
        info.Skills = SkillGenerator.CreateDefaultSkills(roleType);
        int[] levels = ProjectHelper.InitSkillLevel(info.Skills);
        info.UpdateSkill1(levels[0]);
        info.UpdateSkill2(levels[1]);
        info.UpdateSkill3(levels[2]);
        info.UpdateSkill4(levels[3]);
        info.UpdateSkill5(levels[4]);
        info.UpdateSkill6(levels[5]);
        info.UpdateSkill7(levels[6]);

        return info;
    }
}