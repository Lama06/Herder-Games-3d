using HerderGames.Util;
using UnityEngine;

namespace HerderGames.Lehrer
{
    public class Reputation : PersistentDataContainer
    {
        public float ReputationsWert { get; set; }
        private readonly Lehrer Lehrer;

        public Reputation(Lehrer lehrer)
        {
            Lehrer = lehrer;
        }
        
        public void AddReputation(float amount)
        {
            ReputationsWert += amount;
        }

        public bool ShouldGoToSchulleitung => ReputationsWert <= -1f;

        public void ResetAfterMelden()
        {
            ReputationsWert = -0.5f;
        }

        private string SaveKey => $"{Lehrer.SaveKeyRoot}.reputation";

        public void SaveData()
        {
            PlayerPrefs.SetFloat(SaveKey, ReputationsWert);
        }

        public void LoadData()
        {
            ReputationsWert = PlayerPrefs.GetFloat(SaveKey);
        }

        public void DeleteData()
        {
            PlayerPrefs.DeleteKey(SaveKey);
        }
    }
}