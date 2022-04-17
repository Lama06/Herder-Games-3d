using HerderGames.Time;
using UnityEngine;
using UnityEngine.AI;

namespace HerderGames.AI.Goals
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Unterrichten : GoalBase
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private Transform UnterrichtsRaum;
        [SerializeField] private WoechentlicheZeitspannen Wann;

        private NavMeshAgent Agent;

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Wann.IsInside(TimeManager.GetCurrentWochentag(), TimeManager.GetCurrentTime());
        }

        public override void OnStarted()
        {
            Agent.destination = UnterrichtsRaum.position;
        }
    }
}