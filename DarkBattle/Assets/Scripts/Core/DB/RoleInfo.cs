using UnityEngine;
using System.Collections;

/// <summary>
/// 数据库对应的数据
/// </summary>
public class RoleInfo {
    public int id;
    public CommonDefine.RoleStyle style;
    public string Name;
    public CommonDefine.RoleType type;
    public int Level;
    public string Skills;

    private bool[] m_skillsInUse = new bool[CommonDefine.RoleSkillCount];
    /// <summary>
    /// 0表示未解锁， 1表示这个等级是1级....
    /// </summary>
    public int Skill1
    {
        get;
        private set;
    }
    /// <summary>
    /// 0表示未解锁， 1表示这个等级是1级....
    /// </summary>
    public int Skill2
    {
        get;
        private set;
    }
    /// <summary>
    /// 0表示未解锁， 1表示这个等级是1级....
    /// </summary>
    public int Skill3
    {
        get;
        private set;
    }
    /// <summary>
    /// 0表示未解锁， 1表示这个等级是1级....
    /// </summary>
    public int Skill4
    {
        get;
        private set;
    }
    /// <summary>
    /// 0表示未解锁， 1表示这个等级是1级....
    /// </summary>
    public int Skill5
    {
        get;
        private set;
    }
    /// <summary>
    /// 0表示未解锁， 1表示这个等级是1级....
    /// </summary>
    public int Skill6
    {
        get;
        private set;
    }
    /// <summary>
    /// 0表示未解锁， 1表示这个等级是1级....
    /// </summary>
    public int Skill7
    {
        get;
        private set;
    }

    public string UsedSkillIndex
    {
        get;
        private set;
    }

    public void UpdateToDB()
    {
        GameDB.Instance.UpdateRole(id.ToString(), ((int)type).ToString(), ((int)style).ToString(), Level.ToString(),
            Name, Skill1.ToString(), Skill2.ToString(), Skill3.ToString(), Skill4.ToString(), Skill5.ToString(),Skill6.ToString(), Skill7.ToString(), UsedSkillIndex);
    }

    public void UpdateSkill1(int level, bool needUpdate = false)
    {
        this.Skill1 = level;
        if (needUpdate)
            UpdateToDB();
    }
    public void UpdateSkill2(int level, bool needUpdate = false)
    {
        this.Skill2 = level;
        if (needUpdate)
            UpdateToDB();
    }
    public void UpdateSkill3(int level, bool needUpdate = false)
    {
        this.Skill3 = level;
        if (needUpdate)
            UpdateToDB();
    }
    public void UpdateSkill4(int level, bool needUpdate = false)
    {
        this.Skill4 = level;
        if (needUpdate)
            UpdateToDB();
    }
    public void UpdateSkill5(int level, bool needUpdate = false)
    {
        this.Skill5 = level;
        if (needUpdate)
            UpdateToDB();
    }
    public void UpdateSkill6(int level, bool needUpdate = false)
    {
        this.Skill6 = level;
        if (needUpdate)
            UpdateToDB();
    }
    public void UpdateSkill7(int level, bool needUpdate = false)
    {
        this.Skill7 = level;
        if (needUpdate)
            UpdateToDB();
    }

    public void InitUsedSkillIndex(string usedSkill)
    {
        this.UsedSkillIndex = usedSkill;
        string[] skills = usedSkill.Split(':');
        foreach(string skill in skills)
        {
            m_skillsInUse[int.Parse(skill)] = true;
        }
    }

    public void UpdateSkillInUseList(int idx, bool inUse)
    {
        if (m_skillsInUse[idx] != inUse)
        {
            m_skillsInUse[idx] = inUse;
            this.UsedSkillIndex = UpdateUsedSkillIndex();
            UpdateToDB();
        }
    }

    public bool CheckSkillInUse(int idx)
    {
        return m_skillsInUse[idx];
    }

    private string UpdateUsedSkillIndex()
    {
        string tmp = string.Empty;
        for (int i = 0; i < CommonDefine.RoleSkillCount; i++)
        {
            if (m_skillsInUse[i])
            {
                if (tmp.Equals(string.Empty))
                    tmp = i.ToString();
                else
                {
                    tmp += ":";
                    tmp += i.ToString();
                }
            }
        }

        return tmp;
    }
}
