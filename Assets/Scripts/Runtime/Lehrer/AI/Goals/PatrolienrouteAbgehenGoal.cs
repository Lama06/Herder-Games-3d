using System;
using System.Collections;
using System.Collections.Generic;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Animation;
using HerderGames.Lehrer.Sprache;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    public class PatrolienrouteAbgehenGoal : GoalBase
    {
        private readonly TriggerBase Trigger;
        private readonly IList<Transform> Punkte;
        private readonly ISaetzeMoeglichkeitenMehrmals Saetze;
        private readonly AbstractAnimation Animation;

        public PatrolienrouteAbgehenGoal(
            Lehrer lehrer,
            TriggerBase trigger,
            IList<Transform> punkte,
            ISaetzeMoeglichkeitenMehrmals saetze = null,
            AbstractAnimation animation = null
        ) : base(lehrer)
        {
            Trigger = trigger;
            Punkte = punkte;
            Saetze = saetze;
            Animation = animation;
        }

        public override IEnumerable<GoalStatus> ExecuteGoal(IList<Action> goalEndCallback)
        {
            yield return new GoalStatus.CanStartIf(Trigger.ShouldRun);

            Lehrer.Sprache.SaetzeMoeglichkeiten = Saetze;
            Lehrer.AnimationManager.CurrentAnimation = Animation;

            while (true)
            {
                foreach (var punkt in Punkte)
                {
                    foreach (var _ in NavMeshUtil.Pathfind(Lehrer, punkt))
                    {
                        yield return new GoalStatus.ContinueIf(Trigger.ShouldRun);
                    }
                }

                // Den ersten und letzten Punkt überspringen, damit der Lehrer nicht doppelt zum selben Punkt läuft
                for (var i = Punkte.Count - 2; i >= 1; i--)
                {
                    foreach (var _ in NavMeshUtil.Pathfind(Lehrer, Punkte[i]))
                    {
                        yield return new GoalStatus.ContinueIf(Trigger.ShouldRun);
                    }
                }
            }
        }
    }
}