using HerderGames.Player;
using HerderGames.Util;
using UnityEngine;

namespace HerderGames.Schule
{
    public class EntfernenVerbrechen : MonoBehaviour, PersistentDataContainer
    {
        [SerializeField] private string Id;
        [SerializeField] private string InteraktionsMenuName;
        [SerializeField] private int TimeRequired;
        [SerializeField] private float Schwere;
        [SerializeField] private int SchadenFuerDieSchule;
        [SerializeField] private int GeldGewinn;
        [SerializeField] private bool HatItemGewinn;
        [SerializeField] private Item ItemGewinn;

        private bool Active;
        private int InteraktionsMenuId;

        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<Player.Player>();
            if (player == null)
            {
                return;
            }

            InteraktionsMenuId = player.InteraktionsMenu.AddEintrag(new InteraktionsMenuEintrag
            {
                Name = InteraktionsMenuName,
                Callback = id =>
                {
                    player.VerbrechenManager.VerbrechenStarten(TimeRequired, Schwere, () =>
                    {
                        player.Score.SchadenFuerDieSchule += SchadenFuerDieSchule;
                        player.GeldManager.Geld += GeldGewinn;

                        if (HatItemGewinn)
                        {
                            player.Inventory.AddItem(ItemGewinn);
                        }
                        
                        gameObject.SetActive(false);
                        Active = false;
                    });
                    player.InteraktionsMenu.RemoveEintrag(id);
                }
            });
        }

        private void OnTriggerExit(Collider other)
        {
            var player = other.GetComponent<Player.Player>();
            if (player == null)
            {
                return;
            }
            
            player.InteraktionsMenu.RemoveEintrag(InteraktionsMenuId);
        }

        private string SaveKey => $"entfernbarer_gegenstand.{Id}";
        
        public void LoadData()
        {
            Active = PlayerPrefsUtil.GetBool($"{SaveKey}.active", true);
            if (!Active)
            {
                gameObject.SetActive(false);
            }
        }

        public void SaveData()
        {
            PlayerPrefsUtil.SetBool($"{SaveKey}.active", Active);
        }

        public void DeleteData()
        {
            PlayerPrefs.DeleteKey($"{SaveKey}.active");
        }
    }
}
