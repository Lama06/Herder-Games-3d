using System.Collections;
using HerderGames.Time;
using UnityEngine;
using UnityEngine.AI;

namespace HerderGames.AI.Goals
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Renderer))]
    public class SchuleBetretenVerlassenGoal : GoalBase
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private Transform Eingang;
        [SerializeField] private Transform Ausgang;
        [SerializeField] private WoechentlicheZeitspannen ZeitInDerSchule;
        [SerializeField] private bool InDerSchule = true;

        private NavMeshAgent Agent;
        private Renderer Renderer;
        
        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Renderer = GetComponent<Renderer>();
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return !ShouldBeInSchool() || InDerSchule && !ShouldBeInSchool() || !InDerSchule && ShouldBeInSchool();
        }

        private bool ShouldBeInSchool()
        {
            return ZeitInDerSchule.IsInside(TimeManager.GetCurrentWochentag(), TimeManager.GetCurrentTime());
        }

        public override void OnStarted()
        {
            Debug.Log("Started");
        }

        public override IEnumerator Execute()
        {
            while (true)
            {
                if (InDerSchule && !ShouldBeInSchool())
                {
                    Agent.destination = Ausgang.position;
                    Debug.Log("Losgehen");
                    yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Agent);
                    SetVisible(false);
                    InDerSchule = false;
                }

                if (!InDerSchule && ShouldBeInSchool())
                {
                    Agent.Warp(Eingang.position);
                    SetVisible(true);
                    InDerSchule = true;
                }
                yield return null;
            }
        }

        private void SetVisible(bool visible)
        {
            Renderer.enabled = visible;
        }
    }
}