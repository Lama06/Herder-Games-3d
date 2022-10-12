using System;
using System.Collections;
using System.Collections.Generic;
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
            TriggerBase trigger,
            VergiftbaresEssen vergiftbaresEssen,
            ISaetzeMoeglichkeitenMehrmals saetzeWeg = null,
            ISaetzeMoeglichkeitenEinmalig saetzeAngekommenEinmalig = null,
            ISaetzeMoeglichkeitenMehrmals saetzeAngekommen = null,
            AbstractAnimation animationWeg = null,
            AbstractAnimation animationAngekommen = null
        )
        {
            Trigger = trigger;
            VergiftbaresEssen = vergiftbaresEssen;
            SaetzeWeg = saetzeWeg;
            SaetzeAngekommenEinmalig = saetzeAngekommenEinmalig;
            SaetzeAngekommen = saetzeAngekommen;
            AnimationWeg = animationWeg;
            AnimationAngekommen = animationAngekommen;
        }

        public override IEnumerable ExecuteGoal(Stack<Action> unexpectedGoalEndCallback)
        {
            if (!Trigger.ShouldRun || VergiftbaresEssen.VergiftungBemerkt)
            {
                yield break;
            }

            yield return null;

            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeWeg;
            Lehrer.AnimationManager.CurrentAnimation = AnimationWeg;

            foreach (var _ in NavMeshUtil.Pathfind(Lehrer, VergiftbaresEssen.GetStandpunkt()))
            {
                if (!Trigger.ShouldRun || VergiftbaresEssen.VergiftungBemerkt)
                {
                    yield break;
                }

                yield return null;
            }

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