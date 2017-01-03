using UnityEngine;
using System.Collections;
using Game.AIBehaviorTree;

namespace Game.AIBehaviorTree
{
    public class ConditionAddHP : BNodeCondition
    {
        public ConditionAddHP():base()
        {
            this.m_strName = "ConditionAddHP";
        }

        public override void OnEnter(BInput input)
        {
            RoleInput tinput = input as RoleInput;
            Debug.logger.Log(tinput.Parent.RoleObject.name + " ConditionAddHP");
        }

        public override ActionResult Excute(BInput input)
        {
            RoleInput tinput = input as RoleInput;
            if (tinput.Parent.RoleActionFlag.HasFlag((long)StateDef.PlayerActionFlag.AddHP))
            {
                return ActionResult.SUCCESS;
            }

            return ActionResult.FAILURE;
        }
    }
}
