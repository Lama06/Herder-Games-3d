using System.Collections;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Sprache;
using HerderGames.Schule;

namespace HerderGames.Lehrer.AI.Goals
{
    public class VergiftbaresEssenEntgiftenGoal : GoalBase
    {
        private readonly TriggerBase Trigger;
        private readonly VergiftbaresEssen Essen;
        private readonly SaetzeMoeglichkeitenMehrmals SaetzeWeg;
        private readonly SaetzeMoeglichkeitenMehrmals SaetzeAngekommen;

        public VergiftbaresEssenEntgiftenGoal(
            Lehrer lehrer,
            TriggerBase trigger,
            VergiftbaresEssen essen,
            SaetzeMoeglichkeitenMehrmals saetzeWeg,
            SaetzeMoeglichkeitenMehrmals saetzeAngekommen
        ) : base(lehrer)
        {
            Trigger = trigger;
            Essen = essen;
            SaetzeWeg = saetzeWeg;
            SaetzeAngekommen = saetzeAngekommen;
        }

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
