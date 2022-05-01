using HerderGames.Util;
using UnityEngine;

namespace HerderGames.Player
{
    public class GeldManager : MonoBehaviour, PersistentDataContainer
    {
        public int Geld;

        public bool Pay(int amount)
        {
            if (amount > Geld)
            {
                return false;
            }

            Geld -= amount;
            return true;
        }

        private const string SaveKey = "player.geld";

        public void SaveData()
        {
            PlayerPrefs.SetInt(SaveKey, Geld);
        }

        public void LoadData()
        {
            Geld = PlayerPrefs.GetInt(SaveKey);
        }

        public void DeleteData()
        {
            PlayerPrefs.DeleteKey(SaveKey);
        }
    }
}