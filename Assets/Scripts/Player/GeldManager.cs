using HerderGames.Util;
using UnityEngine;

namespace HerderGames.Player
{
    [RequireComponent(typeof(Player))]
    public class GeldManager : MonoBehaviour, PersistentDataContainer
    {
        [SerializeField] private int StartGeld;
        public int Geld { get; set; }

        public bool Pay(int amount)
        {
            if (!CanPay(amount))
            {
                return false;
            }

            Geld -= amount;
            return true;
        }

        public bool CanPay(int amount)
        {
            return Geld >= amount;
        }

        private const string SaveKey = "player.geld";

        public void SaveData()
        {
            PlayerPrefs.SetInt(SaveKey, Geld);
        }

        public void LoadData()
        {
            Geld = PlayerPrefs.GetInt(SaveKey, StartGeld);
        }

        public void DeleteData()
        {
            PlayerPrefs.DeleteKey(SaveKey);
        }
    }
}