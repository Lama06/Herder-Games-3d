using HerderGames.Util;
using UnityEngine;

namespace HerderGames.Lehrer
{
    [RequireComponent(typeof(Lehrer))]
    public class Reputation : MonoBehaviour, PersistentDataContainer
    {
        [SerializeField] private float MisstrauensFaktor = 1;
        [SerializeField] private float GutmuetigkeitsFaktor = 1;

        private Lehrer Lehrer;
        public float ReputationsWert { get; set; }

        private void Awake()
        {
            Lehrer = GetComponent<Lehrer>();
        }

        public void AddReputation(float amount)
        {
            switch (amount)
            {
                case < 0f:
                    amount *= MisstrauensFaktor;
                    break;
                case > 0f:
                    amount *= GutmuetigkeitsFaktor;
                    break;
            }

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
            return $"lehrer.{Lehrer.GetId()}.reputation";
        }
        
        public void SaveData()
        {
            PlayerPrefs.SetFloat(GetSaveKey(), ReputationsWert);
        }

        public void LoadData()
        {
            ReputationsWert = PlayerPrefs.GetFloat(GetSaveKey());
        }

        public void ResetData()
        {
            ReputationsWert = 0f;
        }
    }
}