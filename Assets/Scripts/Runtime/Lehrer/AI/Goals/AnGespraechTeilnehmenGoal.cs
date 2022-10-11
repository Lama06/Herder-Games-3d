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
            Lehrer lehrer,
            TriggerBase trigger,
            Transform standpunkt,
            Gespraech gespraech,
            ISaetzeMoeglichkeitenMehrmals saetzeAufDemWeg = null,
            AbstractAnimation animationWeg = null,
            AbstractAnimation animationAngekommen = null
        ) : base(lehrer)
        {
            Trigger = trigger;
            Standpunkt = standpunkt;
            Gespraech = gespraech;
            SaetzeAufDemWeg = saetzeAufDemWeg;
            AnimationWeg = animationWeg;
            AnimationAngekommen = animationAngekommen;
        }

        public override IEnumerable<GoalStatus> ExecuteGoal(IList<Action> goalEndCallback)
        {
            yield return new GoalStatus.CanStartIf(Trigger.ShouldRun);
            
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAufDemWeg;
            Lehrer.AnimationManager.CurrentAnimation = AnimationWeg;

            foreach (var _ in NavMeshUtil.Pathfind(Lehrer, Standpunkt))
            {
                yield return new GoalStatus.ContinueIf(Trigger.ShouldRun);
            }
            
            Lehrer.AnimationManager.CurrentAnimation = AnimationAngekommen;
            Lehrer.Sprache.SaetzeMoeglichkeiten = null;
            
            IsAngekommen = true;
            goalEndCallback.Add(() => IsAngekommen = false);

            while (Trigger.ShouldRun)
            {
                yield return new GoalStatus.Continue();
            }
        }
    }
}
