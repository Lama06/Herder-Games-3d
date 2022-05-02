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
    [RequireComponent(typeof(InDerSchuleState))]
    [RequireComponent(typeof(Reputation))]
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
        public InDerSchuleState InSchule { get; private set; }
        public Reputation Reputation { get; private set; }
        public SpracheManager Sprache { get; private set; }
        public AIController AI { get; private set; }
        public InteraktionsMenuFragenManager FragenManager { get; private set; }
        public BrainBase Brain { get; private set; }

        private Vector3 LastPosition;
        
        private void Awake()
        {
            Renderer = GetComponent<Renderer>();
            Agent = GetComponent<NavMeshAgent>();
            InSchule = GetComponent<InDerSchuleState>();
            Reputation = GetComponent<Reputation>();
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
            transform.position = PlayerPrefsUtil.GetVector($"{GetSaveKeyRoot()}.position", transform.position);
        }

        public void SaveData()
        {
            PlayerPrefsUtil.SetVector($"{GetSaveKeyRoot()}.position", LastPosition);
        }

        public void DeleteData()
        {
            PlayerPrefsUtil.DeleteVector($"{GetSaveKeyRoot()}.position");
        }

        public string GetName()
        {
            return Name;
        }

        public string GetId()
        {
            return Id;
        }

        public string GetSaveKeyRoot()
        {
            return $"lehrer.{Id}";
        }
    }
}
