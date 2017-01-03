using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GuiMianJunZiRole : RoleBase
{
    public GuiMianJunZiRole(RoleInfo roleInfo)
        : base(roleInfo)
    {

    }

    protected override void InitialAbilities()
    {
        GuiMianSkill1 skill1 = new GuiMianSkill1(m_roleInfo.Skill1, m_roleData.skill1, 0, this);
        GuiMianSkill2 skill2 = new GuiMianSkill2(m_roleInfo.Skill2, m_roleData.skill2, 1, this);
        GuiMianSkill3 skill3 = new GuiMianSkill3(m_roleInfo.Skill3, m_roleData.skill3, 2, this);
        GuiMianSkill4 skill4 = new GuiMianSkill4(m_roleInfo.Skill4, m_roleData.skill4, 3, this);
        GuiMianSkill5 skill5 = new GuiMianSkill5(m_roleInfo.Skill5, m_roleData.skill5, 4, this);
        GuiMianSkill6 skill6 = new GuiMianSkill6(m_roleInfo.Skill6, m_roleData.skill6, 5, this);
        GuiMianSkill7 skill7 = new GuiMianSkill7(m_roleInfo.Skill7, m_roleData.skill7, 6, this);

        m_abilities.Add(skill1);
        m_abilities.Add(skill2);
        m_abilities.Add(skill3);
        m_abilities.Add(skill4);
        m_abilities.Add(skill5);
        m_abilities.Add(skill6);
        m_abilities.Add(skill7);
    }

    public override void ToAttackPerson()
    {
        if (ChooseSkill())
        {
            GameData.Instance.BattleSceneActionFlag.RemoveFlag((long)StateDef.BattleActionFlag.OnChoosingSkill);
            GameData.Instance.BattleSceneActionFlag.AddFlag((long)StateDef.BattleActionFlag.OnAttacking);

            List<RandomObject> ros = new List<RandomObject>();

            foreach (RoleBase play in RoleManager.Instance.RolesInBattle())
            {
                if (CurrentAbility.SkillData.affectpositions.Contains(play.m_playerPosition))
                {
                    RandomObject ro = new RandomObject();
                    ro.ItemId = play.m_playerPosition;
                    ro.Weight = 1;
                    if (play.OverlayItemModel.IsTaged)
                        ro.Weight += 1;
                    ros.Add(ro);
                }
            }

            List<RandomObject> retObjs = ProjectHelper.GetRandomList<RandomObject>(ros, 1);
            RoleBase toWho = RoleManager.Instance.RolesInBattle().Where(role => role.m_playerPosition == retObjs[0].ItemId).FirstOrDefault();
            if (toWho != null)
            {
                RoleManager.Instance.AffectRole(toWho);
            }
        }
        else
        {
            //TODO：移动位置。。。。
            Debug.logger.Log("bad luck..............");
        }
    }

    protected override bool ChooseSkill()
    {
        List<AbilityBase> availableSkills = new List<AbilityBase>();
        foreach(AbilityBase skill in m_abilities)
        {
            if (skill.IsValid())
            {
                availableSkills.Add(skill);
            }
        }

        if (availableSkills.Count > 0)
        {
            List<RandomObject> objs = new List<RandomObject>();
            foreach (AbilityBase ab in availableSkills)
            {
                if (ab != null)
                {
                    RandomObject ro = new RandomObject();
                    ro.ItemId = ab.SkillData.id;
                    ro.Weight = ab.SkillData.weight;
                    objs.Add(ro);
                }
            }

            if (objs.Count > 0)
            {
                List<RandomObject> retObjs = ProjectHelper.GetRandomList<RandomObject>(objs, 1);
                AbilityBase abb = m_abilities.Where(ability => ability.SkillData.id == retObjs[0].ItemId).FirstOrDefault();
                if (abb != null)
                {
                    abb.onSelect();
                    return true;
                }
            }
        }

        return false;
    }
}
