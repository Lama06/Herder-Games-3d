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
        [SerializeField] private float ReichweiteDerStimme = 10;
        [SerializeField] private int DefaultDelay = 10;

        private Lehrer Lehrer;
        private SaetzeMoeglichkeitenMehrmals Source;
        private float TimeSinceLastSatz;

        private void Awake()
        {
            Lehrer = GetComponent<Lehrer>();
        }

        private void Update()
        {
            TimeSinceLastSatz += UnityEngine.Time.deltaTime;
            if (Source != null && TimeSinceLastSatz >= (Source.UseDefaultDelay ? DefaultDelay : Source.CustomDelay))
            {
                var moeglichkeiten = ResolveSaetze(Source.MoeglicheSaetze, Source.SharedIds);
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

        public void SayRandomNow(SaetzeMoeglichkeitenEinmalig source)
        {
            var moeglichkeiten = ResolveSaetze(source.MoeglicheSaetze, source.SharedIds);
            Say(moeglichkeiten);
        }

        public void SetSatzSource(SaetzeMoeglichkeitenMehrmals soruce)
        {
            Source = soruce;
        }
        
        public List<Satz> ResolveSaetze(Satz[] moeglicheSaetze, string[] sharedIds)
        {
            var result = new List<Satz>();
            result.AddRange(moeglicheSaetze);

            foreach (var sharedId in sharedIds)
            {
                foreach (var sharedSaetze in Shared)
                {
                    if (sharedSaetze.Id == sharedId)
                    {
                        result.AddRange(sharedSaetze.Saetze);
                    }
                }
            }

            return result;
        }

        [System.Serializable]
        public class SharedSaetze
        {
            public string Id;
            public Satz[] Saetze;
        }
    }
}