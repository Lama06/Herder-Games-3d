using System.Collections;
using System.Collections.Generic;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Sprache;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    public class PatrolienrouteAbgehenGoal : GoalBase
    {
        private readonly TriggerBase Trigger;
        private readonly IList<Vector3> Punkte;
        private readonly ISaetzeMoeglichkeitenMehrmals Saetze;

        public PatrolienrouteAbgehenGoal(
            Lehrer lehrer,
            TriggerBase trigger,
            IList<Vector3> punkte,
            ISaetzeMoeglichkeitenMehrmals saetze = null
        ) : base(lehrer)
        {
            Trigger = trigger;
            Punkte = punkte;
            Saetze = saetze;
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Trigger.ShouldRun;
        }

        protected override IEnumerator Execute()
        {
            Lehrer.Sprache.SaetzeMoeglichkeiten = Saetze;
            while (true)
            {
                foreach (var punkt in Punkte)
                {
                    Lehrer.Agent.destination = punkt;
                    yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
                }

                // Den ersten und letzten Punkt überspringen, damit der Lehrer nicht doppelt zum selben Punkt läuft
                for (var i = Punkte.Count - 2; i >= 1; i--)
                {
                    Lehrer.Agent.destination = Punkte[i];
                    yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
                }
            }
        }
    }
}
