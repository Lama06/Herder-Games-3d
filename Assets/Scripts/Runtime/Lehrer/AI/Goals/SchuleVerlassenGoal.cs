using System;
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

        public override IEnumerable<GoalStatus> ExecuteGoal(IList<Action> goalEndCallback)
        {
            yield return new GoalStatus.CanStartIf(Trigger.ShouldRun);

            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeBeimVerlassen;
            Lehrer.AnimationManager.CurrentAnimation = AnimationBeimVerlassen;

            foreach (var _ in NavMeshUtil.Pathfind(Lehrer, Ausgang))
            {
                yield return new GoalStatus.ContinueIf(Trigger.ShouldRun);
            }

            Lehrer.InSchule.InSchule = false;
            goalEndCallback.Add(() =>
            {
                Lehrer.InSchule.InSchule = true;
                Lehrer.Agent.Warp(Eingang.position);
            });

            while (Trigger.ShouldRun)
            {
                yield return new GoalStatus.Continue();
            }
        }
    }
}