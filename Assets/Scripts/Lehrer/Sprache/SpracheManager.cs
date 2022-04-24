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
        [SerializeField] private float ReichweiteDerStimme = 20;
        [SerializeField] private float DefaultDelay = 10;

        private Lehrer Lehrer;
        
        private List<SatzMoeglichkeit> RemainingSaetze;
        private SaetzeMoeglichkeitenMehrmals _SaetzeMoeglichkeiten;
        public SaetzeMoeglichkeitenMehrmals SaetzeMoeglichkeiten
        {
            set
            {
                _SaetzeMoeglichkeiten = value;
                if (value != null)
                {
                    RemainingSaetze = ResolveSaetze(value.MoeglicheSaetze, value.SharedIds);
                }
            }
        }

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

                if (_SaetzeMoeglichkeiten == null)
                {
                    continue;
                }

                if (RemainingSaetze.Count == 0)
                {
                    RemainingSaetze = ResolveSaetze(_SaetzeMoeglichkeiten.MoeglicheSaetze, _SaetzeMoeglichkeiten.SharedIds);
                }

                if (RemainingSaetze.Count == 0)
                {
                    continue;
                }

                var satz = RemainingSaetze[Random.Range(0, RemainingSaetze.Count)];
                RemainingSaetze.Remove(satz);

                Say(satz.Text);

                var delay = _SaetzeMoeglichkeiten.UseCustomDelay ? _SaetzeMoeglichkeiten.CustomDelay : DefaultDelay;
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

        private List<SatzMoeglichkeit> ResolveSaetze(IEnumerable<SatzMoeglichkeit> moeglicheSaetze, IEnumerable<string> sharedIds)
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