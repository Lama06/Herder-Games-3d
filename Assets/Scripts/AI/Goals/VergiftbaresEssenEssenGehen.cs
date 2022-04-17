using System.Collections;
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

        public override bool ShouldRun(bool currentlyRunning)
        {
            return VergiftbaresEssen.Status != VergiftungsStatus.VergiftetBemerkt && Wann.IsInside(TimeManager.GetCurrentWochentag(), TimeManager.GetCurrentTime());
        }

        public override IEnumerator Execute()
        {
            Agent.destination = VergiftbaresEssen.GetStandpunkt();
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Agent);
            if (VergiftbaresEssen.Status.IsVergiftet())
            {
                Krankheit.Erkranken(VergiftbaresEssen);
            }
        }
    }
}
