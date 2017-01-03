
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class EffectAddEndEffectEvent
{
    [MenuItem("Assets/动画特效编辑/在特效最后一帧添加事件")]
    public static void ExportEndEffectEvent()
    {
        Export();
    }
    static void Export()
    {
        if (Selection.objects != null && Selection.objects.Length > 0)
        {
            foreach (Object o in Selection.objects)
            {
                addEffectAnimationEventEnd(o);
            }
        }
    }
    public static void addEffectAnimationEventEnd(object o)
    {
        AnimationClip clip = o as AnimationClip;
        if (clip != null)
        {
            List<AnimationEvent> list = new List<AnimationEvent>(AnimationUtility.GetAnimationEvents(clip));
            for (int i = 0; i < list.Count; i++)
            {
                AnimationEvent ec = list[i];
                if (ec.functionName == "EffectAnimationEventEnd")
                {
                    list.Remove(ec);
                }
            }
            AnimationEvent e = new AnimationEvent();
            e.functionName = "EffectAnimationEventEnd";
            e.time = clip.length;
            e.stringParameter = clip.name;
            list.Add(e);
            AnimationUtility.SetAnimationEvents(clip, list.ToArray());
        }
    }
}
