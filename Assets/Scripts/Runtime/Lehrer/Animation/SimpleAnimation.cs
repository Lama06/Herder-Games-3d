using System;
using System.Collections;
using System.Collections.Generic;
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
            
            var remainingTime = Seconds;
            while (true)
            {
                yield return null;

                remainingTime -= Time.deltaTime;

                if (remainingTime <= 0f)
                {
                    break;
                }
            }

            unexpectedAnimationEndCallback.Pop()();
            unexpectedAnimationEndCallback.Pop()();
        }
    }
}
