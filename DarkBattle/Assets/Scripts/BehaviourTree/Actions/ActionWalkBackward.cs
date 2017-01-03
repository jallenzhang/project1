using UnityEngine;
using System.Collections;
using Game.AIBehaviorTree;

namespace Game.AIBehaviorTree
{
    public class ActionWalkBackward : BNodeAction
    {
        public ActionWalkBackward():base()
        {
            this.m_strName = "WalkBackwardAction";
        }

        public override void OnEnter(BInput input)
        {
            RoleInput pInput = (RoleInput)input;
            Animation playerAnim = pInput.Parent.RoleObject.GetComponent<Animation>();
            playerAnim[StateDef.PlayerAnimationClipName.WalkBackR].time = 0;
            playerAnim[StateDef.PlayerAnimationClipName.WalkBackR].wrapMode = WrapMode.Loop;
            playerAnim.Play(StateDef.PlayerAnimationClipName.WalkBackR);
        }

        public override ActionResult Excute(BInput input)
        {
            RoleInput tinput = input as RoleInput;
            if (GameData.Instance.BattleSceneActionFlag.HasFlag((long)StateDef.BattleActionFlag.InFighting))
                return ActionResult.FAILURE;

            if (tinput.Parent.RoleActionFlag.HasFlag((long)StateDef.PlayerActionFlag.WalkBackward))
            {
                return ActionResult.RUNNING;
            }

            return ActionResult.SUCCESS;
        }
    }
}
