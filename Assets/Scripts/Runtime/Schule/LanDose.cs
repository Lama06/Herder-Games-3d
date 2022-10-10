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
            StartCoroutine(ShowInteraktionsMenuEintrag("Internet sabotieren (MIC)", false, true));
            StartCoroutine(ShowInteraktionsMenuEintrag("Internet reparieren (MIO)", true, false));
        }

        private IEnumerator ShowInteraktionsMenuEintrag(string interaktionsMenuName, bool micBefore, bool lanKabelRequired)
        {
            bool ShouldShow() => Mic == micBefore && PlayerInTrigger != null;

            while (true)
            {
                yield return new WaitUntil(ShouldShow);
                var player = PlayerInTrigger;
                var id = player.InteraktionsMenu.AddEintrag(new InteraktionsMenuEintrag
                {
                    Name = interaktionsMenuName,
                    Callback = _ =>
                    {
                        if (lanKabelRequired && !player.Inventory.RemoveItem(Item.LanKabel))
                        {
                            player.Chat.SendChatMessage("Du brauchst ein Lan Kabel um einen Mic durchzuführen");
                            return;
                        }

                        player.VerbrechenManager.VerbrechenStarten(Zeit, Schwere, () => { Mic = !micBefore; });
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
