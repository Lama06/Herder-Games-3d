using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HerderGames.Lehrer.Animation
{ 
    public class AnimationManager
    {
        private readonly Lehrer Lehrer;
        private CurrentAnimationData _CurrentAnimation;

        public AnimationManager(Lehrer lehrer)
        {
            Lehrer = lehrer;
        }
        
        public AbstractAnimation CurrentAnimation
        {
            set
            {
                if (_CurrentAnimation != null)
                {
                    foreach (var action in _CurrentAnimation.UnexpectedAnimationEndCallback)
                    {
                        action();
                    }
                }

                if (value == null)
                {
                    _CurrentAnimation = null;
                }
                else
                {
                    var unexpectedAnimationEndCallback = new Stack<Action>();
                    var enumerator = value.Play(Lehrer, unexpectedAnimationEndCallback).GetEnumerator();
                    _CurrentAnimation = new CurrentAnimationData
                    {
                        Enumerator = enumerator,
                        UnexpectedAnimationEndCallback = unexpectedAnimationEndCallback
                    };
                }
            }
        }

        public void Update()
        {
            if (_CurrentAnimation != null)
            {
                var success = _CurrentAnimation.Enumerator.MoveNext();
                if (!success)
                {
                    _CurrentAnimation = null;
                }   
            }
        }

        private class CurrentAnimationData
        {
            public IEnumerator Enumerator;
            public Stack<Action> UnexpectedAnimationEndCallback;
        }
    }
}