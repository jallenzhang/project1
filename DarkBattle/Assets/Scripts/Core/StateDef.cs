using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateDef {

    #region 动画名称
    public class PlayerAnimationClipName
    {
        /// <summary>
        /// 待机
        /// </summary>
        public const string IdleR = "IdleR";

        /// <summary>
        /// 胜利
        /// </summary>
        public const string WinR = "WinR";

        /// <summary>
        /// 开场挑衅
        /// </summary>
        public const string ProvocationR = "ProvocationR";

        /// <summary>
        /// 走路前进
        /// </summary>
        public const string WalkR = "WalkR";

        /// <summary>
        /// 走路后退
        /// </summary>
        public const string WalkBackR = "WalkBackR";

        /// <summary>
        /// 高速前进
        /// </summary>
        public const string QuickRunningForwardR = "QuickRunningR";

        /// <summary>
        /// 高速后退
        /// </summary>
        public const string QuickRunningBackR = "QuickRunningBackR";

        /// <summary>
        /// 跳跃
        /// </summary>
        public const string JumpR = "JumpR";

        /// <summary>
        /// 普攻1
        /// </summary>
        public const string OrdinaryAttack1R = "Attack1R";

        /// <summary>
        /// 普攻1
        /// </summary>
        public const string OrdinaryAirAttackR = "AirAttackR";
        /// <summary>
        /// 普攻2
        /// </summary>
        public const string OrdinaryAttack2R = "Attack2R";

        /// <summary>
        /// 普攻3
        /// </summary>
        public const string OrdinaryAttack3R = "Attack3R";

        /// <summary>
        /// 普攻4
        /// </summary>
        public const string OrdinaryAttack4R = "Attack4R";

        /// <summary>
        /// 空中攻击动画名称
        /// </summary>
        public const string OrdinaryAttack99R = "Attack99R";

        /// <summary>
        /// 空中攻击动画名称
        /// </summary>
        public const string OrdinaryAttack99LoopR = "Attack99LoopR";

        /// <summary>
        /// 下蹲打 1
        /// </summary>
        public const string AttackSquat1R = "AttackSquat1R";

        /// <summary>
        /// 下蹲打 2
        /// </summary>
        public const string AttackSquat2R = "AttackSquat2R";

        /// <summary>
        /// 空中循环
        /// </summary>
        public const string JumpLoopR = "JumpLoopR";

        /// <summary>
        /// 被动攻击1
        /// </summary>
        public const string PassiveAttack1R = "PassiveAttack1R";

        /// <summary>
        /// 被动攻击2
        /// </summary>
        public const string PassiveAttack2R = "PassiveAttack2R";

        /// <summary>
        /// 被动攻击3
        /// </summary>
        public const string PassiveAttack3R = "PassiveAttack3R";

        /// <summary>
        /// 挨打1
        /// </summary>
        public const string Hit1R = "Hit1R";

        /// <summary>
        /// 挨打2
        /// </summary>
        public const string Hit2R = "Hit2R";

        /// <summary>
        /// 死了 空中攻击
        /// </summary>
        public const string Hit3R = "Hit3R";

        /// <summary>
        /// 击飞动作
        /// </summary>
        public const string Hit4R = "Hit4R";

        /// <summary>
        /// 腹部受击
        /// </summary>
        public const string HitBellyR = "HitBellyR";

        /// <summary>
        /// 重受击
        /// </summary>
        public const string HitHardR = "HitHardR";

        /// <summary>
        /// 小击飞
        /// </summary>
        public const string HitbackStartR = "HitbackStartR";
        public const string HitbackLoopR = "HitbackLoopR";
        public const string HitbackEndR = "HitbackEndR";

        /// <summary>
        /// 旋转击飞
        /// </summary>
        public const string HitSpinStartR = "HitSpinStartR";
        public const string HitSpinLoopR = "HitSpinLoopR";
        public const string HitSpinEndR = "HitSpinEndR";

        /// <summary>
        /// 碰墙
        /// </summary>
        public const string BounceR = "BounceR";

        /// <summary>
        /// 没死 空中攻击
        /// </summary>
        public const string AirRollR = "AirRollR";

        /// <summary>
        /// 没死 空中攻击 循环
        /// </summary>
        public const string AirRollLoopR = "AirRollLoopR";

        /// <summary>
        /// 空中受击循环
        /// </summary>
        public const string Hit3LoopR = "Hit3LoopR";

        /// <summary>
        /// 击飞循环
        /// </summary>
        public const string Hit4LoopR = "Hit4LoopR";

        /// <summary>
        /// 大招吟唱
        /// </summary>
        public const string ChantR = "ChantR";

        /// <summary>
        /// 转身
        /// </summary>
        public const string ZhuanShenR = "ZhuanShenR";

        /// <summary>
        /// 格挡
        /// </summary>
        public const string BlockR = "BlockR";

        /// <summary>
        /// 格挡
        /// </summary>
        public const string SquatBlockR = "SquatBlockR";

        /// <summary>
        /// 技能1
        /// </summary>
        public const string Skill1R = "Skill1R";
        /// <summary>
        /// 技能2
        /// </summary>
        public const string Skill2R = "Skill2R";
        /// <summary>
        /// 技能3
        /// </summary>
        public const string Skill3R = "Skill3R";


        /// <summary>
        /// 击倒
        /// </summary>
        public const string LieDownR = "LieDownR";

        /// <summary>
        /// 击倒
        /// </summary>
        public const string AirHitIdleR = "AirHitIdleR";

        /// <summary>
        /// 击退
        /// </summary>
        public const string HitBackR = "HitBackR";

        /// <summary>
        /// 浮空
        /// </summary>
        public const string HoverR = "HoverR";

        /// <summary>
        /// 颤抖
        /// </summary>
        public const string ShakeR = "ShakeR";

        /// <summary>
        /// 后跳
        /// </summary>
        public const string JumpBackR = "JumpBackR";

        /// <summary>
        /// 下蹲动画名称
        /// </summary>
        public const string SquatR = "SquatR";

        /// <summary>
        /// 下蹲动画名称
        /// </summary>
        public const string SquatLoopR = "SquatLoopR";

        /// <summary>
        /// 下蹲受击
        /// </summary>
        public const string SquatHitR = "SquatHitR";

        /// <summary>
        /// 蓄力循环
        /// </summary>
        public const string ChargeLoopR = "ChargeLoopR";

        /// <summary>
        /// 蓄力开始
        /// </summary>
        public const string ChargeStartR = "ChargeStartR";

        /// <summary>
        /// 蓄力攻击
        /// </summary>
        public const string ChargeAttackR = "ChargeAttackR";
    }
    #endregion

    /// <summary>
    /// 玩家行为标识
    /// </summary>
    public enum PlayerActionFlag : long
    {
        None = 1 << 0,
        /// <summary>
        /// 空闲站立
        /// </summary>
        Idle = 1 << 1,
        /// <summary>
        /// 向前走
        /// </summary>
        WalkForward = 1 << 2,
        /// <summary>
        /// 往后走
        /// </summary>
        WalkBackward = 1 << 3,
        /// <summary>
        /// 开东西，捡东西，解除陷阱等
        /// </summary>
        Pick = 1 << 4,
        /// <summary>
        /// 等待攻击
        /// </summary>
        Waiting2Attack = 1 << 5,
        /// <summary>
        /// 技能1
        /// </summary>
        Skill1 = 1 << 6,
        /// <summary>
        /// 技能2
        /// </summary>
        Skill2 = 1 << 7,
        /// <summary>
        /// 技能3
        /// </summary>
        Skill3 = 1 << 8,
        /// <summary>
        /// 技能4
        /// </summary>
        Skill4 = 1 << 9,
        /// <summary>
        /// 技能5
        /// </summary>
        Skill5 = 1 << 10,
        /// <summary>
        /// 技能6
        /// </summary>
        Skill6 = 1 << 11,
        /// <summary>
        /// 技能7
        /// </summary>
        Skill7 = 1 << 12,
        /// <summary>
        /// 技能8
        /// </summary>
        Skill8 = 1 << 13,
        /// <summary>
        /// 被攻击
        /// </summary>
        Hit = 1 << 14,
        /// <summary>
        /// 流血状态
        /// </summary>
        Blooding = 1 << 15,
        /// <summary>
        /// 中毒状态
        /// </summary>
        Poisoning  =  1 << 16,
        /// <summary>
        /// 闪避，抵抗等行为
        /// </summary>
        Dodge = 1 << 17,
        /// <summary>
        /// 眩晕状态
        /// </summary>
        Faint = 1 << 18,
        /// <summary>
        /// 中陷阱
        /// </summary>
        Trap = 1 << 19,
        /// <summary>
        /// 自虐
        /// </summary>
        Autosadism = 1 << 20,
        /// <summary>
        /// 专注
        /// </summary>
        Dedicated = 1 << 21,
        /// <summary>
        /// 效果增益（让自己的敏捷加成等....）
        /// </summary>
        StatusPlus = 1 << 22,
        /// <summary>
        /// 效果减少（让自己的敏捷减效果....)
        /// </summary>
        StatusMinus = 1 << 23,
        /// <summary>
        /// 被标记
        /// </summary>
        BeSign = 1 << 24,
        /// <summary>
        /// 加血
        /// </summary>
        AddHP = 1 << 25,
        /// <summary>
        /// 死亡
        /// </summary>
        Dead = 1 << 31,
    }

    public enum BattleActionFlag : long
    {
        None = 1 << 0,
        InFighting = 1 << 1,
        OnExchanging = 1 << 2,
        OnChoosingSkill = 1 << 3,
        OnAttacking =  1 << 4,
    }

    public enum DungeonType
    {
        cove,
        crypts,
    }
}

