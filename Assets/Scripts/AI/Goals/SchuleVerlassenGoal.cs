using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace HerderGames.AI.Goals
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(SchuleBetretenVerlassenSharedData))]
    public class SchuleVerlassenGoal : GoalBase
    {
        [SerializeField] private Transform Ausgang;
        
        private SchuleBetretenVerlassenSharedData Shared;
        private NavMeshAgent Agent;

        private void Awake()
        {
            Shared = GetComponent<SchuleBetretenVerlassenSharedData>();
            Agent = GetComponent<NavMeshAgent>();
        }
        
        public override bool CanStart()
        {
            return !Shared.ShouldBeInSchool() && Shared.InDerSchule;
        }

        public override bool ShouldContinue()
        {
            return CanStart();
        }

        public override void OnStarted()
        {
            StartCoroutine(Execute());
        }

        public override void OnEnd(GoalEndReason reason)
        {
            StopAllCoroutines();
        }

        private IEnumerator Execute()
        {
            Agent.destination = Ausgang.position;
            yield return new WaitUntil(() => !Agent.hasPath && !Agent.pathPending);
            Shared.SetVisible(false);
            Shared.InDerSchule = false;
        }
    }
}