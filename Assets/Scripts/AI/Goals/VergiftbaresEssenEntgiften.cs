using System.Collections;
using HerderGames.Schule;
using HerderGames.Time;
using UnityEngine;
using UnityEngine.AI;

namespace HerderGames.AI.Goals
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class VergiftbaresEssenEntgiften : GoalBase
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private WoechentlicheZeitspannen Wann;
        [SerializeField] private VergiftbaresEssen Essen;

        private NavMeshAgent Agent;

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
        }

        public override bool CanStart()
        {
            return Essen.Status == VergiftungsStatus.VergiftetBemerkt &&
                   Wann.IsInside(TimeManager.GetCurrentWochentag(), TimeManager.GetCurrentTime());
        }

        public override bool ShouldContinue()
        {
            return CanStart();
        }

        public override void OnStarted()
        {
            StartCoroutine(Execute());
        }

        private IEnumerator Execute()
        {
            Agent.destination = Essen.GetStandpunkt();
            yield return new WaitUntil(() => !Agent.hasPath && !Agent.pathPending);
            Essen.Status = VergiftungsStatus.NichtVergiftet;
        }
    }
}