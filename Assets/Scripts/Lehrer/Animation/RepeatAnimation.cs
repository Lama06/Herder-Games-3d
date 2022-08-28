using System;
using System.Collections;
using System.Collections.Generic;

namespace HerderGames.Lehrer.Animation
{
    public class RepeatAnimation : AbstractAnimation
    {
        private readonly AbstractAnimation Animation;
        private readonly int? Repeat;

        public RepeatAnimation(AbstractAnimation animation, int? repeat = null)
        {
            Animation = animation;
            Repeat = repeat;
        }
        
        public override IEnumerable Play(Lehrer lehrer, Stack<Action> unexpectedAnimationEndCallback)
        {
            for (var i = 1; Repeat == null || i <= (int) Repeat; i++)
            {
                foreach (var _ in Animation.Play(lehrer, unexpectedAnimationEndCallback))
                {
                    yield return null;
                }
            }
        }
    }
}
