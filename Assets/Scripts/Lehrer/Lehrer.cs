using HerderGames.AI;
using HerderGames.Lehrer.Sprache;
using UnityEngine;
using UnityEngine.AI;

namespace HerderGames.Lehrer
{
    [RequireComponent(typeof(KrankheitsManager))]
    [RequireComponent(typeof(Reputation))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Renderer))]
    [RequireComponent(typeof(SpracheManager))]
    [RequireComponent(typeof(AIController))]
    public class Lehrer : MonoBehaviour
    {
        [SerializeField] private string Name;
        
        public KrankheitsManager Krankheit { get; private set; }
        public Reputation Reputation { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        public Renderer Renderer { get; private set; }
        public SpracheManager Sprache { get; private set; }

        private void Awake()
        {
            Krankheit = GetComponent<KrankheitsManager>();
            Reputation = GetComponent<Reputation>();
            Agent = GetComponent<NavMeshAgent>();
            Renderer = GetComponent<Renderer>();
            Sprache = GetComponent<SpracheManager>();
        }

        public string GetName()
        {
            return Name;
        }
    }
}
