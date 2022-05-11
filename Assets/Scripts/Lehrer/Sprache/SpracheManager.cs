using System.Collections;
using UnityEngine;

namespace HerderGames.Lehrer.Sprache
{
    [RequireComponent(typeof(Lehrer))]
    public class SpracheManager : MonoBehaviour
    {
        private const float DefaultDelay = 5;

        [SerializeField] private Player.Player Player;
        [SerializeField] private float ReichweiteDerStimme = 20;

        private Lehrer Lehrer;
        public ISaetzeMoeglichkeitenMehrmals SaetzeMoeglichkeiten { get; set; }

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

                var (satz, delay) = SaetzeMoeglichkeiten.GetNextSatz();
                
                if (satz == null)
                {
                    continue;
                }

                Say(satz);
                
                yield return new WaitForSeconds(delay ?? DefaultDelay);
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

        public void Say(ISaetzeMoeglichkeitenEinmalig source)
        {
            if (source == null)
            {
                return;
            }

            var satz = source.GetSatz();
            if (satz == null)
            {
                return;
            }

            Say(satz);
        }
    }
}
