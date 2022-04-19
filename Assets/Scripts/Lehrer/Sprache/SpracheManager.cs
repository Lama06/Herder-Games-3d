using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HerderGames.Lehrer.Sprache
{
    [RequireComponent(typeof(Lehrer))]
    public class SpracheManager : MonoBehaviour
    {
        [SerializeField] private Player.Player Player;
        [SerializeField] private SharedSaetze[] Shared;
        [SerializeField] private float TimeBetweemRandomSentences;
        [SerializeField] private float ReichweiteDerStimme = 10;

        private Lehrer Lehrer;
        private Saetze Source;
        private float TimeSinceLastSatz;

        private void Awake()
        {
            Lehrer = GetComponent<Lehrer>();
        }

        private void Update()
        {
            TimeSinceLastSatz += UnityEngine.Time.deltaTime;
            if (TimeSinceLastSatz >= TimeBetweemRandomSentences && Source != null)
            {
                var moeglichkeiten = Source.Resolve(this);
                Say(moeglichkeiten);
            }
        }

        private void Say(List<Satz> moeglichkeiten)
        {
            if (moeglichkeiten.Count == 0)
            {
                return;
            }
            
            if (!(Vector3.Distance(transform.position, Player.transform.position) < ReichweiteDerStimme))
            {
                return;
            }

            var msg = moeglichkeiten[Random.Range(0, moeglichkeiten.Count)];
            
            Player.Chat.SendChatMessage(Lehrer, msg.text);
            TimeSinceLastSatz = 0;
        }

        public void SayRandomNow(Saetze source)
        {
            var moeglichkeiten = source.Resolve(this);
            Say(moeglichkeiten);
        }

        public void SetSatzSource(Saetze soruce)
        {
            Source = soruce;
        }

        public Satz[] GetShared(string id)
        {
            foreach (var shared in Shared)
            {
                if (shared.Id == id)
                {
                    return shared.Saetze;
                }
            }

            return null;
        }
        
        [System.Serializable]
        public class SharedSaetze
        {
            public string Id;
            public Satz[] Saetze;
        }
    }
}