using System.Collections;
using HerderGames.Lehrer.AI.Trigger;
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

        public SchuleVerlassenGoal(
            Lehrer lehrer,
            TriggerBase trigger,
            Vector3 eingang,
            Vector3 ausgang,
            ISaetzeMoeglichkeitenMehrmals saetzeBeimVerlassen = null
        ) : base(lehrer)
        {
            Trigger = trigger;
            Eingang = eingang;
            Ausgang = ausgang;
            SaetzeBeimVerlassen = saetzeBeimVerlassen;
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Trigger.ShouldRun;
        }

        protected override IEnumerator Execute()
        {
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeBeimVerlassen;
            Lehrer.Agent.destination = Ausgang;
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
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
