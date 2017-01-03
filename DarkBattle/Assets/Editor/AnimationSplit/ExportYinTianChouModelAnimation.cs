using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

class ExportYinTianChouModelAnimation : ExportModelAnimation
{
    [MenuItem("Assets/Export Model Animation/导出尹天仇/技能1特效")]
    static void ExportYuLuoChaSkillEffect()
    {
        if (Selection.objects != null && Selection.objects.Length > 0)
        {
            foreach (UnityEngine.Object o in Selection.objects)
            {
                AnimationClip clip = o as AnimationClip;
                switch(clip.name)
                {
                    case "skill01":
                        {
                            AnimationEventManager mgr = new AnimationEventManager(clip);
                            clip.frameRate = 30;

                            mgr.AddAnimationEvent(4 / clip.frameRate, "AnimationEventBoxIn");
                            mgr.AddAnimationEvent(5 / clip.frameRate, "AnimationEventBoxOut");

                            mgr.AddAnimationEvent(7 / clip.frameRate, "AnimationEventBoxIn");
                            mgr.AddAnimationEvent(8 / clip.frameRate, "AnimationEventBoxOut");

                            mgr.AddAnimationEvent(10 / clip.frameRate, "AnimationEventBoxIn");
                            mgr.AddAnimationEvent(11 / clip.frameRate, "AnimationEventBoxOut");

                            mgr.AddAnimationEvent(13 / clip.frameRate, "AnimationEventBoxIn");
                            mgr.AddAnimationEvent(14 / clip.frameRate, "AnimationEventBoxOut");

                            mgr.AddAnimationEvent(16 / clip.frameRate, "AnimationEventBoxIn");
                            mgr.AddAnimationEvent(18 / clip.frameRate, "AnimationEventBoxOut");

                            mgr.AddAnimationEvent(22 / clip.frameRate, "AnimationEventBoxIn");
                            mgr.AddAnimationEvent(23 / clip.frameRate, "AnimationEventBoxOut");

                            mgr.AddAnimationEvent(26 / clip.frameRate, "AnimationEventBoxIn");
                            mgr.AddAnimationEvent(27 / clip.frameRate, "AnimationEventBoxOut");

                            mgr.AddAnimationEvent(31 / clip.frameRate, "AnimationEventBoxIn");

                            mgr.AddAnimationEvent(31 / clip.frameRate, "AnimationEventBoxIn1");//动作的最后一次//

                            mgr.AddAnimationEvent(32 / clip.frameRate, "AnimationEventBoxOut");

                            mgr.AddAnimationEvent(clip.length, "AnimationEventEnd");
                            mgr.SaveAnimationEvent();
                        }
                        break;
                }
            }
        }
    }

    [MenuItem("Assets/Export Model Animation/导出尹天仇/ 动作")]
    static void ExportYuLuoCha()
    {
        ExportYinTianChouModelAnimation a = new ExportYinTianChouModelAnimation();
        a.Export();
    }

