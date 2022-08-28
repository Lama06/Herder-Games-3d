using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HerderGames.Lehrer.Animation
{
    [RequireComponent(typeof(Lehrer))]
    public class AnimationManager : MonoBehaviour
    {
        private Lehrer Lehrer;
        private CurrentAnimationData _CurrentAnimation;

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
        
        private void Awake()
        {
            Lehrer = GetComponent<Lehrer>();
        }

        private void Update()
        {
            if (_CurrentAnimation == null)
            {
                return;
            }

            var success = _CurrentAnimation.Enumerator.MoveNext();
            if (!success)
            {
                _CurrentAnimation = null;
            }
        }

        private class CurrentAnimationData
        {
            public IEnumerator Enumerator;
            public Stack<Action> UnexpectedAnimationEndCallback;
        }
    }
}
