using System.Collections;
using HerderGames.AI;
using HerderGames.Lehrer.Sprache;
using HerderGames.Time;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    public class PatrolienrouteAbgehenGoal : LehrerGoalBase
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private Transform[] Punkte;
        [SerializeField] private WoechentlicheZeitspannen Wann;
        [SerializeField] private SaetzeMoeglichkeitenMehrmals Saetze;
        
        public override bool ShouldRun(bool currentlyRunning)
        {
            return Wann.IsInside(TimeManager.GetCurrentWochentag(), TimeManager.GetCurrentTime());
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