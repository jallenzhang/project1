using UnityEngine;
using System.Collections;
using Game.AIBehaviorTree;

namespace Game.AIBehaviorTree
{
    public class ConditionChoose : BNodeCondition
    {
        public ConditionChoose():base()
        {
            this.m_strName = "ConditionChoose";
        }

        public override void OnEnter(BInput input)
        {
            base.OnEnter(input);
        }

        public override ActionResult Excute(BInput input)
        {
            if (GameData.Instance.BattleSceneActionFlag.HasFlag((long)StateDef.BattleActionFlag.OnChoosingSkill))
            {
                return ActionResult.SUCCESS;
            }

            return ActionResult.FAILURE;
        }
    }
}

