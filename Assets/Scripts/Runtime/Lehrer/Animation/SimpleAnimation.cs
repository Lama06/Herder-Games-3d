using System;
using System.Collections;
using System.Collections.Generic;
using HerderGames.Util;
using UnityEngine;

namespace HerderGames.Lehrer.Animation
{
    public class SimpleAnimation : AbstractAnimation
    {
        private readonly AnimationName Animation;
        private readonly float Seconds;

        public SimpleAnimation(AnimationName animation, float seconds = float.PositiveInfinity)
        {
            Animation = animation;
            Seconds = seconds;
        }

        public override IEnumerable Play(Lehrer lehrer, Stack<Action> unexpectedAnimationEndCallback)
        {
            lehrer.Animator.SetBool(Animation.ParameterName(), true);
            unexpectedAnimationEndCallback.Push(() => lehrer.Animator.SetBool(Animation.ParameterName(), false));

            lehrer.Animator.SetBool(Animation.AnimationType().ParameterName(), true);
            unexpectedAnimationEndCallback.Push(() => lehrer.Animator.SetBool(Animation.AnimationType().ParameterName(), false));
            
            foreach (var _ in IteratorUtil.WaitForSeconds(Seconds))
            {
                yield return null;
            }

            unexpectedAnimationEndCallback.Pop()();
            unexpectedAnimationEndCallback.Pop()();
        }
    }
}
