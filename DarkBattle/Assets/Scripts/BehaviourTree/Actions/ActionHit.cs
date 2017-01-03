using UnityEngine;
using System.Collections;
using Game.AIBehaviorTree;

namespace Game.AIBehaviorTree
{
    public class ActionHit : BNodeAction
    {
        private bool m_over;
        private float m_ftime = 0;
        private float m_duration = 1;
        public ActionHit()
            : base()
        {
            this.m_strName = "HitAction";
        }

        public override void OnEnter(BInput input)
        {
        }

        //excute
        public override ActionResult Excute(BInput input)
        {
            RoleInput tinput = input as RoleInput;
            
            if (tinput.Parent.RoleActionFlag.HasFlag((long)StateDef.PlayerActionFlag.Hit))
            {
                tinput.Parent.RoleActionFlag.RemoveFlag((long)StateDef.PlayerActionFlag.Hit);
                Animation playerAnim = tinput.Parent.RoleObject.GetComponent<Animation>();
                playerAnim[StateDef.PlayerAnimationClipName.Hit1R].time = 0;
                playerAnim[StateDef.PlayerAnimationClipName.Hit1R].wrapMode = WrapMode.Once;
                playerAnim.Play(StateDef.PlayerAnimationClipName.Hit1R);
                m_ftime = Time.time;
            }

            if (m_ftime != 0 && Time.time - this.m_ftime > m_duration)
            {
                this.m_over = true;
                return ActionResult.SUCCESS;
            }

            return ActionResult.RUNNING;
        }
    }
}

