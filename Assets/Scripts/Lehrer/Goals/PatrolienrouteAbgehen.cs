using System.Collections;
using HerderGames.AI;
using HerderGames.Time;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    public class PatrolienrouteAbgehen : LehrerGoalBase
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private Transform[] Punkte;
        [SerializeField] private WoechentlicheZeitspannen Wann;
        
        public override bool ShouldRun(bool currentlyRunning)
        {
            return Wann.IsInside(TimeManager.GetCurrentWochentag(), TimeManager.GetCurrentTime());
        }

        public override IEnumerator Execute()
        {
            while (true)
            {
                foreach (var punkt in Punkte)
                {
                    Lehrer.GetAgent().destination = punkt.position;
                    yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.GetAgent());
                }

                for (var i = Punkte.Length - 1; i >= 0; i--)
                {
                    Lehrer.GetAgent().destination = Punkte[0].position;
                    yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.GetAgent());
                }
            }
        }
    }
}