public class CommonDefine
{
    public static int RoleSkillCount = 7;

    public static string DungeonBackground = "{0}.corridor_bg";
    public static string DungeonMiddle = "{0}.corridor_mid";
    public static string DungeonDoor = "{0}.corridor_door.basic";
    public static string DungeonWall = "{0}.corridor_wall.{1}";
    public static string DungeonRoomBG = "{0}.room_wall.{1}";
    public static string DungeonRoomIcon = "room_{0}";
    public static string DungeonCheckPointIcon = "checkpoint.{0}";
    public static string DungeoncheckPointDoneIcon = "checkpoint.{0}.done";

    public static Dictionary<GoodType, string> GoodNameDic = new Dictionary<GoodType, string>()
    {
        {GoodType.Gold, "inv_gem+citrine"},
        {GoodType.Supply_Antivenom, "inv_supply+antivenom"},
        {GoodType.Supply_Badage, "inv_supply+bandage"},
        {GoodType.Supply_HolyWater, "inv_supply+holy_water"},
        {GoodType.Supply_Shovel, "inv_supply+shovel"},
        {GoodType.Supply_torch, "inv_supply+torch"},
    };

    public static Dictionary<RoleType, string> RoleNameDic = new Dictionary<RoleType, string>()
    {
        {RoleType.Soldier, "YinTianChou"},
        {RoleType.Nun, "XiaoHongChen"},
        {RoleType.YeCha, "YeChaLuo"},
        {RoleType.SunYuan, "SunYuan" },
        {RoleType.GuiMianJunZi, "GuiMianJunZi"}
    };

