using System;
using System.Collections;
using System.Collections.Generic;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer;
using HerderGames.Lehrer.Animation;
using HerderGames.Lehrer.Sprache;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    public class MoveToAndStandAtGoal : GoalBase
    {
        private readonly TriggerBase Trigger;
        private readonly Transform Position;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeWeg;
        private readonly ISaetzeMoeglichkeitenEinmalig SaetzeAngekommenEinmalig;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeAngekommen;
        private readonly AbstractAnimation AnimationWeg;
        private readonly AbstractAnimation AnimationAngekommen;

        public MoveToAndStandAtGoal(
            TriggerBase trigger,
            Transform position,
            ISaetzeMoeglichkeitenMehrmals saetzeWeg = null,
            ISaetzeMoeglichkeitenMehrmals saetzeAngekommen = null,
            ISaetzeMoeglichkeitenEinmalig saetzeAngekommenEinmalig = null,
            AbstractAnimation animationWeg = null,
            AbstractAnimation animationAngekommen = null
        )
        {
            Trigger = trigger;
            Position = position;
            SaetzeWeg = saetzeWeg;
            SaetzeAngekommenEinmalig = saetzeAngekommenEinmalig;
            SaetzeAngekommen = saetzeAngekommen;
            AnimationWeg = animationWeg;
            AnimationAngekommen = animationAngekommen;
        }

        public override IEnumerable ExecuteGoal(Stack<Action> unexpectedGoalEndCallback)
        {
            if (!Trigger.ShouldRun)
            {
                yield break;
            }

            yield return null;

            Lehrer.AnimationManager.CurrentAnimation = AnimationWeg;
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeWeg;

            foreach (var _ in NavMeshUtil.Pathfind(Lehrer, Position))
            {
                if (!Trigger.ShouldRun)
                {
                    yield break;
                }

                yield return null;
            }
            
            Lehrer.AnimationManager.CurrentAnimation = AnimationAngekommen;
            Lehrer.Sprache.Say(SaetzeAngekommenEinmalig);
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAngekommen;

            while (Trigger.ShouldRun)
            {
                yield return null;
            }
        }
    }
}
