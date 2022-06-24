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
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeWeg;
        private readonly ISaetzeMoeglichkeitenEinmalig SaetzeAngekommenEinmalig;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeAngekommen;

        private bool Fertig;
        
        public VergiftbaresEssenEntgiftenGoal(
            Lehrer lehrer,
            TriggerBase trigger,
            VergiftbaresEssen essen,
            ISaetzeMoeglichkeitenMehrmals saetzeWeg = null,
            ISaetzeMoeglichkeitenEinmalig saetzeAngekommenEinmalig = null,
            ISaetzeMoeglichkeitenMehrmals saetzeAngekommen = null
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
            if (!Trigger.Resolve())
            {
                return false;
            }

            if (currentlyRunning)
            {
                return !Fertig;
            }
            
            return Essen.Vergiftet && Essen.VergiftungBemerkt;
        }

        public override void OnGoalStart()
        {
            Fertig = false;
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeWeg;
            Lehrer.Agent.destination = Essen.GetStandpunkt();
        }

        public override IEnumerator Execute()
        {
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            Lehrer.Sprache.Say(SaetzeAngekommenEinmalig);
            Essen.Entgiften();
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAngekommen;
            Fertig = true;
        }
    }
}
