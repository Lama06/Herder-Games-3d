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
        private readonly SaetzeMoeglichkeitenEinmalig SaetzeAngekommenEinmalig;
        private readonly SaetzeMoeglichkeitenMehrmals SaetzeAngekommen;

        public VergiftbaresEssenEntgiftenGoal(
            Lehrer lehrer,
            TriggerBase trigger,
            VergiftbaresEssen essen,
            SaetzeMoeglichkeitenMehrmals saetzeWeg,
            SaetzeMoeglichkeitenEinmalig saetzeAngekommenEinmalig,
            SaetzeMoeglichkeitenMehrmals saetzeAngekommen
        ) : base(lehrer)
        {
            Trigger = trigger;
            Essen = essen;
            SaetzeWeg = saetzeWeg;
            SaetzeAngekommenEinmalig = saetzeAngekommenEinmalig;
            SaetzeAngekommen = saetzeAngekommen;
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Essen.Status.IsBemerkt() && Trigger.Resolve();
        }

        public override IEnumerator Execute()
        {
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeWeg;
            Lehrer.Agent.destination = Essen.GetStandpunkt();
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            Lehrer.Sprache.Say(SaetzeAngekommenEinmalig);
            Essen.Status = EssenVergiftungsStatus.NichtVergiftet;
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAngekommen;
        }
    }
}
