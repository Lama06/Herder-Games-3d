using System;
using System.Collections;
using System.Collections.Generic;
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

        public VergiftbaresEssenEntgiftenGoal(
            TriggerBase trigger,
            VergiftbaresEssen essen,
            ISaetzeMoeglichkeitenMehrmals saetzeWeg = null,
            ISaetzeMoeglichkeitenEinmalig saetzeAngekommenEinmalig = null,
            ISaetzeMoeglichkeitenMehrmals saetzeAngekommen = null,
            AbstractAnimation animationWeg = null,
            AbstractAnimation animationAngekommen = null
        )
        {
            Trigger = trigger;
            Essen = essen;
            SaetzeWeg = saetzeWeg;
            SaetzeAngekommenEinmalig = saetzeAngekommenEinmalig;
            SaetzeAngekommen = saetzeAngekommen;
            AnimationWeg = animationWeg;
            AnimationAngekommen = animationAngekommen;
        }

        public override IEnumerable ExecuteGoal(Stack<Action> unexpectedGoalEndCallback)
        {
            if (!Trigger.ShouldRun || !Essen.Vergiftet || !Essen.VergiftungBemerkt)
            {
                yield break;
            }

            yield return null;

            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeWeg;
            Lehrer.AnimationManager.CurrentAnimation = AnimationWeg;

            foreach (var _ in NavMeshUtil.Pathfind(Lehrer, Essen.GetStandpunkt()))
            {
                if (!Trigger.ShouldRun || !Essen.Vergiftet || !Essen.VergiftungBemerkt)
                {
                    yield break;
                }

                yield return null;
            }

            Lehrer.Sprache.Say(SaetzeAngekommenEinmalig);
            Essen.Entgiften();
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAngekommen;
            Lehrer.AnimationManager.CurrentAnimation = AnimationAngekommen;
        }
    }
}