    public override void SetAnimationEvents(AnimationClip dstClip)
    {
        AnimationEventManager animationEventManager = new AnimationEventManager(dstClip);
        dstClip.frameRate = 30.0f;
        switch (dstClip.name)
        {
            case "ProvocationR":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
            case "WinR":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
            case "ShakeR":
                {
                    dstClip.wrapMode = WrapMode.Loop;
                }
                break;
            case "DieR":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
            case "IdleR":
                {
                    dstClip.wrapMode = WrapMode.Loop;
                }
                break;
            case "BlockR":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
            case "SquatBlockR":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
            case "HitBackR":
                {
                    animationEventManager.AddAnimationEvent(20 / dstClip.frameRate, "AnimationEventBoxIn3");// 落地烟尘激活 //
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
            case "HoverR":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                    animationEventManager.AddAnimationEvent(30 / dstClip.frameRate, "AnimationEventBoxIn");
                    animationEventManager.AddAnimationEvent(21 / dstClip.frameRate, "AnimationEventBoxIn3");// 落地烟尘激活 //
                    animationEventManager.AddAnimationEvent(45 / dstClip.frameRate, "AnimationEventBoxIn4"); //死亡使用//
                }
                break;
            case "LieDownR":
                {
                    animationEventManager.AddAnimationEvent(26 / dstClip.frameRate, "AnimationEventBoxIn"); // 启动无敌 //
                    animationEventManager.AddAnimationEvent(37 / dstClip.frameRate, "AnimationEventBoxIn3");// 落地烟尘激活 //
                    animationEventManager.AddAnimationEvent(51 / dstClip.frameRate, "AnimationEventBoxIn4"); //死亡使用//
                    animationEventManager.AddAnimationEvent(40 / dstClip.frameRate, "AnimationEventLieDownBegin"); //躺地开始//
                    animationEventManager.AddAnimationEvent(60 / dstClip.frameRate, "AnimationEventLieDownEnd"); //躺地结束//
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
            case "AirHitIdleR":
                {
                    animationEventManager.AddAnimationEvent(0 / dstClip.frameRate, "AnimationEventBoxIn3");// 落地烟尘激活 //
                    animationEventManager.AddAnimationEvent(19 / dstClip.frameRate, "AnimationEventBoxIn4"); //死亡使用//
                    animationEventManager.AddAnimationEvent(5 / dstClip.frameRate, "AnimationEventLieDownBegin"); //躺地开始//
                    animationEventManager.AddAnimationEvent(25 / dstClip.frameRate, "AnimationEventLieDownEnd"); //躺地结束//
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
            case "AirRollR":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
            case "AirRollLoopR":
                {
                    dstClip.wrapMode = WrapMode.Loop;
                }
                break;
            case "ChargeStartR":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
            case "ChargeLoopR":
                {
                    dstClip.wrapMode = WrapMode.Loop;
                }
                break;
            case "ChargeAttackR":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                    animationEventManager.AddAnimationEvent(7 / dstClip.frameRate, "AnimationEventBoxIn1"); // 关闭碰撞体//
                    animationEventManager.AddAnimationEvent(4 / dstClip.frameRate, "AnimationEventBoxIn"); // 激活碰撞体 //
                    animationEventManager.AddAnimationEvent(0 / dstClip.frameRate, "AnimationEventMovePlay");//人物位移
                    animationEventManager.AddAnimationEvent(3 / dstClip.frameRate, "AnimationEventMoveStop");//人物位移停止
                    animationEventManager.AddAnimationEvent(11 / dstClip.frameRate, "AnimationEventCanRemoveCoolDown");//cooldown帧限制//
                }
                break;
            case "JumpBackR":
            case "JumpR":
                {
                    animationEventManager.AddAnimationEvent(3 / dstClip.frameRate, "AnimationEventBoxIn");//触发跳跃//
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
            case "Attack1R":
                {
                    animationEventManager.AddAnimationEvent(2 / dstClip.frameRate, "AnimationEventBoxIn"); //激活碰撞体//
                    animationEventManager.AddAnimationEvent(10 / dstClip.frameRate, "AnimationEventBoxIn1");//关闭碰撞体//
                    animationEventManager.AddAnimationEvent(2 / dstClip.frameRate, "AnimationEventMovePlay");//人物位移
                    animationEventManager.AddAnimationEvent(4 / dstClip.frameRate, "AnimationEventMoveStop");//人物位移停止 
                    animationEventManager.AddAnimationEvent(10 / dstClip.frameRate, "AnimationEventCanRemoveCoolDown");//cooldown帧限制//
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
            case "Attack2R":
                {
                    animationEventManager.AddAnimationEvent(3 / dstClip.frameRate, "AnimationEventBoxIn");//激活碰撞体//
                    animationEventManager.AddAnimationEvent(10 / dstClip.frameRate, "AnimationEventBoxIn1");//关闭碰撞体//
                    animationEventManager.AddAnimationEvent(0 / dstClip.frameRate, "AnimationEventMovePlay");//人物位移
                    animationEventManager.AddAnimationEvent(5 / dstClip.frameRate, "AnimationEventMoveStop");//人物位移停止
                    animationEventManager.AddAnimationEvent(10 / dstClip.frameRate, "AnimationEventCanRemoveCoolDown");//cooldown帧限制//
                    dstClip.wrapMode = WrapMode.ClampForever;
                } 
                break;
            case "Attack3R":
                {
                    animationEventManager.AddAnimationEvent(2 / dstClip.frameRate, "AnimationEventBoxIn");//激活碰撞体//
                    animationEventManager.AddAnimationEvent(10 / dstClip.frameRate, "AnimationEventBoxIn1");//关闭碰撞体//
                    animationEventManager.AddAnimationEvent(0 / dstClip.frameRate, "AnimationEventMovePlay");//人物位移
                    animationEventManager.AddAnimationEvent(7 / dstClip.frameRate, "AnimationEventMoveStop");//人物位移停止
                    animationEventManager.AddAnimationEvent(10 / dstClip.frameRate, "AnimationEventCanRemoveCoolDown");//cooldown帧限制//
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
            case "Attack4R":
                {
                    animationEventManager.AddAnimationEvent(3 / dstClip.frameRate, "AnimationEventBoxIn");//激活碰撞体//
                    animationEventManager.AddAnimationEvent(1 / dstClip.frameRate, "AnimationEventMovePlay");//人物位移
                    animationEventManager.AddAnimationEvent(5 / dstClip.frameRate, "AnimationEventMoveStop");//人物位移停止
                    animationEventManager.AddAnimationEvent(7 / dstClip.frameRate, "AnimationEventCanRemoveCoolDown");//cooldown帧限制//
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
            case "Hit1R":
            case "Hit2R":
            case "Hit3R":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
            case "PassiveAttack3R":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
            case "PassiveAttack2R":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
            case "PassiveAttack1R":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
            case "ChantR":
                {
                    dstClip.wrapMode = WrapMode.Loop;
                }
                break;
            case "WalkR":
                {
                    dstClip.wrapMode = WrapMode.Loop;
                    animationEventManager.AddAnimationEvent(1 / dstClip.frameRate, "AnimationMoveWardEventL");
                    animationEventManager.AddAnimationEvent(9 / dstClip.frameRate, "AnimationMoveWardEventR");
                    animationEventManager.AddAnimationEvent(13 / dstClip.frameRate, "AnimationMoveWardEventL");
                }
                break;
            case "WalkBackR":
                {
                    dstClip.wrapMode = WrapMode.Loop;
                    animationEventManager.AddAnimationEvent(1 / dstClip.frameRate, "AnimationMoveBackWardEventL");
                    animationEventManager.AddAnimationEvent(9 / dstClip.frameRate, "AnimationMoveBackWardEventR");
                    animationEventManager.AddAnimationEvent(15 / dstClip.frameRate, "AnimationMoveBackWardEventL");
                }
                break;
            case "JumpLoopR":
                {
                    dstClip.wrapMode = WrapMode.Loop;
                }
                break;
            case "ZhuanShenL":
            case "ZhuanShenR":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
            case "Skill1R":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                    animationEventManager.AddAnimationEvent(20 / dstClip.frameRate, "AnimationEventBoxIn"); //播放技能特效//
                    animationEventManager.AddAnimationEvent(30 / dstClip.frameRate, "AnimationEventCanRemoveCoolDown");//cooldown帧限制//
                }
                break;
            case "Skill2R":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                    animationEventManager.AddAnimationEvent(13 / dstClip.frameRate, "AnimationEventBoxIn3"); //移动人物//
                    animationEventManager.AddAnimationEvent(10 / dstClip.frameRate, "AnimationEventBoxIn2"); //播放技能特效//
                    animationEventManager.AddAnimationEvent(12 / dstClip.frameRate, "AnimationEventBoxIn"); //开始攻击判定//
                    animationEventManager.AddAnimationEvent(21 / dstClip.frameRate, "AnimationEventBoxIn1"); //播放技能特效, 攻击判定结束//
                    animationEventManager.AddAnimationEvent(25 / dstClip.frameRate, "AnimationEventCanRemoveCoolDown");//cooldown帧限制//
                }
                break;
            case "Skill3R":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                    animationEventManager.AddAnimationEvent(3 / dstClip.frameRate, "AnimationEventBoxIn"); //开始攻击判定//
                    animationEventManager.AddAnimationEvent(7 / dstClip.frameRate, "AnimationEventBoxIn"); //开始攻击判定//
                    animationEventManager.AddAnimationEvent(11 / dstClip.frameRate, "AnimationEventBoxIn"); //开始攻击判定//
                    animationEventManager.AddAnimationEvent(15 / dstClip.frameRate, "AnimationEventBoxIn"); //开始攻击判定//
                    animationEventManager.AddAnimationEvent(20 / dstClip.frameRate, "AnimationEventBoxIn"); //开始攻击判定//
                    animationEventManager.AddAnimationEvent(20 / dstClip.frameRate, "AnimationEventBoxIn1"); //倒地标示//
                    animationEventManager.AddAnimationEvent(30 / dstClip.frameRate, "AnimationEventCanRemoveCoolDown");//cooldown帧限制//
                }
                break;
            case "Attack99R":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                    animationEventManager.AddAnimationEvent(0 / dstClip.frameRate, "AnimationEventBoxIn"); //触发碰撞体碰撞//
                } 
                break;
            case "Attack99LoopR":
                {
                    dstClip.wrapMode = WrapMode.Loop;
                }
                break;
            case "AttackSquat1R":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                    animationEventManager.AddAnimationEvent(4 / dstClip.frameRate, "AnimationEventBoxIn"); //触发碰撞体碰撞//
                    animationEventManager.AddAnimationEvent(8 / dstClip.frameRate, "AnimationEventBoxIn1"); //换下一个动作//
                    animationEventManager.AddAnimationEvent(12f / dstClip.frameRate, "AnimationEventCanRemoveCoolDown"); //cooldown帧限制//
                    animationEventManager.AddAnimationEvent(1 / dstClip.frameRate, "AnimationEventMovePlay");//人物位移
                    animationEventManager.AddAnimationEvent(5 / dstClip.frameRate, "AnimationEventMoveStop");//人物位移停止
                }
                break;
            case "AttackSquat2R":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                    animationEventManager.AddAnimationEvent(2f / dstClip.frameRate, "AnimationEventBoxIn"); //触发碰撞体碰撞//
                    animationEventManager.AddAnimationEvent(5f / dstClip.frameRate, "AnimationEventBoxIn1"); //触发碰撞体碰撞//
                    animationEventManager.AddAnimationEvent(0 / dstClip.frameRate, "AnimationEventMovePlay");//人物位移
                    animationEventManager.AddAnimationEvent(3 / dstClip.frameRate, "AnimationEventMoveStop");//人物位移停止
                }
                break;
            case "SquatR":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
            case "SquatHitR":
                {
                    animationEventManager.AddAnimationEvent(6 / dstClip.frameRate, "AnimationEventBoxIn3");// 落地烟尘激活 //
                    animationEventManager.AddAnimationEvent(22 / dstClip.frameRate, "AnimationEventBoxIn4"); //死亡使用//
                    animationEventManager.AddAnimationEvent(10 / dstClip.frameRate, "AnimationEventLieDownBegin"); //躺地开始//
                    animationEventManager.AddAnimationEvent(30 / dstClip.frameRate, "AnimationEventLieDownEnd"); //躺地结束//
                    dstClip.wrapMode = WrapMode.ClampForever; 
                }
                break;
            case "SquatLoopR":
                {
                    dstClip.wrapMode = WrapMode.Loop;
                }
                break;
            case "AirAttackR":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                    animationEventManager.AddAnimationEvent(4 / dstClip.frameRate, "AnimationEventBoxIn"); //触发碰撞体碰撞//
                    animationEventManager.AddAnimationEvent(7 / dstClip.frameRate, "AnimationEventBoxIn1"); //触发碰撞体碰撞//
                    animationEventManager.AddAnimationEvent(10 / dstClip.frameRate, "AnimationEventCanRemoveCoolDown");//cooldown帧限制//
                    animationEventManager.AddAnimationEvent(1 / dstClip.frameRate, "AnimationEventMovePlay");//人物位移
                    animationEventManager.AddAnimationEvent(5 / dstClip.frameRate, "AnimationEventMoveStop");//人物位移停止
                }
                break;
            case "QuickRunningR":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                    //                     animationEventManager.AddAnimationEvent(1 / dstClip.frameRate, "AnimationMoveWardEventL");
                    //                     animationEventManager.AddAnimationEvent(9 / dstClip.frameRate, "AnimationMoveWardEventR"); 
                    //                     animationEventManager.AddAnimationEvent(23 / dstClip.frameRate, "AnimationMoveWardEventL"); 
                }
                break;
            case "QuickRunningBackR":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                    //                     animationEventManager.AddAnimationEvent(1 / dstClip.frameRate, "AnimationMoveBackWardEventL");
                    //                     animationEventManager.AddAnimationEvent(18 / dstClip.frameRate, "AnimationMoveBackWardEventR");
                    //                     animationEventManager.AddAnimationEvent(28 / dstClip.frameRate, "AnimationMoveBackWardEventL"); 
                }
                break;
            case "Hit3LoopR":
                {
                    dstClip.wrapMode = WrapMode.Loop;
                }
                break;
            case "BounceR":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
            case "HitSpinStartR":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
            case "HitSpinLoopR":
                {
                    dstClip.wrapMode = WrapMode.Loop;
                }
                break;
            case "HitSpinEndR":
                {
                    animationEventManager.AddAnimationEvent(20 / dstClip.frameRate, "AnimationEventBoxIn");// 死亡停止 //
                    animationEventManager.AddAnimationEvent(15 / dstClip.frameRate, "AnimationEventLieDownBegin"); //躺地开始//
                    animationEventManager.AddAnimationEvent(30 / dstClip.frameRate, "AnimationEventLieDownEnd"); //躺地结束//
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
            case "HitbackStartR":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
            case "HitbackLoopR":
                {
                    dstClip.wrapMode = WrapMode.Loop;
                }
                break;
            case "HitbackEndR":
                {
                    animationEventManager.AddAnimationEvent(15 / dstClip.frameRate, "AnimationEventBoxIn");// 死亡停止 //
                    animationEventManager.AddAnimationEvent(10 / dstClip.frameRate, "AnimationEventLieDownBegin"); //躺地开始//
                    animationEventManager.AddAnimationEvent(25 / dstClip.frameRate, "AnimationEventLieDownEnd"); //躺地结束//
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
            case "HitHardR":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
            case "HitBellyR":
                {
                    dstClip.wrapMode = WrapMode.ClampForever;
                }
                break;
        }

        animationEventManager.AddAnimationEvent(dstClip.length, "AnimationEventEnd");
        animationEventManager.SaveAnimationEvent();
    }

}

