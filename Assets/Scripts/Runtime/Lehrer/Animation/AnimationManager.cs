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
            var animationClip = Lehrer.Animator.GetCurrentAnimatorClipInfo(0)[0].clip;
            var animationSpeed = animationClip.averageSpeed;

            // Hier nicht animationSpeed.x + animationSpeed.z benutzen,
            // sondern den Betrag des Geschwindigkeitsvektors  mit Satz des Phytagoras berechnen
            Lehrer.Agent.speed = Mathf.Sqrt(animationSpeed.x * animationSpeed.x + animationSpeed.z * animationSpeed.z);
            
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