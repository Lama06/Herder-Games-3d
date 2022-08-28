using System;
using System.Collections;
using System.Collections.Generic;

namespace HerderGames.Lehrer.Animation
{
    public abstract class AbstractAnimation
    {
        public abstract IEnumerable Play(Lehrer lehrer, Stack<Action> unexpectedAnimationEndCallback);
    }
}
