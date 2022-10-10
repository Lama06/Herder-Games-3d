using System.Collections;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Animation;
using HerderGames.Lehrer.Sprache;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    public class SchuleVerlassenGoal : GoalBase
    {
        private readonly TriggerBase Trigger;
        private readonly Vector3 Eingang;
        private readonly Vector3 Ausgang;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeBeimVerlassen;
        private readonly AbstractAnimation AnimationBeimVerlassen;

        public SchuleVerlassenGoal(
            Lehrer lehrer,
            TriggerBase trigger,
            Vector3 eingang,
            Vector3 ausgang,
            ISaetzeMoeglichkeitenMehrmals saetzeBeimVerlassen = null,
            AbstractAnimation animationBeimVerlassen = null
        ) : base(lehrer)
        {
            Trigger = trigger;
            Eingang = eingang;
            Ausgang = ausgang;
            SaetzeBeimVerlassen = saetzeBeimVerlassen;
            AnimationBeimVerlassen = animationBeimVerlassen;
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Trigger.ShouldRun;
        }

        protected override IEnumerator Execute()
        {
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeBeimVerlassen;
            Lehrer.AnimationManager.CurrentAnimation = AnimationBeimVerlassen;
            Lehrer.Agent.destination = Ausgang;
            yield return NavMeshUtil.Pathfind(Lehrer.Agent);
            Lehrer.InSchule.InSchule = false;
        }

        protected override void OnGoalEnd()
        {
            if (Lehrer.InSchule.InSchule)
            {
                return;
            }

            Lehrer.Agent.Warp(Eingang);
            Lehrer.InSchule.InSchule = true;
        }
    }
}
