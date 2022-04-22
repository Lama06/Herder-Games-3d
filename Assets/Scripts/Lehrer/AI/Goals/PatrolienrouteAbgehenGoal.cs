using System.Collections;
using HerderGames.Lehrer.Sprache;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    public class PatrolienrouteAbgehenGoal : GoalBase
    {
        [SerializeField] private Trigger.Trigger Trigger;
        [SerializeField] private Transform[] Punkte;
        [SerializeField] private SaetzeMoeglichkeitenMehrmals Saetze;
        
        public override bool ShouldRun(bool currentlyRunning)
        {
            return Trigger.Resolve();
        }

        public override IEnumerator Execute()
        {
            Lehrer.Sprache.SaetzeMoeglichkeiten = Saetze;
            while (true)
            {
                foreach (var punkt in Punkte)
                {
                    Lehrer.Agent.destination = punkt.position;
                    yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
                }
                
                // Den ersten und letzten Punkt überspringen, damit der Lehrer nicht doppelt zum selben Punkt läuft
                for (var i = Punkte.Length - 2; i >= 1; i--)
                {
                    Lehrer.Agent.destination = Punkte[i].position;
                    yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
                }
            }
        }
    }
}