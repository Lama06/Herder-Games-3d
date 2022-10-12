using System;
using System.Collections;
using System.Collections.Generic;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Animation;
using HerderGames.Lehrer.Sprache;
using HerderGames.Schule;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    public class InternetReparierenGoal : GoalBase
    {
        private readonly TriggerBase Trigger;
        private readonly InternetManager InternetManager;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeWeg;
        private readonly ISaetzeMoeglichkeitenEinmalig SaetzeAngekommenEinmalig;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeAngekommen;
        private readonly AbstractAnimation AnimationWeg;
        private readonly AbstractAnimation AnimationAngekommen;

        public InternetReparierenGoal(
            TriggerBase trigger,
            InternetManager internet,
            ISaetzeMoeglichkeitenMehrmals saetzeWeg = null,
            ISaetzeMoeglichkeitenEinmalig saetzeAngekommenEinmalig = null,
            ISaetzeMoeglichkeitenMehrmals saetzeAngekommen = null,
            AbstractAnimation animationWeg = null,
            AbstractAnimation animationAngekommen = null
        )
        {
            Trigger = trigger;
            InternetManager = internet;
            SaetzeWeg = saetzeWeg;
            SaetzeAngekommenEinmalig = saetzeAngekommenEinmalig;
            SaetzeAngekommen = saetzeAngekommen;
            AnimationWeg = animationWeg;
            AnimationAngekommen = animationAngekommen;
        }

        private LanDose MicInVision
        {
            get
            {
                foreach (var lanDose in InternetManager.LanDosen)
                {
                    if (lanDose.Mic && Lehrer.Vision.CanSee(lanDose.gameObject))
                    {
                        return lanDose;
                    }
                }

                return null;
            }
        }

        public override IEnumerable ExecuteGoal(Stack<Action> unexpectedGoalEndCallback)
        {
            var lanDose = MicInVision;

            if (!Trigger.ShouldRun || lanDose == null || !lanDose.Mic)
            {
                yield break;
            }

            yield return null;

            Lehrer.AnimationManager.CurrentAnimation = AnimationWeg;
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeWeg;

            foreach (var _ in NavMeshUtil.Pathfind(Lehrer, lanDose.transform))
            {
                if (!Trigger.ShouldRun || !lanDose.Mic)
                {
                    yield break;
                }

                yield return null;
            }

            Lehrer.AnimationManager.CurrentAnimation = AnimationAngekommen;
            Lehrer.Sprache.Say(SaetzeAngekommenEinmalig);
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAngekommen;
            lanDose.Mic = false;
        }
    }
}