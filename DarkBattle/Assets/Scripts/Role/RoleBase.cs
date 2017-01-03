using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RoleBase : IComparable<RoleBase>
{
    /// <summary>
    /// 人物对象
    /// </summary>
    public GameObject RoleObject;
    /// <summary>
    /// 人物属性对象
    /// </summary>
    public RoleInput m_input;
    /// <summary>
    /// 人物行为标识
    /// </summary>
    public CommonFlag RoleActionFlag = new CommonFlag();

    /// <summary>
    /// position is like: 1, 2, 3, 4, 5, 6, 7, 8
    /// </summary>
    public int m_playerPosition;

    public CommonDefine.RoleType m_roleType;

    /// <summary>
    /// 人物的技能列表
    /// </summary>
    public List<AbilityBase> m_abilities = new List<AbilityBase>();

    private AbilityBase m_currentAbility = null;
    private SimpleState m_idleState = new SimpleState();
    private SimpleState m_bloodState = new SimpleState();
    private SimpleState m_bufferState = new SimpleState();
    private SimpleState m_debufferState = new SimpleState();
    private SimpleState m_normalHitState = new SimpleState();
    private SimpleState m_posionState = new SimpleState();
    private SimpleState m_pressureState = new SimpleState();
    private SimpleStateMachine m_stateMachine;
    public AbilityBase CurrentAbility
    {
        get;
        set;
    }

    private OverlayItemModel m_overlayItemModel = null;
    public OverlayItemModel OverlayItemModel
    {
        get
        {
            return m_overlayItemModel;
        }
    }


    public int m_heroId
    {
        get;
        private set;
    }

    /// <summary>
    /// data from net or local db
    /// </summary>
    public RoleInfo m_roleInfo = null;

    /// <summary>
    /// data from local configure
    /// </summary>
    protected RoleData m_roleData = null;

    private OverlayItemView m_overlayItemView = null;

    public RoleBase(RoleInfo roleInfo)
    {
        InitRoleInfo(roleInfo);
    }

    public void InitRoleInfo(RoleInfo roleInfo)
    {
        m_heroId = roleInfo.id;
        m_roleInfo = roleInfo;
        m_roleData = RoleData.GetRoleDataByID((int)roleInfo.type);
        m_roleType = roleInfo.type;
        m_input = new RoleInput(this);
        m_input.Level = roleInfo.Level;
        //TODO:should calc by level
        m_input.HP = m_roleData.hp;
        m_input.Agility = m_roleData.agility;
        InitialAbilities();
        m_overlayItemModel = new OverlayItemModel();
        InitStates();
    }

    public int CompareTo(RoleBase rb)
    {
        if (rb == null)
            return 1;

        return m_playerPosition - rb.m_playerPosition;
    }

    private void InitStates()
    {
        m_stateMachine = new SimpleStateMachine();

        this.m_idleState.onEnter = () =>
        {
            Animation playerAnim = RoleObject.GetComponent<Animation>();
            playerAnim[StateDef.PlayerAnimationClipName.IdleR].time = 0;
            playerAnim[StateDef.PlayerAnimationClipName.IdleR].wrapMode = WrapMode.Loop;
            playerAnim.Play(StateDef.PlayerAnimationClipName.IdleR);
        };

        this.m_bufferState.onEnter = () =>
        {
            m_overlayItemModel.IsBuff = true;
            Animation playerAnim = RoleObject.GetComponent<Animation>();
            playerAnim[StateDef.PlayerAnimationClipName.ChargeStartR].time = 0;
            playerAnim[StateDef.PlayerAnimationClipName.ChargeStartR].wrapMode = WrapMode.Once;
            playerAnim.Play(StateDef.PlayerAnimationClipName.ChargeStartR);
            CoroutineAgent.DelayOperation(playerAnim[StateDef.PlayerAnimationClipName.ChargeStartR].length, () => { m_stateMachine.State = m_idleState; });
        };

        this.m_debufferState.onEnter = () =>
        {
            m_overlayItemModel.IsDebuff = true;
            Animation playerAnim = RoleObject.GetComponent<Animation>();
            playerAnim[StateDef.PlayerAnimationClipName.WinR].time = 0;
            playerAnim[StateDef.PlayerAnimationClipName.WinR].wrapMode = WrapMode.Once;
            playerAnim.Play(StateDef.PlayerAnimationClipName.WinR);
            CoroutineAgent.DelayOperation(playerAnim[StateDef.PlayerAnimationClipName.WinR].length, () => { m_stateMachine.State = m_idleState; });
        };

        this.m_bloodState.onEnter = () =>
        {
            m_overlayItemModel.IsBlooding = true;
            Animation playerAnim = RoleObject.GetComponent<Animation>();
            playerAnim[StateDef.PlayerAnimationClipName.HitBellyR].time = 0;
            playerAnim[StateDef.PlayerAnimationClipName.HitBellyR].wrapMode = WrapMode.Once;
            playerAnim.Play(StateDef.PlayerAnimationClipName.HitBellyR);
            CoroutineAgent.DelayOperation(playerAnim[StateDef.PlayerAnimationClipName.HitBellyR].length, () => { m_stateMachine.State = m_idleState; });
        };

        this.m_normalHitState.onEnter = () =>
        {
            Animation playerAnim = RoleObject.GetComponent<Animation>();
            playerAnim[StateDef.PlayerAnimationClipName.Hit1R].time = 0;
            playerAnim[StateDef.PlayerAnimationClipName.Hit1R].wrapMode = WrapMode.Once;
            playerAnim.Play(StateDef.PlayerAnimationClipName.Hit1R);
            CoroutineAgent.DelayOperation(playerAnim[StateDef.PlayerAnimationClipName.Hit1R].length, () => { m_stateMachine.State = m_idleState; });
        };

        this.m_posionState.onEnter = () =>
        {
            m_overlayItemModel.IsPoison = true;
            Animation playerAnim = RoleObject.GetComponent<Animation>();
            playerAnim[StateDef.PlayerAnimationClipName.Hit2R].time = 0;
            playerAnim[StateDef.PlayerAnimationClipName.Hit2R].wrapMode = WrapMode.Once;
            playerAnim.Play(StateDef.PlayerAnimationClipName.Hit2R);
            CoroutineAgent.DelayOperation(playerAnim[StateDef.PlayerAnimationClipName.Hit2R].length, () => { m_stateMachine.State = m_idleState; });
        };

        this.m_pressureState.onEnter = () =>
        {
            m_overlayItemModel.Pressure += 1;
            Animation playerAnim = RoleObject.GetComponent<Animation>();
            playerAnim[StateDef.PlayerAnimationClipName.HitHardR].time = 0;
            playerAnim[StateDef.PlayerAnimationClipName.HitHardR].wrapMode = WrapMode.Once;
            playerAnim.Play(StateDef.PlayerAnimationClipName.HitHardR);
            CoroutineAgent.DelayOperation(playerAnim[StateDef.PlayerAnimationClipName.HitHardR].length, () => { m_stateMachine.State = m_idleState; });
        };
    }

    protected virtual void InitialAbilities()
    {
        
    }

    protected virtual bool ChooseSkill()
    {
        return true;
    }

    public virtual void ToAttackPerson()
    {

    }

    public bool IsHero()
    {
        return m_roleType > CommonDefine.RoleType.Hero && m_roleType < CommonDefine.RoleType.Enemy;
    }

    public bool IsEnemy()
    {
        return m_roleType > CommonDefine.RoleType.Enemy;
    }

    public void onUnSelect()
    {
        OverlayItemModel.IsSelected = false;
    }

    public void onSelect()
    {
        OverlayItemModel.IsSelected = true;
    }

    public void UpdateSkillInUseList(int idx, bool inUse)
    {
        m_roleInfo.UpdateSkillInUseList(idx, inUse);
    }

    public void InitOverlayItem(GameObject parent)
    {
        if (m_overlayItemView == null)
        {
            GameObject obj = NGUITools.AddChild(parent, (GameObject)ResMgr.Instance.LoadAssetFromResource("Prefabs/UI/Overlay/overlayItem"));
            m_overlayItemView = obj.GetComponent<OverlayItemView>();
            m_overlayItemView.Init(m_overlayItemModel);
        }
    }

    public void UpdateOverlayItemPosition(OverlayView overlayView)
    {
        if (m_overlayItemView != null)
        {
            m_overlayItemView.gameObject.transform.position = ProjectHelper.WorldCameraToNGUIPos(overlayView.worldCamera, overlayView.NGUICamera, RoleObject.transform.position);//parent.position);
        }
    }

    /// <summary>
    /// 被攻击，中毒等的动画播放
    /// </summary>
    public void PlayOnBeSkilled(AbilityBase ability)
    {
        switch(ability.AbilityType)
        {
            case AbilityType.Blood:
                m_stateMachine.State = m_bloodState;
                break;
            case AbilityType.Buffer:
                m_stateMachine.State = m_bufferState;
                break;
            case AbilityType.Debuffer:
                m_stateMachine.State = m_debufferState;
                break;
            case AbilityType.NormalAttack:
                m_stateMachine.State = m_normalHitState;
                break;
            case AbilityType.Poison:
                m_stateMachine.State = m_posionState;
                break;
            case AbilityType.Pressure:
                m_stateMachine.State = m_pressureState;
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ability"></param>
    /// <param name="attacker"></param>
    public void OnBeAffected(AbilityBase ability, RoleBase attacker)
    {
        float power = ability.SkillData.power * attacker.m_input.Level;
        m_overlayItemModel.HP -= (power / (float)m_input.HP);
    }
}
