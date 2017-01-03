using UnityEngine;
using System.Collections;
using Game.AIBehaviorTree;

namespace Game.AIBehaviorTree
{
    public class ConditionNoFighting : BNodeCondition
    {
        public ConditionNoFighting():base()
        {
            this.m_strName = "ConditionNoFighting";
        }

        public override void OnEnter(BInput input)
        {
            base.OnEnter(input);
        }

        public override ActionResult Excute(BInput input)
        {
            if (!GameData.Instance.BattleSceneActionFlag.HasFlag((long)StateDef.BattleActionFlag.InFighting))
            {
                return ActionResult.SUCCESS;
            }

            return ActionResult.FAILURE;
        }
    }

}