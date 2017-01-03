using UnityEngine;
using System.Collections;
using Game.AIBehaviorTree;

namespace Game.AIBehaviorTree
{
    public class ConditionWalkBackward : BNodeCondition
    {
        public ConditionWalkBackward():base()
        {
            this.m_strName = "ConditionWalkBackward";
        }

        public override void OnEnter(BInput input)
        {
            base.OnEnter(input);
        }

        public override ActionResult Excute(BInput input)
        {
            RoleInput pInput = (RoleInput)input;
            if (GameData.Instance.BattleSceneActionFlag.HasFlag((long)StateDef.BattleActionFlag.InFighting))
                return ActionResult.FAILURE;

            if (pInput.Parent.RoleActionFlag.HasFlag((long)StateDef.PlayerActionFlag.WalkBackward))
            {
                return ActionResult.SUCCESS;
            }

            return ActionResult.FAILURE;
        }
    }
}
