using System;
using UnityEngine;

public class AnimationReciever : MonoBehaviour
{
    public event Action<AnimationEvent> AttackFrame;
    public event Action<AnimationEvent> AttackEnd;
    void OnAttackFrame(AnimationEvent animationEvent)
    {
        AttackFrame?.Invoke(animationEvent);
    }

    void OnAttackEnd(AnimationEvent animationEvent)
    {
        AttackEnd?.Invoke(animationEvent);
    }
}