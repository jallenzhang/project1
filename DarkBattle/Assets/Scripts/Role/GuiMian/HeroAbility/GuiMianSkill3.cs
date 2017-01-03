using UnityEngine;
using System.Collections;

public class GuiMianSkill3 : AbilityBase
{
    public GuiMianSkill3(int level, int skillId, int idx, RoleBase parent)
        : base(skillId, idx, parent)
    {
        Level = level;
        Init();
    }
    /// <summary>
    /// 根据level再进行计算其属性
    /// </summary>
    /// <param name="level"></param>
    private void Init()
    {
        //TODO:
    }

    public override void Perform()
    {
        Debug.logger.Log("GuiMianSkill3 " + this.Level + " power " + this.SkillData.name);
        Animation playerAnim = Parent.RoleObject.GetComponent<Animation>();
        playerAnim[StateDef.PlayerAnimationClipName.OrdinaryAttack1R].time = 0;
        playerAnim.Play(StateDef.PlayerAnimationClipName.OrdinaryAttack1R);
        //m_duration = playerAnim[StateDef.PlayerAnimationClipName.OrdinaryAttack1R].length;
        CoroutineAgent.DelayOperation(playerAnim[StateDef.PlayerAnimationClipName.OrdinaryAttack1R].length, base.Perform);
    }
}
