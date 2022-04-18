using UnityEngine;

namespace HerderGames.Player
{
    [RequireComponent(typeof(Chat))]
    [RequireComponent(typeof(InteraktionsMenu))]
    [RequireComponent(typeof(VerbrechenManager))]
    [RequireComponent(typeof(Verwarnungen))]
    public class Player : MonoBehaviour
    {
        private Chat Chat;
        private InteraktionsMenu InteraktionsMenu;
        private VerbrechenManager VerbrechenManager;
        private Verwarnungen Verwarnungen;

        private void Awake()
        {
            Chat = GetComponent<Chat>();
            InteraktionsMenu = GetComponent<InteraktionsMenu>();
            VerbrechenManager = GetComponent<VerbrechenManager>();
            Verwarnungen = GetComponent<Verwarnungen>();
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

        public Verwarnungen GetVerwarnungen()
        {
            return Verwarnungen;
        }
    }
}