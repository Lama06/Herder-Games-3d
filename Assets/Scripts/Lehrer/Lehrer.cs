using HerderGames.AI;
using UnityEngine;
using UnityEngine.AI;

namespace HerderGames.Lehrer
{
    [RequireComponent(typeof(KrankheitsManager))]
    [RequireComponent(typeof(Reputation))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Renderer))]
    [RequireComponent(typeof(AIController))]
    public class Lehrer : MonoBehaviour
    {
        [SerializeField] private string Name;
        
        private KrankheitsManager Krankheit;
        private Reputation Reputation;
        private NavMeshAgent Agent;
        private Renderer Renderer;

        private void Awake()
        {
            Krankheit = GetComponent<KrankheitsManager>();
            Reputation = GetComponent<Reputation>();
            Agent = GetComponent<NavMeshAgent>();
            Renderer = GetComponent<Renderer>();
        }
        
        public KrankheitsManager GetKrankheit()
        {
            return Krankheit;
        }

        public Reputation GetReputation()
        {
            return Reputation;
        }

        public NavMeshAgent GetAgent()
        {
            return Agent;
        }

        public Renderer GetRenderer()
        {
            return Renderer;
        }

        public string GetName()
        {
            return name;
        }
    }
}
