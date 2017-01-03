using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillGenerator {
    public static string CreateDefaultSkills(CommonDefine.RoleType roleType)
    {
        List<RandomObject> objs = new List<RandomObject>();
        RoleData role = RoleData.GetRoleDataByID((int)roleType);
        RandomObject ro1 = new RandomObject();
        ro1.ItemId = role.skill1;
        ro1.Weight = SkillData.GetSkillDataByID(role.skill1).weight;
        objs.Add(ro1);

        RandomObject ro2 = new RandomObject();
        ro2.ItemId = role.skill2;
        ro2.Weight = SkillData.GetSkillDataByID(role.skill2).weight;
        objs.Add(ro2);

        RandomObject ro3 = new RandomObject();
        ro3.ItemId = role.skill3;
        ro3.Weight = SkillData.GetSkillDataByID(role.skill3).weight;
        objs.Add(ro3);

        RandomObject ro4 = new RandomObject();
        ro4.ItemId = role.skill4;
        ro4.Weight = SkillData.GetSkillDataByID(role.skill4).weight;
        objs.Add(ro4);

        RandomObject ro5 = new RandomObject();
        ro5.ItemId = role.skill5;
        ro5.Weight = SkillData.GetSkillDataByID(role.skill5).weight;
        objs.Add(ro5);

        RandomObject ro6 = new RandomObject();
        ro6.ItemId = role.skill6;
        ro6.Weight = SkillData.GetSkillDataByID(role.skill6).weight;
        objs.Add(ro6);

        RandomObject ro7 = new RandomObject();
        ro7.ItemId = role.skill7;
        ro7.Weight = SkillData.GetSkillDataByID(role.skill7).weight;
        objs.Add(ro7);

        List<RandomObject> retObjs = ProjectHelper.GetRandomList<RandomObject>(objs, 4);
        if (retObjs == null)
            Debug.logger.LogError("DungeonPoint", "random objet occurs error");

        string ret = string.Empty;

        foreach(RandomObject obj in retObjs)
        {
            if (!string.IsNullOrEmpty(ret))
                ret += ":";
            ret += (obj.ItemId % (int)roleType).ToString();
        }

        return ret;
    }

    public static List<int> GetAvailableSkills(CommonDefine.RoleType roleType, int pos)
    {
        List<int> ret = new List<int>();
        RoleData role = RoleData.GetRoleDataByID((int)roleType);
        Debug.logger.Log("pos " + pos);
        List<int> poses = SkillData.GetSkillDataByID(role.skill1).positions;
        if (poses.Contains(pos))
        {
            Debug.logger.Log("role.skill1 " + role.skill1);
            ret.Add(role.skill1);
        }

        poses.Clear();
        poses = SkillData.GetSkillDataByID(role.skill2).positions;
        if (poses.Contains(pos))
        {
            Debug.logger.Log("role.skill2 " + role.skill2);
            ret.Add(role.skill2);
        }

        poses.Clear();
        poses = SkillData.GetSkillDataByID(role.skill3).positions;
        if (poses.Contains(pos))
        {
            Debug.logger.Log("role.skill3 " + role.skill3);
            ret.Add(role.skill3);
        }

        poses.Clear();
        poses = SkillData.GetSkillDataByID(role.skill4).positions;
        if (poses.Contains(pos))
        {
            Debug.logger.Log("role.skill4 " + role.skill4);
            ret.Add(role.skill4);
        }

        poses.Clear();
        poses = SkillData.GetSkillDataByID(role.skill5).positions;
        if (poses.Contains(pos))
        {
            Debug.logger.Log("role.skill5 " + role.skill5);
            ret.Add(role.skill5);
        }

        poses.Clear();
        poses = SkillData.GetSkillDataByID(role.skill6).positions;
        if (poses.Contains(pos))
        {
            Debug.logger.Log("role.skill6 " + role.skill6);
            ret.Add(role.skill6);
        }

        poses.Clear();
        poses = SkillData.GetSkillDataByID(role.skill7).positions;
        if (poses.Contains(pos))
        {
            Debug.logger.Log("role.skill7 " + role.skill7);
            ret.Add(role.skill7);
        }
        return ret;
    }
}
