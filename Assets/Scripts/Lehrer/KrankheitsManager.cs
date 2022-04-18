using HerderGames.Schule;
using HerderGames.Time;
using UnityEngine;

namespace HerderGames.Lehrer
{
    [RequireComponent(typeof(Lehrer))]
    public class KrankheitsManager : MonoBehaviour
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private ZeitDauer LaengeDerErkrankung;

        private Lehrer Lehrer;

        public Zeitpunkt ZeitpunktDerErkrankung { get; private set; }
        public VergiftbaresEssen GrundDerErkrankung { get; private set; }
        private bool VergiftungGemeldet;
        
        private void Awake()
        {
            Lehrer = GetComponent<Lehrer>();
        }

        private void Update()
        {
            if (IsKrankMitSymtomen() && !VergiftungGemeldet)
            {
                GrundDerErkrankung.Status = VergiftungsStatus.VergiftetBemerkt;
                VergiftungGemeldet = true;
            }
        }

        public void Erkranken(VergiftbaresEssen grund)
        {
            ZeitpunktDerErkrankung = TimeManager.GetCurrentZeitpunkt();
            GrundDerErkrankung = grund;
        }
        
        public bool IsKrankMitSymtomen()
        {
            if (ZeitpunktDerErkrankung == null)
            {
                return false;
            }

            if (ZeitpunktDerErkrankung.IstAmSelbenTagWie(TimeManager.GetCurrentZeitpunkt()))
            {
                return false;
            }

            var laengeDerAktuellenErkrankung = ZeitpunktDerErkrankung.Diff(TimeManager.GetCurrentZeitpunkt());
            return LaengeDerErkrankung.IsLongerThan(laengeDerAktuellenErkrankung); }
    }
}
