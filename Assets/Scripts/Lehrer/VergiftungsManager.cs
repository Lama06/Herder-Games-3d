using HerderGames.Schule;
using HerderGames.Time;
using UnityEngine;

namespace HerderGames.Lehrer
{
    [RequireComponent(typeof(Lehrer))]
    public class VergiftungsManager : MonoBehaviour
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private Player.Player Player;
        [SerializeField] private ZeitDauer LaengeDerVergiftung;
        [SerializeField] private int KostenFuerDieSchuleProTagVergiftet;

        public Zeitpunkt ZeitpunktDerVergiftung { get; private set; }
        public VergiftbaresEssen GrundDerVergiftung { get; private set; }
        private bool VergiftungGemeldet;

        private void Update()
        {
            if (IsVergiftetMitSymtomen() && !VergiftungGemeldet)
            {
                GrundDerVergiftung.Status = VergiftungsStatus.VergiftetBemerkt;
                VergiftungGemeldet = true;
            }
        }

        public void Vergiften(VergiftbaresEssen grund)
        {
            ZeitpunktDerVergiftung = TimeManager.GetCurrentZeitpunkt();
            GrundDerVergiftung = grund;
            Player.Score.SchadenFuerDieSchule +=
                LaengeDerVergiftung.GetAnzahlTage() * KostenFuerDieSchuleProTagVergiftet;
        }

        public bool IsVergiftetMitSymtomen()
        {
            if (ZeitpunktDerVergiftung == null)
            {
                return false;
            }

            if (ZeitpunktDerVergiftung.IstAmSelbenTagWie(TimeManager.GetCurrentZeitpunkt()))
            {
                return false;
            }

            var laengeDerAktuellenVergiftung = ZeitpunktDerVergiftung.Diff(TimeManager.GetCurrentZeitpunkt());
            return LaengeDerVergiftung.IsLongerThan(laengeDerAktuellenVergiftung);
        }
    }
}