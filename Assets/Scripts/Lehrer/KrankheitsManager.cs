using HerderGames.Schule;
using HerderGames.Time;
using UnityEngine;

namespace HerderGames.Lehrer
{
    [RequireComponent(typeof(Lehrer))]
    public class KrankheitsManager : MonoBehaviour
    {
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
            ZeitpunktDerErkrankung = Lehrer.GetTimeManager().GetCurrentZeitpunkt();
            GrundDerErkrankung = grund;
        }
        
        public bool IsKrankMitSymtomen()
        {
            if (ZeitpunktDerErkrankung == null)
            {
                return false;
            }

            if (ZeitpunktDerErkrankung.IstAmSelbenTagWie(Lehrer.GetTimeManager().GetCurrentZeitpunkt()))
            {
                return false;
            }

            var laengeDerAktuellenErkrankung = ZeitpunktDerErkrankung.Diff(Lehrer.GetTimeManager().GetCurrentZeitpunkt());
            return LaengeDerErkrankung.IsLongerThan(laengeDerAktuellenErkrankung); }
    }
}
