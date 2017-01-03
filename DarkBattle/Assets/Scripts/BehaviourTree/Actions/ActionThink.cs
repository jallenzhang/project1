using UnityEngine;
using System.Collections;
using Game.AIBehaviorTree;

namespace Game.AIBehaviorTree
{
    public class ActionThink : BNodeAction
    {
        float m_fTime = 0;
        bool m_over = false;
        private float duration = 0;
        public ActionThink()
            : base()
        {
            this.m_strName = "ThinkAction";
        }

        public override void OnEnter(BInput input)
        {
            RoleInput tinput = input as RoleInput;
            Animation playerAnim = tinput.Parent.RoleObject.GetComponent<Animation>();
            playerAnim[StateDef.PlayerAnimationClipName.IdleR].time = 0;
            playerAnim[StateDef.PlayerAnimationClipName.IdleR].wrapMode = WrapMode.Loop;
            playerAnim.Play(StateDef.PlayerAnimationClipName.IdleR);
            m_fTime = DarkBattleTimer.Instance.PastedTime;
            m_over = false;
            duration = tinput.Agility;
        }

        public override ActionResult Excute(BInput input)
        {
            if (DarkBattleTimer.Instance.PastedTime - m_fTime > duration
                && !GameData.Instance.BattleSceneActionFlag.HasFlag((long)StateDef.BattleActionFlag.OnChoosingSkill)
                && DarkBattleTimer.Instance.IsRunning)
                this.m_over = true;

            if (!GameData.Instance.BattleSceneActionFlag.HasFlag((long)StateDef.BattleActionFlag.InFighting))
                return ActionResult.FAILURE;

            if (this.m_over == true && DarkBattleTimer.Instance.IsRunning)
            {
                RoleManager.Instance.SelectedHero = ((RoleInput)input).Parent;//.RoleObject.GetComponent<Hero>();
                //DarkBattleTimer.Instance.IsRunning = false;
                GameData.Instance.BattleSceneActionFlag.AddFlag((long)StateDef.BattleActionFlag.OnChoosingSkill);
                return ActionResult.SUCCESS;
            }

            return ActionResult.RUNNING;
        }
    }
}

