using System.Collections;
using HerderGames.Schule;
using HerderGames.Util;
using HerderGames.Zeit;
using UnityEngine;

namespace HerderGames.Lehrer
{
    [RequireComponent(typeof(Lehrer))]
    public class VergiftungsManager : MonoBehaviour, PersistentDataContainer
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private Player.Player Player;
        [SerializeField] private int LaengeDerVergiftung;
        [SerializeField] private int KostenFuerDieSchuleProTagVergiftet;

        public bool Vergiftet { get; private set; }
        public bool Syntome { get; private set; }
        public VergiftungsType VergiftungsType { get; private set; }
        private int TageRemaining;
        private VergiftbaresEssen GrundDerVergiftung;

        private Lehrer Lehrer;

        private void Awake()
        {
            Lehrer = GetComponent<Lehrer>();
        }

        private void Start()
        {
            StartCoroutine(ManageVergiftung());
        }

        public void Vergiften(VergiftbaresEssen grund)
        {
            Vergiftet = true;
            Syntome = false;
            VergiftungsType = grund.VergiftungsTyp;
            GrundDerVergiftung = grund;
            TageRemaining = LaengeDerVergiftung;
        }

        public void Vergiften(VergiftungsType type)
        {
            Vergiftet = true;
            Syntome = false;
            VergiftungsType = type;
            GrundDerVergiftung = null;
            TageRemaining = LaengeDerVergiftung;
        }

        private IEnumerator ManageVergiftung()
        {
            var currentWochentag = TimeManager.CurrentWochentag;
            while (true)
            {
                yield return new WaitUntil(() => currentWochentag != TimeManager.CurrentWochentag);
                currentWochentag = TimeManager.CurrentWochentag;
                // Wird immer am Anfang eines neuen Wochentages ausgeführt

                if (!Vergiftet)
                {
                    continue;
                }

                if (TageRemaining <= 0)
                {
                    Vergiftet = false;
                    Syntome = false;
                    GrundDerVergiftung = null;
                    TageRemaining = 0;
                    continue;
                }

                TageRemaining--;

                if (!Syntome)
                {
                    Syntome = true;
                    if (GrundDerVergiftung != null) // Kann null sein, weil es nicht gespeichert wird
                    {
                        GrundDerVergiftung.Bemerken();
                    }
                }

                Player.Score.SchadenFuerDieSchule += KostenFuerDieSchuleProTagVergiftet;
            }
        }

        private string GetSaveKeyRoot()
        {
            return $"{Lehrer.GetSaveKeyRoot()}.vergiftung";
        }

        public void SaveData()
        {
            PlayerPrefsUtil.SetBool($"{GetSaveKeyRoot()}.vergiftet", Vergiftet);
            PlayerPrefsUtil.SetBool($"{GetSaveKeyRoot()}.syntome", Syntome);
            PlayerPrefs.SetInt($"{GetSaveKeyRoot()}.vergiftungs_type", (int) VergiftungsType);
            PlayerPrefs.SetInt($"{GetSaveKeyRoot()}.tage_remaining", TageRemaining);
        }

        public void LoadData()
        {
            Vergiftet = PlayerPrefsUtil.GetBool($"{GetSaveKeyRoot()}.vergiftet", false);
            Syntome = PlayerPrefsUtil.GetBool($"{GetSaveKeyRoot()}.syntome", false);
            VergiftungsType = (VergiftungsType) PlayerPrefs.GetInt($"{GetSaveKeyRoot()}.vergiftungs_type", 0);
            TageRemaining = PlayerPrefs.GetInt($"{GetSaveKeyRoot()}.tage_remaining", 0);
        }

        public void DeleteData()
        {
            PlayerPrefs.DeleteKey($"{GetSaveKeyRoot()}.vergiftet");
            PlayerPrefs.DeleteKey($"{GetSaveKeyRoot()}.syntome");
            PlayerPrefs.DeleteKey($"{GetSaveKeyRoot()}.vergiftungs_type");
            PlayerPrefs.DeleteKey($"{GetSaveKeyRoot()}.tage_remaining");
        }
    }
}
