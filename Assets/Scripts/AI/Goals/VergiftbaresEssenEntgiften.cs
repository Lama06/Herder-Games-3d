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

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Essen.Status == VergiftungsStatus.VergiftetBemerkt && Wann.IsInside(TimeManager.GetCurrentWochentag(), TimeManager.GetCurrentTime());
        }

        public override IEnumerator Execute()
        {
            Agent.destination = Essen.GetStandpunkt();
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Agent);
            Essen.Status = VergiftungsStatus.NichtVergiftet;
        }
    }
}