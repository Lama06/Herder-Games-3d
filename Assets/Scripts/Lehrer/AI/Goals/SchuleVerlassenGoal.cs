using System.Collections;
using HerderGames.Lehrer.Sprache;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    public class SchuleVerlassenGoal : GoalBase
    {
        [SerializeField] private Trigger.Trigger Trigger;
        [SerializeField] private Transform Eingang;
        [SerializeField] private Transform Ausgang;
        [SerializeField] private SaetzeMoeglichkeitenMehrmals SaetzeBeimVerlassen;

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
            Lehrer.Agent.destination = Ausgang.position;
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            Lehrer.InSchule.SetInSchule(false);
        }

        public override void OnGoalEnd(GoalEndReason reason)
        {
            if (Lehrer.InSchule.GetInSchule())
            {
                return;
            }
            
            Lehrer.Agent.Warp(Eingang.position);
            Lehrer.InSchule.SetInSchule(true);
        }
    }
}