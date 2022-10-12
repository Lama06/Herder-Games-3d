using System;
using System.Collections;
using HerderGames.Util;
using UnityEngine;

namespace HerderGames.Lehrer.Sprache
{
    public class SpracheManager
    {
        private const float DefaultDelay = 5;

        private readonly Lehrer Lehrer;
        private bool _Enabled;
        private Coroutine SaySaetzeMehrmalsCoroutine;
        public ISaetzeMoeglichkeitenMehrmals SaetzeMoeglichkeiten { get; set; }

        public SpracheManager(Lehrer lehrer)
        {
            Lehrer = lehrer;
        }

        public bool Enabled
        {
            get => _Enabled;
            set
            {
                if (_Enabled == value)
                {
                    return;
                }

                _Enabled = value;

                if (_Enabled)
                {
                    SaySaetzeMehrmalsCoroutine = Lehrer.StartCoroutine(SaySaetzeMehrmals());
                }
                else
                {
                    Lehrer.StopCoroutine(SaySaetzeMehrmalsCoroutine);
                }
            }
        }

        public void Awake()
        {
            Enabled = true;
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

                var (satz, delay) = SaetzeMoeglichkeiten.NextSatz;

                if (satz == null)
                {
                    continue;
                }

                Say(satz);

                foreach (var _ in IteratorUtil.WaitForSeconds(delay ?? DefaultDelay))
                {
                    yield return null;
                }
            }
        }

        public void Say(string satz)
        {
            if (!(Vector3.Distance(Lehrer.transform.position, Lehrer.Player.transform.position) < Lehrer.Configuration.ReichweiteDerStimme))
            {
                return;
            }

            Lehrer.Player.Chat.SendChatMessage(Lehrer, satz);
        }

        public void Say(ISaetzeMoeglichkeitenEinmalig source)
        {
            if (source == null)
            {
                return;
            }

            var satz = source.Satz;
            if (satz == null)
            {
                return;
            }

            Say(satz);
        }
    }
}