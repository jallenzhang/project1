using UnityEngine;
using System.Collections;

public class NunRole : RoleBase {

	public NunRole(RoleInfo roleInfo) : base(roleInfo)
    {

    }

    protected override void InitialAbilities()
    {
        NunSkill1 skill1 = new NunSkill1(m_roleInfo.Skill1, m_roleData.skill1, 0, this);
        NunSkill2 skill2 = new NunSkill2(m_roleInfo.Skill2, m_roleData.skill2, 1, this);
        NunSkill3 skill3 = new NunSkill3(m_roleInfo.Skill3, m_roleData.skill3, 2, this);
        NunSkill4 skill4 = new NunSkill4(m_roleInfo.Skill4, m_roleData.skill4, 3, this);
        NunSkill5 skill5 = new NunSkill5(m_roleInfo.Skill5, m_roleData.skill5, 4, this);
        NunSkill6 skill6 = new NunSkill6(m_roleInfo.Skill6, m_roleData.skill6, 5, this);
        NunSkill7 skill7 = new NunSkill7(m_roleInfo.Skill7, m_roleData.skill7, 6, this);

        m_abilities.Add(skill1);
        m_abilities.Add(skill2);
        m_abilities.Add(skill3);
        m_abilities.Add(skill4);
        m_abilities.Add(skill5);
        m_abilities.Add(skill6);
        m_abilities.Add(skill7);
    }
}
