using System;
using System.Collections;
using System.Collections.Generic;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Animation;
using HerderGames.Lehrer.Sprache;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    public class AnGespraechTeilnehmenGoal : GoalBase
    {
        private readonly TriggerBase Trigger;
        private readonly Transform Standpunkt;
        public Gespraech Gespraech { get; }
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeAufDemWeg;
        private readonly AbstractAnimation AnimationWeg;
        private readonly AbstractAnimation AnimationAngekommen;

        public bool IsAngekommen { get; private set; }

        public AnGespraechTeilnehmenGoal(
            TriggerBase trigger,
            Transform standpunkt,
            Gespraech gespraech,
            ISaetzeMoeglichkeitenMehrmals saetzeAufDemWeg = null,
            AbstractAnimation animationWeg = null,
            AbstractAnimation animationAngekommen = null
        )
        {
            Trigger = trigger;
            Standpunkt = standpunkt;
            Gespraech = gespraech;
            SaetzeAufDemWeg = saetzeAufDemWeg;
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

            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAufDemWeg;
            Lehrer.AnimationManager.CurrentAnimation = AnimationWeg;

            foreach (var _ in NavMeshUtil.Pathfind(Lehrer, Standpunkt))
            {
                if (!Trigger.ShouldRun)
                {
                    yield break;
                }

                yield return null;
            }
            
            Lehrer.AnimationManager.CurrentAnimation = AnimationAngekommen;
            Lehrer.Sprache.SaetzeMoeglichkeiten = null;
            
            IsAngekommen = true;
            unexpectedGoalEndCallback.Push(() => IsAngekommen = false);

            while (Trigger.ShouldRun)
            {
                yield return null;
            }

            unexpectedGoalEndCallback.Pop()();
        }
    }
}
