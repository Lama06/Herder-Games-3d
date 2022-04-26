using System.Collections;
using HerderGames.Schule;
using HerderGames.Zeit;
using UnityEngine;

namespace HerderGames.Lehrer
{
    [RequireComponent(typeof(Lehrer))]
    public class VergiftungsManager : MonoBehaviour
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private Player.Player Player;
        [SerializeField] private int LaengeDerVergiftung;
        [SerializeField] private int KostenFuerDieSchuleProTagVergiftet;

        private bool Vergiftet;
        public bool Syntome { get; private set; }
        private VergiftbaresEssen GrundDerVergiftung;
        private int TageRemaining;

        private void Start()
        {
            StartCoroutine(ManageVergiftung());
        }

        private IEnumerator ManageVergiftung()
        {
            var currentWochentag = TimeManager.GetCurrentWochentag();
            while (true)
            {
                yield return new WaitUntil(() => currentWochentag != TimeManager.GetCurrentWochentag());
                currentWochentag = TimeManager.GetCurrentWochentag();
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
                    GrundDerVergiftung.Status = VergiftungsStatus.VergiftetBemerkt;
                }
                
                Player.Score.SchadenFuerDieSchule += KostenFuerDieSchuleProTagVergiftet;
            }
        }

        public void Vergiften(VergiftbaresEssen grund)
        {
            Vergiftet = true;
            Syntome = false;
            GrundDerVergiftung = grund;
            TageRemaining = LaengeDerVergiftung;
        }
    }
}