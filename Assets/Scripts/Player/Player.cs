using System;
using HerderGames.Util;
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
    public class Player : MonoBehaviour, PersistentDataContainer
    {
        public Chat Chat { get; private set; }
        public InteraktionsMenu InteraktionsMenu { get; private set; }
        public VerbrechenManager VerbrechenManager { get; private set; }
        public Verwarnungen Verwarnungen { get; private set; }
        public Stundenplan Stundenplan { get; private set; }
        public Score Score { get; private set; }
        public GeldManager GeldManager { get; private set; }

        private Vector3 LastPosition;

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

        private void Update()
        {
            LastPosition = transform.position;
        }

        public void LoadData()
        {
            transform.position = PlayerPrefsUtil.GetVector("player.position", transform.position);
        }

        public void SaveData()
        {
            PlayerPrefsUtil.SetVector("player.position", LastPosition);
            // Hier kann nicht direkt transform.position verwendet werden, weil es sein kann, dass SaveData() nach dem
            // deaktivieren des Objektes aufgerufen wird, was zu einem Error führen würde
        }

        public void DeleteData()
        {
            PlayerPrefs.DeleteKey("player.position");
        }
    }
}