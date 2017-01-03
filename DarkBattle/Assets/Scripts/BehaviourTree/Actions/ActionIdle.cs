using UnityEngine;
using System.Collections;
using Game.AIBehaviorTree;

namespace Game.AIBehaviorTree
{
    public class ActionIdle : BNodeAction
    {
        public ActionIdle() : base()
        {
            this.m_strName = "IdleAction";
        }

        public override void OnEnter(BInput input)
        {
            RoleInput tinput = input as RoleInput;
            Animation playerAnim = tinput.Parent.RoleObject.GetComponent<Animation>();
            playerAnim[StateDef.PlayerAnimationClipName.IdleR].time = 0;
            playerAnim[StateDef.PlayerAnimationClipName.IdleR].wrapMode = WrapMode.Loop;
            playerAnim.Play(StateDef.PlayerAnimationClipName.IdleR);
            
        }

        public override ActionResult Excute(BInput input)
        {
            RoleInput tinput = input as RoleInput;
            if (GameData.Instance.BattleSceneActionFlag.HasFlag((long)StateDef.BattleActionFlag.InFighting))
                return ActionResult.FAILURE;

            if (tinput.Parent.RoleActionFlag.HasFlag((long)StateDef.PlayerActionFlag.Idle))
            {
                return ActionResult.RUNNING;
            }

            return ActionResult.SUCCESS;

        }
    }
}
