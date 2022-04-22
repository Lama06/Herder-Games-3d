using System.Collections;
using HerderGames.Lehrer.Sprache;
using HerderGames.Schule;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    public class VergiftbaresEssenEntgiftenGoal : GoalBase
    {
        [SerializeField] private Trigger.Trigger Trigger;
        [SerializeField] private VergiftbaresEssen Essen;
        [SerializeField] private SaetzeMoeglichkeitenMehrmals SaetzeWeg;
        [SerializeField] private SaetzeMoeglichkeitenMehrmals SaetzeAngekommen;

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Essen.Status == VergiftungsStatus.VergiftetBemerkt && Trigger.Resolve();
        }

        public override IEnumerator Execute()
        {
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeWeg;
            Lehrer.Agent.destination = Essen.GetStandpunkt();
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            Essen.Status = VergiftungsStatus.NichtVergiftet;
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAngekommen;
        }
    }
}