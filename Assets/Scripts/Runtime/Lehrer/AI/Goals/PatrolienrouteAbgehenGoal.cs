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

        public override IEnumerable ExecuteGoal(Stack<Action> unexpectedGoalEndCallback)
        {
            if (!Trigger.ShouldRun)
            {
                yield break;
            }

            yield return null;

            Lehrer.Sprache.SaetzeMoeglichkeiten = Saetze;
            Lehrer.AnimationManager.CurrentAnimation = Animation;

            while (true)
            {
                foreach (var punkt in Punkte)
                {
                    foreach (var _ in NavMeshUtil.Pathfind(Lehrer, punkt))
                    {
                        if (!Trigger.ShouldRun)
                        {
                            yield break;
                        }

                        yield return null;
                    }
                }

                // Den ersten und letzten Punkt überspringen, damit der Lehrer nicht doppelt zum selben Punkt läuft
                for (var i = Punkte.Count - 2; i >= 1; i--)
                {
                    foreach (var _ in NavMeshUtil.Pathfind(Lehrer, Punkte[i]))
                    {
                        if (!Trigger.ShouldRun)
                        {
                            yield break;
                        }

                        yield return null;
                    }
                }
            }
        }
    }
}