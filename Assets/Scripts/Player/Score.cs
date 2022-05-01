using HerderGames.Util;
using UnityEngine;

namespace HerderGames.Player
{
    public class Score : MonoBehaviour, PersistentDataContainer
    {
        public int SchadenFuerDieSchule;

        private const string SaveKey = "player.schaden";

        public void SaveData()
        {
            PlayerPrefs.SetInt(SaveKey, SchadenFuerDieSchule);
        }

        public void LoadData()
        {
            SchadenFuerDieSchule = PlayerPrefs.GetInt(SaveKey);
        }

        public void DeleteData()
        {
            PlayerPrefs.DeleteKey(SaveKey);
        }
    }
}
