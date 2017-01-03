using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.IO;

//[CustomEditor(typeof(AnimationClip), true)]
public class AnimationClipEditor : Editor
{
    private AnimationClip ani;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("\n特效最后一帧增加一个事件\n\n按这个按钮\n"))
        {
            EffectAddEndEffectEvent.addEffectAnimationEventEnd(base.target);
        }
    }
    void OnEnable ()
	{
        if (ani == null)
        {
            ani = target as AnimationClip;
        }
    }

}
