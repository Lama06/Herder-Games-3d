using UnityEngine;

namespace HerderGames.Player
{
    [RequireComponent(typeof(Chat))]
    [RequireComponent(typeof(InteraktionsMenu))]
    [RequireComponent(typeof(VerbrechenManager))]
    [RequireComponent(typeof(Verwarnungen))]
    [RequireComponent(typeof(Stundenplan))]
    [RequireComponent(typeof(Score))]
    [RequireComponent(typeof(GeldManager))]
    public class Player : MonoBehaviour
    {
        public Chat Chat { get; private set; }
        public InteraktionsMenu InteraktionsMenu { get; private set; }
        public VerbrechenManager VerbrechenManager { get; private set; }
        public Verwarnungen Verwarnungen { get; private set; }
        public Stundenplan Stundenplan { get; private set; }
        public Score Score { get; private set; }
        public GeldManager GeldManager { get; private set; }
        
        private void Awake()
        {
            Chat = GetComponent<Chat>();
            InteraktionsMenu = GetComponent<InteraktionsMenu>();
            VerbrechenManager = GetComponent<VerbrechenManager>();
            Verwarnungen = GetComponent<Verwarnungen>();
            Stundenplan = GetComponent<Stundenplan>();
            Score = GetComponent<Score>();
            GeldManager = GetComponent<GeldManager>();
        }
    }
}