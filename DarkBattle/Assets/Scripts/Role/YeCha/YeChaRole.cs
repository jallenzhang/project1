using UnityEngine;
using System.Collections;

public class YeChaRole : RoleBase {
    public YeChaRole(RoleInfo roleInfo) 
        : base(roleInfo)
    {

    }

    protected override void InitialAbilities()
    {
        YeChaSkill1 skill1 = new YeChaSkill1(m_roleInfo.Skill1, m_roleData.skill1, 0, this);
        YeChaSkill2 skill2 = new YeChaSkill2(m_roleInfo.Skill2, m_roleData.skill2, 1, this);
        YeChaSkill3 skill3 = new YeChaSkill3(m_roleInfo.Skill3, m_roleData.skill3, 2, this);
        YeChaSkill4 skill4 = new YeChaSkill4(m_roleInfo.Skill4, m_roleData.skill4, 3, this);
        YeChaSkill5 skill5 = new YeChaSkill5(m_roleInfo.Skill5, m_roleData.skill5, 4, this);
        YeChaSkill6 skill6 = new YeChaSkill6(m_roleInfo.Skill6, m_roleData.skill6, 5, this);
        YeChaSkill7 skill7 = new YeChaSkill7(m_roleInfo.Skill7, m_roleData.skill7, 6, this);

        m_abilities.Add(skill1);
        m_abilities.Add(skill2);
        m_abilities.Add(skill3);
        m_abilities.Add(skill4);
        m_abilities.Add(skill5);
        m_abilities.Add(skill6);
        m_abilities.Add(skill7);
    }
}
