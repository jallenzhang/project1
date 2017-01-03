using UnityEngine;
using System.Collections;
using Game.AIBehaviorTree;

namespace Game.AIBehaviorTree
{
    public class ConditionWalkForward : BNodeCondition
    {
        public ConditionWalkForward():base()
        {
            this.m_strName = "ConditionWalkForward";
        }

        public override void OnEnter(BInput input)
        {
            
        }

        public override ActionResult Excute(BInput input)
        {
            RoleInput pInput = (RoleInput)input;
            if (GameData.Instance.BattleSceneActionFlag.HasFlag((long)StateDef.BattleActionFlag.InFighting))
                return ActionResult.FAILURE;

            if (pInput.Parent.RoleActionFlag.HasFlag((long)StateDef.PlayerActionFlag.WalkForward))
            {
                return ActionResult.SUCCESS;
            }

            return ActionResult.FAILURE;
        }
    }
}
