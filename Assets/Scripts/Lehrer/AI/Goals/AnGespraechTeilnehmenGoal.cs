using System.Collections;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Animation;
using HerderGames.Lehrer.Sprache;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    public class AnGespraechTeilnehmenGoal : GoalBase
    {
        private readonly TriggerBase Trigger;
        private readonly Vector3 Standpunkt;
        public Gespraech Gespraech { get; }
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeAufDemWeg;
        private readonly AbstractAnimation AnimationWeg;
        private readonly AbstractAnimation AnimationAngekommen;

        public bool IsAngekommen { get; private set; }

        public AnGespraechTeilnehmenGoal(
            Lehrer lehrer,
            TriggerBase trigger,
            Vector3 standpunkt,
            Gespraech gespraech,
            ISaetzeMoeglichkeitenMehrmals saetzeAufDemWeg = null,
            AbstractAnimation animationWeg = null,
            AbstractAnimation animationAngekommen = null
        ) : base(lehrer)
        {
            Trigger = trigger;
            Standpunkt = standpunkt;
            Gespraech = gespraech;
            SaetzeAufDemWeg = saetzeAufDemWeg;
            AnimationWeg = animationWeg;
            AnimationAngekommen = animationAngekommen;
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Trigger.ShouldRun;
        }

        protected override IEnumerator Execute()
        {
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAufDemWeg;
            Lehrer.AnimationManager.CurrentAnimation = AnimationWeg;
            Lehrer.Agent.destination = Standpunkt;
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            Lehrer.AnimationManager.CurrentAnimation = AnimationAngekommen;
            Lehrer.Sprache.SaetzeMoeglichkeiten = null;
            IsAngekommen = true;
        }

        protected override void OnGoalEnd()
        {
            IsAngekommen = false;
        }
    }
}
