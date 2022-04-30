using HerderGames.Lehrer.AI;
using HerderGames.Lehrer.Fragen;
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
    [RequireComponent(typeof(InteraktionsMenuFragenManager))]
    [RequireComponent(typeof(BrainBase))]
    public class Lehrer : MonoBehaviour
    {
        [SerializeField] private string Name;
        [SerializeField] private string Id;

        public Renderer Renderer { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        public InDerSchuleState InSchule { get; private set; }
        public Reputation Reputation { get; private set; }
        public SpracheManager Sprache { get; private set; }
        public AIController AI { get; private set; }
        public BrainBase Brain { get; private set; }
        public InteraktionsMenuFragenManager FragenManager { get; private set; }

        private void Awake()
        {
            Renderer = GetComponent<Renderer>();
            Agent = GetComponent<NavMeshAgent>();
            InSchule = GetComponent<InDerSchuleState>();
            Reputation = GetComponent<Reputation>();
            Sprache = GetComponent<SpracheManager>();
            AI = GetComponent<AIController>();
            Brain = GetComponent<BrainBase>();
            FragenManager = GetComponent<InteraktionsMenuFragenManager>();
        }

        public string GetName()
        {
            return Name;
        }

        public string GetId()
        {
            return Id;
        }
    }
}
