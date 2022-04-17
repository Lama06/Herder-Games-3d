using HerderGames.AI;
using HerderGames.Time;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    [RequireComponent(typeof(Lehrer))]
    public class MoveToAndStandAtGoal : GoalBase
    {
        [SerializeField] private Transform Position;
        [SerializeField] private WoechentlicheZeitspannen Wann;

        private Lehrer Lehrer;

        private void Awake()
        {
            Lehrer = GetComponent<Lehrer>();
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Wann.IsInside(Lehrer.GetTimeManager().GetCurrentWochentag(),
                Lehrer.GetTimeManager().GetCurrentTime());
        }

        public override void OnStarted()
        {
            Lehrer.GetAgent().destination = Position.position;
        }
    }
}