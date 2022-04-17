using HerderGames.Time;
using UnityEngine;

namespace HerderGames.Player
{
    [RequireComponent(typeof(Chat))]
    [RequireComponent(typeof(InteraktionsMenu))]
    [RequireComponent(typeof(VerbrechenManager))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private TimeManager TimeManager;
        
        private Chat Chat;
        private InteraktionsMenu InteraktionsMenu;
        private VerbrechenManager VerbrechenManager;

        private void Awake()
        {
            Chat = GetComponent<Chat>();
            InteraktionsMenu = GetComponent<InteraktionsMenu>();
            VerbrechenManager = GetComponent<VerbrechenManager>();
        }
        
        public TimeManager GetTimeManager()
        {
            return TimeManager;
        }
        
        public Chat GetChat()
        {
            return Chat;
        }

        public InteraktionsMenu GetInteraktionsMenu()
        {
            return InteraktionsMenu;
        }

        public VerbrechenManager GetVerbrechenManager()
        {
            return VerbrechenManager;
        }
    }
}