using System;
using HerderGames.Lehrer.AI;
using HerderGames.Lehrer.Fragen;
using HerderGames.Lehrer.Sprache;
using HerderGames.Util;
using UnityEngine;
using UnityEngine.AI;

namespace HerderGames.Lehrer
{
    [RequireComponent(typeof(Renderer))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(InDerSchuleStatus))]
    [RequireComponent(typeof(Reputation))]
    [RequireComponent(typeof(VergiftungsManager))]
    [RequireComponent(typeof(VisionSensor))]
    [RequireComponent(typeof(SpracheManager))]
    [RequireComponent(typeof(AIController))]
    [RequireComponent(typeof(InteraktionsMenuFragenManager))]
    [RequireComponent(typeof(BrainBase))]
    public class Lehrer : MonoBehaviour, PersistentDataContainer
    {
        [SerializeField] private string Name;
        [SerializeField] private string Id;

        public Renderer Renderer { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        public InDerSchuleStatus InSchule { get; private set; }
        public Reputation Reputation { get; private set; }
        public VergiftungsManager Vergiftung { get; private set; }
        public VisionSensor Vision { get; private set; }
        public SpracheManager Sprache { get; private set; }
        public AIController AI { get; private set; }
        public InteraktionsMenuFragenManager FragenManager { get; private set; }
        public BrainBase Brain { get; private set; }

        private Vector3 LastPosition;
        
        private void Awake()
        {
            Renderer = GetComponent<Renderer>();
            Agent = GetComponent<NavMeshAgent>();
            InSchule = GetComponent<InDerSchuleStatus>();
            Reputation = GetComponent<Reputation>();
            Vergiftung = GetComponent<VergiftungsManager>();
            Vision = GetComponent<VisionSensor>();
            Sprache = GetComponent<SpracheManager>();
            AI = GetComponent<AIController>();
            FragenManager = GetComponent<InteraktionsMenuFragenManager>();
            Brain = GetComponent<BrainBase>();
        }

        private void Update()
        {
            LastPosition = transform.position;
        }

        public void LoadData()
        {
            transform.position = PlayerPrefsUtil.GetVector($"{SaveKeyRoot}.position", transform.position);
        }

        public void SaveData()
        {
            PlayerPrefsUtil.SetVector($"{SaveKeyRoot}.position", LastPosition);
        }

        public void DeleteData()
        {
            PlayerPrefsUtil.DeleteVector($"{SaveKeyRoot}.position");
        }

        public string GetName()
        {
            return Name;
        }

        public string GetId()
        {
            return Id;
        }

        public string SaveKeyRoot => $"lehrer.{Id}";
    }
}
