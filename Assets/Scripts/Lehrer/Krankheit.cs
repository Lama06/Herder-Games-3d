using HerderGames.Schule;
using HerderGames.Time;
using UnityEngine;

namespace HerderGames
{
    public class Krankheit : MonoBehaviour
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private ZeitDauer LaengeDerErkrankung;

        public Zeitpunkt ZeitpunktDerErkrankung { get; private set; }
        public VergiftbaresEssen GrundDerErkrankung { get; private set; }

        public void Erkranken(VergiftbaresEssen grund)
        {
            ZeitpunktDerErkrankung = TimeManager.GetCurrentZeitpunkt();
            GrundDerErkrankung = grund;
        }
        
        public bool IsKrank()
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
