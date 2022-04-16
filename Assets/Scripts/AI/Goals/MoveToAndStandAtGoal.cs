using HerderGames.Time;
using UnityEngine;
using UnityEngine.AI;

namespace HerderGames.AI.Goals
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MoveToAndStandAtGoal : GoalBase
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private Transform Position;
        [SerializeField] private WoechentlicheZeitspannen Wann;

        private NavMeshAgent Agent;

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
        }

        public override bool CanStart()
        {
            return Wann.IsInside(TimeManager.GetCurrentWochentag(), TimeManager.GetCurrentTime());
        }

        public override bool ShouldContinue()
        {
            return Wann.IsInside(TimeManager.GetCurrentWochentag(), TimeManager.GetCurrentTime());
        }

        public override void OnStarted()
        {
            Agent.destination = Position.position;
        }
    }
}