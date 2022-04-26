using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HerderGames.Lehrer.Sprache
{
    [RequireComponent(typeof(Lehrer))]
    public class SpracheManager : MonoBehaviour
    {
        private const float DefaultDelay = 10;
        
        [SerializeField] private Player.Player Player;
        [SerializeField] private float ReichweiteDerStimme = 20;

        private Lehrer Lehrer;
        private List<string> RemainingSaetze;
        private SaetzeMoeglichkeitenMehrmals _SaetzeMoeglichkeiten;
        public SaetzeMoeglichkeitenMehrmals SaetzeMoeglichkeiten
        {
            set
            {
                _SaetzeMoeglichkeiten = value;
                if (value != null)
                {
                    RemainingSaetze = new List<string>(value.MoeglicheSaetze);
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
                    RemainingSaetze = new List<string>(_SaetzeMoeglichkeiten.MoeglicheSaetze);
                }

                if (RemainingSaetze.Count == 0)
                {
                    continue;
                }

                var satz = RemainingSaetze[Random.Range(0, RemainingSaetze.Count)];
                RemainingSaetze.Remove(satz);

                Say(satz);

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
            if (source == null)
            {
                return;
            }
            
            var moeglichkeiten = source.MoeglicheSaetze;
            if (moeglichkeiten.Count == 0)
            {
                return;
            }

            var satz = moeglichkeiten[Random.Range(0, moeglichkeiten.Count)];

            Say(satz);
        }
    }
}