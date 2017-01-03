using UnityEngine;
using System.Collections;

public class SoldierRole : RoleBase {

	public SoldierRole(RoleInfo roleInfo) 
        : base(roleInfo)
    {

    }

    protected override void InitialAbilities()
    {
        SoldierSkill1 skill1 = new SoldierSkill1(m_roleInfo.Skill1, m_roleData.skill1, 0, this);
        SoldierSkill2 skill2 = new SoldierSkill2(m_roleInfo.Skill2, m_roleData.skill2, 1, this);
        SoldierSkill3 skill3 = new SoldierSkill3(m_roleInfo.Skill3, m_roleData.skill3, 2, this);
        SoldierSkill4 skill4 = new SoldierSkill4(m_roleInfo.Skill4, m_roleData.skill4, 3, this);
        SoldierSkill5 skill5 = new SoldierSkill5(m_roleInfo.Skill5, m_roleData.skill5, 4, this);
        SoldierSkill6 skill6 = new SoldierSkill6(m_roleInfo.Skill6, m_roleData.skill6, 5, this);
        SoldierSkill7 skill7 = new SoldierSkill7(m_roleInfo.Skill7, m_roleData.skill7, 6, this);

        m_abilities.Add(skill1);
        m_abilities.Add(skill2);
        m_abilities.Add(skill3);
        m_abilities.Add(skill4);
        m_abilities.Add(skill5);
        m_abilities.Add(skill6);
        m_abilities.Add(skill7);
    }
}
