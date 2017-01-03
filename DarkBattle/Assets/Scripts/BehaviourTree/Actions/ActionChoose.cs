using UnityEngine;
using System.Collections;
using Game.AIBehaviorTree;

namespace Game.AIBehaviorTree
{
    public class ActionChoose : BNodeAction
    {

        public ActionChoose() : base()
        {
            this.m_strName = "ChooseAction";
        }

        public override void OnEnter(BInput input)
        {
            DarkBattleTimer.Instance.IsRunning = false;
            RoleInput tinput = input as RoleInput;
            tinput.Parent.ToAttackPerson();
            Debug.Log("ActionChoose");
        }

        public override ActionResult Excute(BInput input)
        {
            if (GameData.Instance.BattleSceneActionFlag.HasFlag((long)StateDef.BattleActionFlag.OnChoosingSkill))
            {
                return ActionResult.RUNNING;
            }
            else
            {
                return ActionResult.SUCCESS;
            }
        }
    }
}
