using System.Collections;
using HerderGames.Schule;
using HerderGames.Util;
using HerderGames.Zeit;
using UnityEngine;

namespace HerderGames.Lehrer
{
    public class VergiftungsManager
    {
        private readonly Lehrer Lehrer;
        public bool Vergiftet { get; private set; }
        public bool Syntome { get; private set; }
        public VergiftungsType VergiftungsType { get; private set; }
        private int TageRemaining;
        private VergiftbaresEssen GrundDerVergiftung;

        public VergiftungsManager(Lehrer lehrer)
        {
            Lehrer = lehrer;
        }

        public void Awake()
        {
            Lehrer.StartCoroutine(ManageVergiftung());
        }

        public void Vergiften(VergiftbaresEssen grund)
        {
            Vergiftet = true;
            Syntome = false;
            VergiftungsType = grund.VergiftungsTyp;
            GrundDerVergiftung = grund;
            TageRemaining = Lehrer.Configuration.LaengeVergiftung;
        }

        public void Vergiften(VergiftungsType type)
        {
            Vergiftet = true;
            Syntome = false;
            VergiftungsType = type;
            GrundDerVergiftung = null;
            TageRemaining = Lehrer.Configuration.LaengeVergiftung;
        }

        private IEnumerator ManageVergiftung()
        {
            var currentWochentag = Lehrer.TimeManager.CurrentWochentag;
            while (true)
            {
                yield return new WaitUntil(() => currentWochentag != Lehrer.TimeManager.CurrentWochentag);
                currentWochentag = Lehrer.TimeManager.CurrentWochentag;
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

                Lehrer.Player.Score.SchadenFuerDieSchule += Lehrer.Configuration.KostenProTagVergiftet;
            }
        }

        private string SaveKeyRoot => $"{Lehrer.SaveKeyRoot}.vergiftung";

        public void SaveData()
        {
            PlayerPrefsUtil.SetBool($"{SaveKeyRoot}.vergiftet", Vergiftet);
            PlayerPrefsUtil.SetBool($"{SaveKeyRoot}.syntome", Syntome);
            PlayerPrefs.SetInt($"{SaveKeyRoot}.vergiftungs_type", (int) VergiftungsType);
            PlayerPrefs.SetInt($"{SaveKeyRoot}.tage_remaining", TageRemaining);
        }

        public void LoadData()
        {
            Vergiftet = PlayerPrefsUtil.GetBool($"{SaveKeyRoot}.vergiftet", false);
            Syntome = PlayerPrefsUtil.GetBool($"{SaveKeyRoot}.syntome", false);
            VergiftungsType = (VergiftungsType) PlayerPrefs.GetInt($"{SaveKeyRoot}.vergiftungs_type", 0);
            TageRemaining = PlayerPrefs.GetInt($"{SaveKeyRoot}.tage_remaining", 0);
        }

        public void DeleteData()
        {
            PlayerPrefs.DeleteKey($"{SaveKeyRoot}.vergiftet");
            PlayerPrefs.DeleteKey($"{SaveKeyRoot}.syntome");
            PlayerPrefs.DeleteKey($"{SaveKeyRoot}.vergiftungs_type");
            PlayerPrefs.DeleteKey($"{SaveKeyRoot}.tage_remaining");
        }
    }
}
