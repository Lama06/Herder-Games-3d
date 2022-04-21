using HerderGames.AI;
using HerderGames.Lehrer.Sprache;
using UnityEngine;
using UnityEngine.AI;

namespace HerderGames.Lehrer
{
    [RequireComponent(typeof(VergiftungsManager))]
    [RequireComponent(typeof(Reputation))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Renderer))]
    [RequireComponent(typeof(SpracheManager))]
    [RequireComponent(typeof(AIController))]
    public class Lehrer : MonoBehaviour
    {
        [SerializeField] private string Name;
        
        public VergiftungsManager Vergiftung { get; private set; }
        public Reputation Reputation { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        public Renderer Renderer { get; private set; }
        public SpracheManager Sprache { get; private set; }
        public AIController AI { get; private set; }

        private void Awake()
        {
            Vergiftung = GetComponent<VergiftungsManager>();
            Reputation = GetComponent<Reputation>();
            Agent = GetComponent<NavMeshAgent>();
            Renderer = GetComponent<Renderer>();
            Sprache = GetComponent<SpracheManager>();
            AI = GetComponent<AIController>();
        }

        public string GetName()
        {
            return Name;
        }
    }
}
