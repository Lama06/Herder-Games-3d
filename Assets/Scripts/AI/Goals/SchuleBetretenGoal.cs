using UnityEngine;
using UnityEngine.AI;

namespace HerderGames.AI.Goals
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(SchuleBetretenVerlassenSharedData))]
    public class SchuleBetretenGoal : GoalBase
    {
        [SerializeField] private Transform Eingang;
        
        private SchuleBetretenVerlassenSharedData Shared;
        private NavMeshAgent Agent;

        private void Awake()
        {
            Shared = GetComponent<SchuleBetretenVerlassenSharedData>();
            Agent = GetComponent<NavMeshAgent>();
        }

        public override bool CanStart()
        {
            return Shared.ShouldBeInSchool() && !Shared.InDerSchule;
        }

        public override bool ShouldContinue()
        {
            return CanStart();
        }

        public override void OnStarted()
        {
            Agent.Warp(Eingang.position);
            Shared.SetVisible(true);
            Shared.InDerSchule = true;
        }
    }
}