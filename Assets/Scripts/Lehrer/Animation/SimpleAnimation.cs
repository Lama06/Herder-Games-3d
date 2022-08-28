using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HerderGames.Lehrer.Animation
{
    public class SimpleAnimation : AbstractAnimation
    {
        private readonly AnimationType Animation;
        private readonly float Seconds;

        public SimpleAnimation(AnimationType animation, float seconds)
        {
            Animation = animation;
            Seconds = seconds;
        }

        public override IEnumerable Play(Lehrer lehrer, Stack<Action> unexpectedAnimationEndCallback)
        {
            lehrer.Animator.SetBool(Animation.ParameterName(), true);
            unexpectedAnimationEndCallback.Push(() => lehrer.Animator.SetBool(Animation.ParameterName(), false));

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
        }
    }
}
