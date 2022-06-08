using HerderGames.Util;
using UnityEngine;

namespace HerderGames.Lehrer
{
    [RequireComponent(typeof(Lehrer))]
    public class Reputation : MonoBehaviour, PersistentDataContainer
    {
        private Lehrer Lehrer;
        public float ReputationsWert { get; set; }

        private void Awake()
        {
            Lehrer = GetComponent<Lehrer>();
        }

        public void AddReputation(float amount)
        {
            ReputationsWert += amount;
        }

        public bool ShouldGoToSchulleitung()
        {
            return ReputationsWert <= -1f;
        }

        public void ResetAfterMelden()
        {
            ReputationsWert = -0.5f;
        }

        private string GetSaveKey()
        {
            return $"{Lehrer.GetSaveKeyRoot()}.reputation";
        }
        
        public void SaveData()
        {
            PlayerPrefs.SetFloat(GetSaveKey(), ReputationsWert);
        }

        public void LoadData()
        {
            ReputationsWert = PlayerPrefs.GetFloat(GetSaveKey());
        }

        public void DeleteData()
        {
            PlayerPrefs.DeleteKey(GetSaveKey());
        }
    }
}