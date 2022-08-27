using System.Collections;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer;
using HerderGames.Lehrer.Sprache;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    public class MoveToAndStandAtGoal : GoalBase
    {
        private readonly TriggerBase Trigger;
        private readonly Vector3 Position;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeWeg;
        private readonly ISaetzeMoeglichkeitenEinmalig SaetzeAngekommenEinmalig;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeAngekommen;
        private readonly AnimationType AnimationWeg;
        private readonly AnimationType AnimationAngekommen;

        public MoveToAndStandAtGoal(
            Lehrer lehrer,
            TriggerBase trigger,
            Vector3 position,
            ISaetzeMoeglichkeitenMehrmals saetzeWeg = null,
            ISaetzeMoeglichkeitenMehrmals saetzeAngekommen = null,
            ISaetzeMoeglichkeitenEinmalig saetzeAngekommenEinmalig = null,
            AnimationType animationWeg = AnimationType.WALKING,
            AnimationType animationAngekommen = AnimationType.IDLE
        ) : base(lehrer)
        {
            Trigger = trigger;
            Position = position;
            SaetzeWeg = saetzeWeg;
            SaetzeAngekommenEinmalig = saetzeAngekommenEinmalig;
            SaetzeAngekommen = saetzeAngekommen;
            AnimationWeg = animationWeg;
            AnimationAngekommen = animationAngekommen;
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Trigger.ShouldRun;
        }

        protected override IEnumerator Execute()
        {
            Lehrer.Animator.Play(AnimationWeg.AnimationName());
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeWeg;
            Lehrer.Agent.destination = Position;
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            Lehrer.Animator.Play(AnimationAngekommen.AnimationName());
            Lehrer.Sprache.Say(SaetzeAngekommenEinmalig);
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAngekommen;
        }
    }
}
