using System.Collections;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Animation;
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
        private readonly AbstractAnimation AnimationWeg;
        private readonly AbstractAnimation AnimationAngekommen;

        private bool Fertig;
        
        public VergiftbaresEssenEntgiftenGoal(
            Lehrer lehrer,
            TriggerBase trigger,
            VergiftbaresEssen essen,
            ISaetzeMoeglichkeitenMehrmals saetzeWeg = null,
            ISaetzeMoeglichkeitenEinmalig saetzeAngekommenEinmalig = null,
            ISaetzeMoeglichkeitenMehrmals saetzeAngekommen = null,
            AbstractAnimation animationWeg = null,
            AbstractAnimation animationAngekommen = null
        ) : base(lehrer)
        {
            Trigger = trigger;
            Essen = essen;
            SaetzeWeg = saetzeWeg;
            SaetzeAngekommenEinmalig = saetzeAngekommenEinmalig;
            SaetzeAngekommen = saetzeAngekommen;
            AnimationWeg = animationWeg;
            AnimationAngekommen = animationAngekommen;
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            if (!Trigger.ShouldRun)
            {
                return false;
            }

            if (currentlyRunning)
            {
                return !Fertig;
            }
            
            return Essen.Vergiftet && Essen.VergiftungBemerkt;
        }

        protected override IEnumerator Execute()
        {
            Fertig = false;
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeWeg;
            Lehrer.AnimationManager.CurrentAnimation = AnimationWeg;
            Lehrer.Agent.destination = Essen.GetStandpunkt();
            
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            
            Lehrer.Sprache.Say(SaetzeAngekommenEinmalig);
            Essen.Entgiften();
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAngekommen;
            Lehrer.AnimationManager.CurrentAnimation = AnimationAngekommen;
            Fertig = true;
        }
    }
}
