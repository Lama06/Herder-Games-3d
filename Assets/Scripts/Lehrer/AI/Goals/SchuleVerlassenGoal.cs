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
        private readonly SaetzeMoeglichkeitenMehrmals SaetzeBeimVerlassen;

        public SchuleVerlassenGoal(
            Lehrer lehrer,
            TriggerBase trigger,
            Vector3 eingang,
            Vector3 ausgang,
            SaetzeMoeglichkeitenMehrmals saetzeBeimVerlassen
        ) : base(lehrer)
        {
            Trigger = trigger;
            Eingang = eingang;
            Ausgang = ausgang;
            SaetzeBeimVerlassen = saetzeBeimVerlassen;
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return ShouldBeOutsideSchool();
        }

        private bool ShouldBeOutsideSchool()
        {
            return Trigger.Resolve();
        }

        public override IEnumerator Execute()
        {
            if (!Lehrer.InSchule.GetInSchule())
            {
                yield break;
            }

            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeBeimVerlassen;
            Lehrer.Agent.destination = Ausgang;
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            Lehrer.InSchule.SetInSchule(false);
        }

        public override void OnGoalEnd(GoalEndReason reason)
        {
            if (Lehrer.InSchule.GetInSchule())
            {
                return;
            }

            Lehrer.Agent.Warp(Eingang);
            Lehrer.InSchule.SetInSchule(true);
        }
    }
}
