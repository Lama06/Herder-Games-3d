using HerderGames.Time;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    public class MoveToAndStandAtGoal : LehrerGoalBase
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private Transform Position;
        [SerializeField] private WoechentlicheZeitspannen Wann;

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Wann.IsInside(TimeManager.GetCurrentWochentag(), TimeManager.GetCurrentTime());
        }

        public override void OnStarted()
        {
            Lehrer.GetAgent().destination = Position.position;
        }
    }
}