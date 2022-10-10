using System.Collections;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Animation;
using HerderGames.Lehrer.Sprache;
using HerderGames.Schule;

namespace HerderGames.Lehrer.AI.Goals
{
    public class VergiftbaresEssenEssenGoal : GoalBase
    {
        private readonly TriggerBase Trigger;
        private readonly VergiftbaresEssen VergiftbaresEssen;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeWeg;
        private readonly ISaetzeMoeglichkeitenEinmalig SaetzeAngekommenEinmalig;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeAngekommen;
        private readonly AbstractAnimation AnimationWeg;
        private readonly AbstractAnimation AnimationAngekommen;

        public VergiftbaresEssenEssenGoal(
            Lehrer lehrer,
            TriggerBase trigger,
            VergiftbaresEssen vergiftbaresEssen,
            ISaetzeMoeglichkeitenMehrmals saetzeWeg = null,
            ISaetzeMoeglichkeitenEinmalig saetzeAngekommenEinmalig = null,
            ISaetzeMoeglichkeitenMehrmals saetzeAngekommen = null,
            AbstractAnimation animationWeg = null,
            AbstractAnimation animationAngekommen = null
        ) : base(lehrer)
        {
            Trigger = trigger;
            VergiftbaresEssen = vergiftbaresEssen;
            SaetzeWeg = saetzeWeg;
            SaetzeAngekommenEinmalig = saetzeAngekommenEinmalig;
            SaetzeAngekommen = saetzeAngekommen;
            AnimationWeg = animationWeg;
            AnimationAngekommen = animationAngekommen;
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return !VergiftbaresEssen.VergiftungBemerkt && Trigger.ShouldRun;
        }

        protected override IEnumerator Execute()
        {
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeWeg;
            Lehrer.Agent.destination = VergiftbaresEssen.GetStandpunkt();
            Lehrer.AnimationManager.CurrentAnimation = AnimationWeg;
            
            yield return NavMeshUtil.Pathfind(Lehrer.Agent);
            
            if (VergiftbaresEssen.Vergiftet)
            {
                Lehrer.Vergiftung.Vergiften(VergiftbaresEssen);
            }
            Lehrer.Sprache.Say(SaetzeAngekommenEinmalig);
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAngekommen;
            Lehrer.AnimationManager.CurrentAnimation = AnimationAngekommen;
        }
    }
}