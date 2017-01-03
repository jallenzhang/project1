using UnityEngine;
using System.Collections;
using Game.AIBehaviorTree;

namespace Game.AIBehaviorTree
{
    public class ConditionHit : BNodeCondition
    {
        public ConditionHit()
			:base()
		{
			this.m_strName = "ConditionHit";
		}

		public override void OnEnter (BInput input)
		{
            RoleInput tinput = input as RoleInput;
            Debug.logger.Log(tinput.Parent.RoleObject.name + " tinput.Parent.RoleActionFlag.HasFlag((long)StateDef.PlayerActionFlag.Hit) " + ((RoleInput)input).Parent.RoleActionFlag.HasFlag((long)StateDef.PlayerActionFlag.Hit));
		}

		public override ActionResult Excute (BInput input)
		{
            RoleInput tinput = input as RoleInput;
            if (tinput.Parent.RoleActionFlag.HasFlag((long)StateDef.PlayerActionFlag.Hit))
            {
                Debug.logger.Log("????????????????????");
                //tinput.Parent.RoleActionFlag.RemoveFlag((long)StateDef.PlayerActionFlag.Hit);
                return ActionResult.SUCCESS;
            }

            return ActionResult.RUNNING;
		}
    }
}
