using System;
using HerderGames.Lehrer.AI;
using HerderGames.Lehrer.Animation;
using HerderGames.Lehrer.Fragen;
using HerderGames.Lehrer.Sprache;
using HerderGames.Schule;
using HerderGames.Util;
using HerderGames.Zeit;
using UnityEngine;
using UnityEngine.AI;

namespace HerderGames.Lehrer
{
    public abstract class Lehrer : MonoBehaviour, PersistentDataContainer
    {
        // Inspector fields
        [SerializeField] [InspectorName("Player")]
        private Player.Player _Player;

        [SerializeField] [InspectorName("Time Manager")]
        private TimeManager _TimeManager;

        [SerializeField] [InspectorName("Alarm Manager")]
        private AlarmManager _AlarmManager;

        [SerializeField] [InspectorName("Internet Manager")]
        private InternetManager _InternetManager;

        [SerializeField] [InspectorName("Eye Location")]
        private Transform _EyeLocation;

        [SerializeField] private Avatar Avatar;

        [SerializeField] private RuntimeAnimatorController AnimationController;

        // Inspector Field Accessors
        public Player.Player Player => _Player;
        public TimeManager TimeManager => _TimeManager;
        public AlarmManager AlarmManager => _AlarmManager;
        public InternetManager Internet => _InternetManager;
        public Transform EyeLocation => _EyeLocation;

        // Components
        public NavMeshAgent Agent { get; private set; }
        public Animator Animator { get; private set; }

        // Managers
        public AnimationManager AnimationManager { get; private set; }
        public Reputation Reputation { get; private set; }
        public VergiftungsManager Vergiftung { get; private set; }
        public VisionSensor Vision { get; private set; }
        public SpracheManager Sprache { get; private set; }
        public AIController AI { get; private set; }
        public InteraktionsMenuFragenManager FragenManager { get; private set; }
        public AnimationNavAgentSpeedSyncer AnimationNavAgentSpeedSyncer { get; private set; }

        // Other Fields
        public LehrerConfiguration Configuration { get; private set; }
        public string SaveKeyRoot => $"lehrer.{Configuration.Id}";
        private Vector3 LastPosition;

        // Abstract Methods
        protected abstract LehrerConfiguration CreateConfiguration();

        protected abstract void RegisterFragen(InteraktionsMenuFragenManager fragen);

        protected abstract void RegisterGoals(AIController ai);

        // Unity Event Functions
        private void Awake()
        {
            Configuration = CreateConfiguration();

            // Init Unity Components
            Agent = gameObject.AddComponent<NavMeshAgent>();
            Agent.enabled = false;

            Animator = gameObject.AddComponent<Animator>();
            Animator.avatar = Avatar;
            Animator.runtimeAnimatorController = AnimationController;
            Animator.applyRootMotion = false;

            // Create Managers
            AnimationManager = new AnimationManager(this);
            Reputation = new Reputation(this);
            Vergiftung = new VergiftungsManager(this);
            Vision = new VisionSensor(this);
            Sprache = new SpracheManager(this);
            AI = new AIController(this);
            FragenManager = new InteraktionsMenuFragenManager(this);
            AnimationNavAgentSpeedSyncer = new AnimationNavAgentSpeedSyncer(this);

            // Awake Managers
            Vergiftung.Awake();
            Sprache.Awake();

            // Register
            RegisterGoals(AI);
            RegisterFragen(FragenManager);
        }

        private void Update()
        {
            LastPosition = transform.position;

            // Update Managers
            AnimationManager.Update();
            AI.Update();
            FragenManager.Update();
            AnimationNavAgentSpeedSyncer.Update();
        }

        // Persistent Data Container

        public void LoadData()
        {
            transform.position = PlayerPrefsUtil.GetVector($"{SaveKeyRoot}.position", transform.position);
            
            Reputation.LoadData();
            Vergiftung.LoadData();
        }

        public void SaveData()
        {
            PlayerPrefsUtil.SetVector($"{SaveKeyRoot}.position", LastPosition);
            
            Reputation.SaveData();
            Vergiftung.SaveData();
        }

        public void DeleteData()
        {
            PlayerPrefsUtil.DeleteVector($"{SaveKeyRoot}.position");
            
            Reputation.DeleteData();
            Vergiftung.DeleteData();
        }
    }
}