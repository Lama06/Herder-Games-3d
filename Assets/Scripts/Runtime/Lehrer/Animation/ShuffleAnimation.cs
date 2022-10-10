using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace HerderGames.Lehrer.Animation
{
    public class ShuffleAnimation : AbstractAnimation
    {
        private readonly Choice[] Choices;

        public ShuffleAnimation(params Choice[] choices)
        {
            Choices = choices;
        }

        private AbstractAnimation RandomAnimation
        {
            get
            {
                var weightedAnimationsList = new List<AbstractAnimation>();
                foreach (var choice in Choices)
                {
                    for (var i = 1; i <= choice.Probability; i++)
                    {
                        weightedAnimationsList.Add(choice.Animation);
                    }
                }

                return weightedAnimationsList[Random.Range(0, weightedAnimationsList.Count)];
            }
        }

        public override IEnumerable Play(Lehrer lehrer, Stack<Action> unexpectedAnimationEndCallback)
        {
            var abstractAnimation = RandomAnimation;

            foreach (var _ in abstractAnimation.Play(lehrer, unexpectedAnimationEndCallback))
            {
                yield return null;
            }
        }

        public struct Choice
        {
            public readonly int Probability;
            public readonly AbstractAnimation Animation;

            public Choice(int probability, AbstractAnimation animation)
            {
                Probability = probability;
                Animation = animation;
            }
        }
    }
}
