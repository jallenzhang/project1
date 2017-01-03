using UnityEngine;
using System.Collections;
using Game.AIBehaviorTree;

namespace Game.AIBehaviorTree
{
    public class ActionAddHP : BNodeAction
    {
        private bool m_over;
        private float m_ftime;
        private float m_duration = 1;
        public ActionAddHP() : base()
        {
            this.m_strName = "AddHPAction";
        }

        public override void OnEnter(BInput input)
        {
            RoleInput tinput = input as RoleInput;
            this.m_ftime = Time.time;
            this.m_over = false;
            Debug.Log("ActionAddHP");
        }

        //excute
        public override ActionResult Excute(BInput input)
        {
            if (Time.time - this.m_ftime > m_duration)
                this.m_over = true;
            if (this.m_over)
            {
                RoleInput tinput = input as RoleInput;
                tinput.Parent.RoleActionFlag.RemoveFlag((long)StateDef.PlayerActionFlag.AddHP);
                return ActionResult.SUCCESS;
            }

            return ActionResult.RUNNING;
        }
    }
}
