using System;
using System.Collections;
using System.Collections.Generic;

namespace HerderGames.Lehrer.Animation
{
    public class TimelineAnimation : AbstractAnimation
    {
        private readonly AbstractAnimation[] Animations;

        public TimelineAnimation(params AbstractAnimation[] animations)
        {
            Animations = animations;
        }
        
        public override IEnumerable Play(Lehrer lehrer, Stack<Action> unexpectedAnimationEndCallback)
        {
            foreach (var animation in Animations)
            {
                foreach (var _ in animation.Play(lehrer, unexpectedAnimationEndCallback))
                {
                    yield return null;
                }
            }
        }
    }
}
