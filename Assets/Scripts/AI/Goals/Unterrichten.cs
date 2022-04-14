using HerderGames.Time;
using HerderGames.Time.Data;
using UnityEngine;
using UnityEngine.AI;

namespace HerderGames.AI.Goals
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Unterrichten : GoalBase
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private Transform UnterrichtsRaum;
        [SerializeField] private Zeitspanne Wann;

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
            Agent.destination = UnterrichtsRaum.position;
        }
    }
}