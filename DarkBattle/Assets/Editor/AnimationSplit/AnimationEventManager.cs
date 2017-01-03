using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class AnimationEventManager
{
    /// <summary>
    /// 所有事件集合
    /// </summary>
    private List<AnimationEvent> mAllAnimationEvent = new List<AnimationEvent>();
       
    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="Clip">需要操作的动画</param>
    public AnimationEventManager(AnimationClip Clip)
    {
        m_dstClip = Clip;
    }
    private AnimationClip m_dstClip;
    /// <summary>
    /// 当前动画片段
    /// </summary>
    public AnimationClip Clip
    {
        get
        {
            return m_dstClip;
        }
    }

    /// <summary>
    /// 添加当前动画事件
    /// </summary>
    /// <param name="time">帧</param>
    /// <param name="functionName">函数名</param>
    public void AddAnimationEvent(float time, string functionName)
    {
        if (Clip == null)
        {
            return;
        }
        if (time > Clip.length)
        {
            Debug.LogError("设置的事件响应时间大于动画的总长度！" + functionName);
            return;
        }
        if (functionName == null || functionName == "")
        {
            Debug.LogError("动画响应事件函数名不能为空！");
            return;
        }
        AnimationEvent _event = new AnimationEvent();
        _event.time = time; // -0.0016f;
        _event.stringParameter = Clip.name;
        _event.functionName = functionName;
        mAllAnimationEvent.Add(_event);
        //SaveAnimationEvent();
    }
    /// <summary>
    /// 对当前动画添加事件
    /// </summary>
    /// <param name="aEvent"></param>
    public void AddAnimationEvent(AnimationEvent aEvent)
    {
        if(aEvent != null)
        {
            mAllAnimationEvent.Add(aEvent);
        }
        //SaveAnimationEvent();
    }
    public void RemoveAnimationEventByTime(float time)
    {
        AnimationEvent t = mAllAnimationEvent.Find(a => a.time == time);
        if (t != null)
        {
            mAllAnimationEvent.Remove(t);
        }
        else
        {
            Debug.LogError("当前time没有动画帧");
            return;
        }
        //SaveAnimationEvent();
    }

    /// <summary>
    /// 应用当前设置到动画
    /// </summary>
    public void SaveAnimationEvent()
    {
        AnimationEvent[] aevent = new AnimationEvent[mAllAnimationEvent.Count];
        for (int i = 0; i < aevent.Length; i++)
        {
            aevent[i] = mAllAnimationEvent[i];
            Debug.Log(string.Format("SaveAnimationEvent AnimationName:{0}  time:{1} functionName:{2}", Clip.name, mAllAnimationEvent[i].time, mAllAnimationEvent[i].functionName));
        }
        AnimationUtility.SetAnimationEvents(Clip, aevent);
    }
}
