using UnityEngine;
using System.Collections;
using Game.AIBehaviorTree;
using System.Collections.Generic;

public class RoleInput : BInput {
    public RoleBase Parent;
    public int HP;
    public int Pressure;
    public float Agility =100;
    public int Level;

    public RoleInput(RoleBase parent)
    {
        this.Parent = parent;
    }
}

public class SkillData : XMLData<SkillData>
{
    public static readonly string fileName = "xml/skill";

    public static SkillData GetSkillDataByID(int id)
    {
        if (!SkillData.dataMap.ContainsKey(id))
            return null;

        SkillData skillData = SkillData.dataMap[id];

        return skillData;
    }
    /// <summary>
    /// 技能的名字
    /// </summary>
    public string name {get; protected set;}
    /// <summary>
    /// 如果是对敌伤害，那么就是伤害值
    /// 如果是对于自己人的加血，那么就是加血量
    /// </summary>
    public int power { get; protected set; }
    /// <summary>
    /// 技能的暴击率
    /// </summary>
    public int critrate { get; protected set; }

    /// <summary>
    /// 技能的描述
    /// </summary>
    public string description { get; protected set; }

    /// <summary>
    /// 技能在此位置上可施展，值为：1， 2， 3， 4
    /// </summary>
    public List<int> positions { get; protected set; }

    /// <summary>
    /// 技能影响或者伤害的位置，值为：1、2、3、4、5、6、7、8
    /// </summary>
    public List<int> affectpositions { get; protected set; }
    /// <summary>
    /// 初始技能时的权重
    /// </summary>
    public int weight { get; protected set; }

    public int skillType { get; protected set; }
}

public enum AbilityType
{
    None = 0,
    NormalAttack = 1,//
    Poison,
    Buffer,
    Debuffer,
    Blood,
    Pressure,
}

public class AbilityBase
{
    public RoleBase Parent;
    /// <summary>
    /// 技能的等级，等级0表示此技能还是锁住的
    /// </summary>
    public int Level;

    /// <summary>
    /// 第几个技能
    /// </summary>
    public int Index;

    public SkillData SkillData;

    private bool m_inUse = false;
    public bool InUse
    {
        get
        {
            return Parent.m_roleInfo.CheckSkillInUse(Index);
        }
    }

    public AbilityBase(int skillID, int idx, RoleBase parent)
    {
        SkillData = SkillData.GetSkillDataByID(skillID);
        Index = idx;
        Parent = parent;
    }

    public AbilityType AbilityType
    {
        get
        {
            return (AbilityType)SkillData.skillType;
        }
    }

    public void onSelect()
    {
        RoleManager.Instance.SelectedHero.CurrentAbility = this;
        foreach (KeyValuePair<int, RoleBase> kvRole in RoleManager.Instance.RoleInBattleDic)
        {
            if (this.SkillData.affectpositions.Contains(kvRole.Value.m_playerPosition))
            {
                if (this.SkillData.affectpositions.Contains(Parent.m_playerPosition))
                {
                    //对队友有影响
                    kvRole.Value.OverlayItemModel.AffectHelp = true;
                }
                else
                {
                    //对敌人有影响
                    kvRole.Value.OverlayItemModel.AffectAttack = true;
                }
            }
            else
            {
                kvRole.Value.OverlayItemModel.AffectAttack = false;
                kvRole.Value.OverlayItemModel.AffectHelp = false;
            }
        }
    }

    /// <summary>
    /// 执行技能
    /// </summary>
    public virtual void Perform()
    {
        GameData.Instance.BattleSceneActionFlag.RemoveFlag((long)StateDef.BattleActionFlag.OnAttacking);
        RoleManager.Instance.ClearAttackAndAffectFlags();
    }

    public virtual bool IsValid()
    {
        if (SkillData.positions.Contains(Parent.m_playerPosition))
        {
            foreach(RoleBase role in RoleManager.Instance.RolesInBattle())
            {
                if (SkillData.affectpositions.Contains(role.m_playerPosition))
                    return true;
            }

            return false;
        }

        return false;
    }

    public bool ChangeInUse(bool inUse)
    {
        m_inUse = inUse;
        Parent.UpdateSkillInUseList(Index, inUse);
        return m_inUse;
    }


}
