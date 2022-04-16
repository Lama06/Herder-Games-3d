using HerderGames.Schule;
using UnityEngine;
using UnityEngine.AI;

namespace HerderGames.AI.Goals
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BeiAlarmAufDenSchulhofGehen : GoalBase
    {
        [SerializeField] private AlarmManager AlarmManager;
        [SerializeField] private Transform Sammelpunkt;

        private NavMeshAgent Agent;

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
        }

        public override bool CanStart()
        {
            return AlarmManager.Alarm;
        }

        public override bool ShouldContinue()
        {
            return CanStart();
        }

        public override void OnStarted()
        {
            Agent.destination = Sammelpunkt.position;
        }
    }
}