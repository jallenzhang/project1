using UnityEngine;
using System.Collections;
using Game.AIBehaviorTree;

namespace Game.AIBehaviorTree
{
    public class ConditionIdle : BNodeCondition
    {
        public ConditionIdle() : base()
        {
            m_strName = "ConditionIdle";
        }

        public override void OnEnter(BInput input)
        {
            base.OnEnter(input);
        }

        public override ActionResult Excute(BInput input)
        {
            RoleInput tinput = input as RoleInput;
            if (tinput.Parent.RoleActionFlag.HasFlag((long)StateDef.PlayerActionFlag.Idle))
            {
                return ActionResult.SUCCESS;
            }

            return ActionResult.FAILURE;
        }
    }
}

