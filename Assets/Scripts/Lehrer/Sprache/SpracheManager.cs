using System;
using System.Collections;
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
        [SerializeField] private float DefaultDelay = 10;

        private Lehrer Lehrer;
        public SaetzeMoeglichkeitenMehrmals SaetzeMoeglichkeiten { get; set; }

        private void Awake()
        {
            Lehrer = GetComponent<Lehrer>();
        }

        private void Start()
        {
            StartCoroutine(SaySaetzeMehrmals());
        }

        private IEnumerator SaySaetzeMehrmals()
        {
            while (true)
            {
                yield return null;

                if (SaetzeMoeglichkeiten == null)
                {
                    continue;
                }

                var moeglichkeiten =
                    ResolveSaetze(SaetzeMoeglichkeiten.MoeglicheSaetze, SaetzeMoeglichkeiten.SharedIds);
                if (moeglichkeiten.Count == 0)
                {
                    continue;
                }

                var satz = moeglichkeiten[Random.Range(0, moeglichkeiten.Count)];

                Say(satz.Text);

                var delay = SaetzeMoeglichkeiten.UseCustomDelay ? SaetzeMoeglichkeiten.CustomDelay : DefaultDelay;
                yield return new WaitForSeconds(delay);
            }
        }

        public void Say(string satz)
        {
            if (!(Vector3.Distance(transform.position, Player.transform.position) < ReichweiteDerStimme))
            {
                return;
            }

            Player.Chat.SendChatMessage(Lehrer, satz);
        }

        public void Say(SaetzeMoeglichkeitenEinmalig source)
        {
            var moeglichkeiten = ResolveSaetze(source.MoeglicheSaetze, source.SharedIds);
            if (moeglichkeiten.Count == 0)
            {
                return;
            }

            var satz = moeglichkeiten[Random.Range(0, moeglichkeiten.Count)];

            Say(satz.Text);
        }

        private List<SatzMoeglichkeit> ResolveSaetze(SatzMoeglichkeit[] moeglicheSaetze, string[] sharedIds)
        {
            var result = new List<SatzMoeglichkeit>();
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

        [Serializable]
        public class SharedSaetze
        {
            public string Id;
            public SatzMoeglichkeit[] Saetze;
        }
    }
}