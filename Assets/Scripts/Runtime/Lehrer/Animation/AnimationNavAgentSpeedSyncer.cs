using UnityEngine;

namespace HerderGames.Lehrer.Animation
{
    public class AnimationNavAgentSpeedSyncer
    {
        private readonly Lehrer Lehrer;

        public AnimationNavAgentSpeedSyncer(Lehrer lehrer)
        {
            Lehrer = lehrer;
        }

        public void Update()
        {
            var animationClip = Lehrer.Animator.GetCurrentAnimatorClipInfo(0)[0].clip;
            var animationSpeed = animationClip.averageSpeed;

            // Hier nicht animationSpeed.x + animationSpeed.z benutzen,
            // sondern den Betrag des Geschwindigkeitsvektors  mit Satz des Phytagoras berechnen
            Lehrer.Agent.speed = Mathf.Sqrt(animationSpeed.x * animationSpeed.x + animationSpeed.z * animationSpeed.z);
        }
    }
}