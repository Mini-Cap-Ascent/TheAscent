using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TaskName("Wait for Animation")]
[TaskCategory("Generic")]
[TaskDescription("wait for an animation to be completed via the Animation Listener")]
public class WaitForAnimationAction : Action
{
    public SharedString animationName;

    private AnimationListener animationListener;

    private bool animationCompleted = false;
    private int animationHashCode;

    public override void OnStart()
    {
        animationCompleted = false;
        animationHashCode = Animator.StringToHash(animationName.Value);

        animationListener = gameObject.GetComponent<AnimationListener>();
        animationListener.AddAnimationCompletedListener(animationHashCode, OnAnimationCompleted);
    }

    private void OnAnimationCompleted(int hashCode)
    {
        if(hashCode == animationHashCode)
        {
            animationCompleted = true;
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (animationCompleted == true)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        animationListener.RemoveAnimationCompletedListener(animationHashCode, OnAnimationCompleted);
    }
}