    public static Dictionary<RoleType, string> RoleAbilityDic = new Dictionary<RoleType, string>()
    {
        {RoleType.Soldier, "abomination.ability.{0}"},
        {RoleType.Nun, "arbalest.ability.{0}"},
        {RoleType.YeCha, "grave_robber.ability.{0}"},
        {RoleType.SunYuan, "hellion.ability.{0}" }
    };
    

    public enum GoodType
    {
        /// <summary>
        /// 无指定
        /// </summary>
        None,
        /// <summary>
        /// 金币
        /// </summary>
        Gold,
        /// <summary>
        /// 宝石
        /// </summary>
        Gem,
        /// <summary>
        /// 食物
        /// </summary>
        Food,
        /// <summary>
        /// 解毒水
        /// </summary>
        Supply_Antivenom,
        /// <summary>
        /// 止血带
        /// </summary>
        Supply_Badage,
        /// <summary>
        /// 进化水，增强效益
        /// </summary>
        Supply_HolyWater,
        /// <summary>
        /// 铲子
        /// </summary>
        Supply_Shovel,
        /// <summary>
        /// 火把
        /// </summary>
        Supply_torch,
    }

    public enum RoleType
    {
        None = 0,
        Hero = 6000,
        Soldier = 6001,
        Nun = 6002,
        YeCha = 6003,
        SunYuan = 6004,

        Enemy = 8000,
        GuiMianJunZi = 8001,
    }

    public enum RoleStyle
    {
        Normal = 1,
        Red,
        Blue,
        //etc
    }

    public enum MoveDirection
    {
        East = 0,
        South,
        West,
        North,
        None,
    }

    public enum DungeonCellType
    {
        Unused,
        Room,
    }

    public enum RoomType
    {
        entrance,
        unknown,
        empty,
        treasure,
        boss,
        battle,
    }

    public enum CheckPointType
    {
        None,
        /// <summary>
        /// 木桶
        /// </summary>
        bucket,
        /// <summary>
        /// 棺木
        /// </summary>
        coffin,
        /// <summary>
        /// 背包
        /// </summary>
        bag,
    }
}
