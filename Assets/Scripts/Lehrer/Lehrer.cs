using HerderGames.Lehrer.AI;
using HerderGames.Lehrer.Sprache;
using UnityEngine;
using UnityEngine.AI;

namespace HerderGames.Lehrer
{
    [RequireComponent(typeof(Renderer))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(InDerSchuleState))]
    [RequireComponent(typeof(Reputation))]
    [RequireComponent(typeof(SpracheManager))]
    [RequireComponent(typeof(AIController))]
    public class Lehrer : MonoBehaviour
    {
        [SerializeField] private string Name;

        public Renderer Renderer { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        public InDerSchuleState InSchule { get; private set; }
        public Reputation Reputation { get; private set; }
        public SpracheManager Sprache { get; private set; }
        public AIController AI { get; private set; }

        private void Awake()
        {
            Renderer = GetComponent<Renderer>();
            Agent = GetComponent<NavMeshAgent>();
            InSchule = GetComponent<InDerSchuleState>();
            Reputation = GetComponent<Reputation>();
            Sprache = GetComponent<SpracheManager>();
            AI = GetComponent<AIController>();
        }

        public string GetName()
        {
            return Name;
        }
    }
}
