using UnityEngine;
using System.Collections;

namespace Game.AIBehaviorTree
{
    public class ConditionAttack : BNodeCondition
    {
        public ConditionAttack()
            : base()
        {
            this.m_strName = "ConditionAttack";
        }

        public override void OnEnter(BInput input)
        {
            base.OnEnter(input);
        }

        public override ActionResult Excute(BInput input)
        {
            if (GameData.Instance.BattleSceneActionFlag.HasFlag((long)StateDef.BattleActionFlag.OnAttacking))
            {
                return ActionResult.SUCCESS;
            }

            return ActionResult.FAILURE;
        }
    }
}

