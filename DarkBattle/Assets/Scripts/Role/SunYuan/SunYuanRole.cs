using UnityEngine;
using System.Collections;

public class SunYuanRole : RoleBase
{
    public SunYuanRole(RoleInfo roleInfo) 
        : base(roleInfo)
    {

    }

    protected override void InitialAbilities()
    {
        SunYuanSkill1 skill1 = new SunYuanSkill1(m_roleInfo.Skill1, m_roleData.skill1, 0, this);
        SunYuanSkill2 skill2 = new SunYuanSkill2(m_roleInfo.Skill2, m_roleData.skill2, 1, this);
        SunYuanSkill3 skill3 = new SunYuanSkill3(m_roleInfo.Skill3, m_roleData.skill3, 2, this);
        SunYuanSkill4 skill4 = new SunYuanSkill4(m_roleInfo.Skill4, m_roleData.skill4, 3, this);
        SunYuanSkill5 skill5 = new SunYuanSkill5(m_roleInfo.Skill5, m_roleData.skill5, 4, this);
        SunYuanSkill6 skill6 = new SunYuanSkill6(m_roleInfo.Skill6, m_roleData.skill6, 5, this);
        SunYuanSkill7 skill7 = new SunYuanSkill7(m_roleInfo.Skill7, m_roleData.skill7, 6, this);

        m_abilities.Add(skill1);
        m_abilities.Add(skill2);
        m_abilities.Add(skill3);
        m_abilities.Add(skill4);
        m_abilities.Add(skill5);
        m_abilities.Add(skill6);
        m_abilities.Add(skill7);
    }
}
