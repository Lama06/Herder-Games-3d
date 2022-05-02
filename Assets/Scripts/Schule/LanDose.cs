using System.Collections;
using HerderGames.Player;
using HerderGames.Util;
using UnityEngine;

namespace HerderGames.Schule
{
    public class LanDose : MonoBehaviour, PersistentDataContainer
    {
        [SerializeField] private string Id;
        [SerializeField] private int Zeit;
        [SerializeField] private float Schwere;
        
        public bool Mic { get; set; }
        private Player.Player PlayerInTrigger;

        private void Start()
        {
            StartCoroutine(ShowInteraktionsMenuEintrag("Internet sabotieren (MIC)", false));
            StartCoroutine(ShowInteraktionsMenuEintrag("Internet reparieren (MIO)", true));
        }

        private IEnumerator ShowInteraktionsMenuEintrag(string name, bool micBefore)
        {
            bool ShouldShow() => Mic == micBefore && PlayerInTrigger != null;

            while (true)
            {
                yield return new WaitUntil(ShouldShow);
                var player = PlayerInTrigger;
                var id = player.InteraktionsMenu.AddEintrag(new InteraktionsMenuEintrag
                {
                    Name = name,
                    Callback = _ =>
                    {
                        player.VerbrechenManager.VerbrechenStarten(Zeit, Schwere, () =>
                        {
                            Mic = !micBefore;
                        });
                    }
                });

                yield return new WaitUntil(() => !ShouldShow());
                player.InteraktionsMenu.RemoveEintrag(id);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Player.Player>(out var player))
            {
                PlayerInTrigger = player;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Player.Player>() != null)
            {
                PlayerInTrigger = null;
            }
        }

        private string SaveKeyRoot => $"lan_dose.{Id}";

        public void SaveData()
        {
            PlayerPrefsUtil.SetBool($"{SaveKeyRoot}.mic", Mic);
        }

        public void LoadData()
        {
            Mic = PlayerPrefsUtil.GetBool($"{SaveKeyRoot}.mic", false);
        }

        public void DeleteData()
        {
            PlayerPrefs.DeleteKey($"{SaveKeyRoot}.mic");
        }
    }
}
