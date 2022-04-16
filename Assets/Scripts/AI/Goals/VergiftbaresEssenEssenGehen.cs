using System.Collections;
using HerderGames;
using HerderGames.Schule;
using HerderGames.Time;
using UnityEngine;
using UnityEngine.AI;

namespace HerderGames.AI.Goals
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Krankheit))]
    public class VergiftbaresEssenEssenGehen : GoalBase
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private VergiftbaresEssen VergiftbaresEssen;
        [SerializeField] private WoechentlicheZeitspannen Wann;

        private NavMeshAgent Agent;
        private Krankheit Krankheit;

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Krankheit = GetComponent<Krankheit>();
        }

        public override bool CanStart()
        {
            return VergiftbaresEssen.Status != VergiftungsStatus.VergiftetBemerkt && Wann.IsInside(TimeManager.GetCurrentWochentag(), TimeManager.GetCurrentTime());
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
            Agent.destination = VergiftbaresEssen.GetStandpunkt();
            yield return new WaitUntil(() => !Agent.hasPath && !Agent.pathPending);
            if (VergiftbaresEssen.Status.IsVergiftet())
            {
                Krankheit.Erkranken(VergiftbaresEssen);
            }
        }
    }
}
