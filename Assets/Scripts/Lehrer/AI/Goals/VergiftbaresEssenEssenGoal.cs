using System.Collections;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Sprache;
using HerderGames.Schule;

namespace HerderGames.Lehrer.AI.Goals
{
    public class VergiftbaresEssenEssenGoal : GoalBase
    {
        private readonly TriggerBase Trigger;
        private readonly VergiftbaresEssen VergiftbaresEssen;
        private readonly SaetzeMoeglichkeitenMehrmals SaetzeWeg;
        private readonly SaetzeMoeglichkeitenEinmalig SaetzeAngekommenEinmalig;
        private readonly SaetzeMoeglichkeitenMehrmals SaetzeAngekommen;
        private readonly VergiftungsManager Vergiftung;

        public VergiftbaresEssenEssenGoal(
            Lehrer lehrer,
            TriggerBase trigger,
            VergiftbaresEssen vergiftbaresEssen,
            SaetzeMoeglichkeitenMehrmals saetzeWeg,
            SaetzeMoeglichkeitenEinmalig saetzeAngekommenEinmalig,
            SaetzeMoeglichkeitenMehrmals saetzeAngekommen,
            VergiftungsManager vergiftung
        ) : base(lehrer)
        {
            Trigger = trigger;
            VergiftbaresEssen = vergiftbaresEssen;
            SaetzeWeg = saetzeWeg;
            SaetzeAngekommenEinmalig = saetzeAngekommenEinmalig;
            SaetzeAngekommen = saetzeAngekommen;
            Vergiftung = vergiftung;
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return !VergiftbaresEssen.VergiftungBemerkt && Trigger.Resolve();
        }

        public override IEnumerator Execute()
        {
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeWeg;
            Lehrer.Agent.destination = VergiftbaresEssen.GetStandpunkt();
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            if (VergiftbaresEssen.Vergiftet)
            {
                Vergiftung.Vergiften(VergiftbaresEssen);
            }

            Lehrer.Sprache.Say(SaetzeAngekommenEinmalig);
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAngekommen;
        }
    }
}
