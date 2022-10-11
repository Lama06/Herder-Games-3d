using System;
using System.Collections;
using System.Collections.Generic;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Animation;
using HerderGames.Lehrer.Sprache;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    public class SchuleVerlassenGoal : GoalBase
    {
        private readonly TriggerBase Trigger;
        private readonly Transform Eingang;
        private readonly Transform Ausgang;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeBeimVerlassen;
        private readonly AbstractAnimation AnimationBeimVerlassen;

        public SchuleVerlassenGoal(
            Lehrer lehrer,
            TriggerBase trigger,
            Transform eingang,
            Transform ausgang,
            ISaetzeMoeglichkeitenMehrmals saetzeBeimVerlassen = null,
            AbstractAnimation animationBeimVerlassen = null
        ) : base(lehrer)
        {
            Trigger = trigger;
            Eingang = eingang;
            Ausgang = ausgang;
            SaetzeBeimVerlassen = saetzeBeimVerlassen;
            AnimationBeimVerlassen = animationBeimVerlassen;
        }

        public override IEnumerable ExecuteGoal(Stack<Action> unexpectedGoalEndCallback)
        {
            if (!Trigger.ShouldRun)
            {
                yield break;
            }

            yield return null;

            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeBeimVerlassen;
            Lehrer.AnimationManager.CurrentAnimation = AnimationBeimVerlassen;

            foreach (var _ in NavMeshUtil.Pathfind(Lehrer, Ausgang))
            {
                if (!Trigger.ShouldRun)
                {
                    yield break;
                }

                yield return null;
            }

            Lehrer.InSchule.InSchule = false;
            unexpectedGoalEndCallback.Push(() =>
            {
                Lehrer.InSchule.InSchule = true;
                Lehrer.Agent.Warp(Eingang.position);
            });

            while (Trigger.ShouldRun)
            {
                yield return null;
            }

            unexpectedGoalEndCallback.Pop()();
        }
    }
}