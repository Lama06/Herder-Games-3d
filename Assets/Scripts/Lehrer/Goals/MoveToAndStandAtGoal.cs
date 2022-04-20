using System.Collections;
using HerderGames.AI;
using HerderGames.Lehrer.Sprache;
using HerderGames.Time;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    public class MoveToAndStandAtGoal : LehrerGoalBase
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private Transform Position;
        [SerializeField] private WoechentlicheZeitspannen Wann;
        [SerializeField] private SaetzeMehrmals SaetzeWeg;
        [SerializeField] private SaetzeMehrmals SaetzeAngekommen;

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Wann.IsInside(TimeManager.GetCurrentWochentag(), TimeManager.GetCurrentTime());
        }

        public override IEnumerator Execute()
        {
            Lehrer.Sprache.SetSatzSource(SaetzeWeg);
            Lehrer.Agent.destination = Position.position;
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            Lehrer.Sprache.SetSatzSource(SaetzeAngekommen);
        }
    }
}