using System.Collections;
using HerderGames.Lehrer.Sprache;
using HerderGames.Schule;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    [RequireComponent(typeof(VergiftungsManager))]
    public class VergiftbaresEssenEssenGoal : GoalBase
    {
        [SerializeField] private Trigger.Trigger Trigger;
        [SerializeField] private VergiftbaresEssen VergiftbaresEssen;
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
            return VergiftbaresEssen.Status != VergiftungsStatus.VergiftetBemerkt && Trigger.Resolve();
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