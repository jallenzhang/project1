using UnityEngine;
using System.Collections;

public class NunSkill4 : AbilityBase
{
    public NunSkill4(int level, int skillId, int idx, RoleBase parent)
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

    public override bool IsValid()
    {
        return base.IsValid();
    }

    public override void Perform()
    {
        Animation playerAnim = Parent.RoleObject.GetComponent<Animation>();
        playerAnim[StateDef.PlayerAnimationClipName.OrdinaryAttack1R].time = 0;

        playerAnim.Play(StateDef.PlayerAnimationClipName.OrdinaryAttack1R);
        //m_duration = playerAnim[StateDef.PlayerAnimationClipName.OrdinaryAttack1R].length;
        CoroutineAgent.DelayOperation(1f, base.Perform);
    }
}
