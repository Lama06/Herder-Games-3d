using System.Collections;
using HerderGames.AI;
using HerderGames.Lehrer.Sprache;
using HerderGames.Time;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    public class PatrolienrouteAbgehen : LehrerGoalBase
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private Transform[] Punkte;
        [SerializeField] private WoechentlicheZeitspannen Wann;
        [SerializeField] private Saetze Saetze;
        
        public override bool ShouldRun(bool currentlyRunning)
        {
            return Wann.IsInside(TimeManager.GetCurrentWochentag(), TimeManager.GetCurrentTime());
        }

        public override IEnumerator Execute()
        {
            Lehrer.Sprache.SetSatzSource(Saetze);
            while (true)
            {
                foreach (var punkt in Punkte)
                {
                    Lehrer.Agent.destination = punkt.position;
                    Debug.Log(Lehrer.Agent.pathPending);
                    yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
                }
                
                for (var i = Punkte.Length - 2; i >= 1; i--)
                {
                    Lehrer.Agent.destination = Punkte[i].position;
                    Debug.Log(Lehrer.Agent.pathPending);
                    yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
                }
            }
        }
    }
}