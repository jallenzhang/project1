using UnityEngine;
using System.Collections;

//	ActionAttack.cs
//	Author: Lu Zexi
//	2014-10-23


namespace Game.AIBehaviorTree
{
	//attack action
	public class ActionAttack : BNodeAction
	{
		private bool m_over;
		private float m_ftime;
        private float m_duration = 1f;

		public ActionAttack()
			:base()
		{
			this.m_strName = "AttackAction";
		}

		public override void OnEnter (BInput input)
		{
            //RoleInput tinput = input as RoleInput;
            //Animation playerAnim = tinput.Parent.RoleObject.GetComponent<Animation>();
            //playerAnim[StateDef.PlayerAnimationClipName.OrdinaryAttack1R].time = 0;
            //playerAnim.Play(StateDef.PlayerAnimationClipName.OrdinaryAttack1R);
            //m_duration = playerAnim[StateDef.PlayerAnimationClipName.OrdinaryAttack1R].length;
            this.m_ftime = Time.time;
            this.m_over = false;
			Debug.Log("attack");
		}

		//excute
		public override ActionResult Excute (BInput input)
		{
            if (Time.time - this.m_ftime > m_duration)
                this.m_over = true;
            if (this.m_over)
            {
                Debug.logger.Log("Time.time - this.m_ftime " + (Time.time - this.m_ftime));
                return ActionResult.SUCCESS;
            }

            if (GameData.Instance.BattleSceneActionFlag.HasFlag((long)StateDef.BattleActionFlag.OnAttacking))
			    return ActionResult.RUNNING;

            return ActionResult.SUCCESS;
		}

        public override void OnExit(BInput input)
        {
            this.m_over = false;
            DarkBattleTimer.Instance.IsRunning = true;
            base.OnExit(input);
        }
	}

}