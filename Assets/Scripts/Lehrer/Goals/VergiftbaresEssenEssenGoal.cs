using System.Collections;
using HerderGames.AI;
using HerderGames.Lehrer.Sprache;
using HerderGames.Schule;
using HerderGames.Time;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    [RequireComponent(typeof(VergiftungsManager))]
    public class VergiftbaresEssenEssenGoal : LehrerGoalBase
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private VergiftbaresEssen VergiftbaresEssen;
        [SerializeField] private WoechentlicheZeitspannen Wann;
        [SerializeField] private SaetzeMoeglichkeitenMehrmals SaetzeWeg;
        [SerializeField] private SaetzeMoeglichkeitenMehrmals SaetzeAngekommen;

        private VergiftungsManager Vergiftung;

        protected override void Awake()
        {
            base.Awake();
            Vergiftung = GetComponent<VergiftungsManager>();
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return VergiftbaresEssen.Status != VergiftungsStatus.VergiftetBemerkt && Wann.IsInside(TimeManager.GetCurrentWochentag(), TimeManager.GetCurrentTime());
        }

        public override IEnumerator Execute()
        {
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeWeg;
            Lehrer.Agent.destination = VergiftbaresEssen.GetStandpunkt();
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            if (VergiftbaresEssen.Status.IsVergiftet())
            {
                Vergiftung.Vergiften(VergiftbaresEssen);
            }
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAngekommen;
        }
    }
}
