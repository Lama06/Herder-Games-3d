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
            TriggerBase trigger,
            Transform eingang,
            Transform ausgang,
            ISaetzeMoeglichkeitenMehrmals saetzeBeimVerlassen = null,
            AbstractAnimation animationBeimVerlassen = null
        )
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

            InSchule = false;
            unexpectedGoalEndCallback.Push(() =>
            {
                InSchule = true;
                Lehrer.Agent.Warp(Eingang.position);
            });

            while (Trigger.ShouldRun)
            {
                yield return null;
            }

            unexpectedGoalEndCallback.Pop()();
        }

        private bool InSchule
        {
            set
            {
                foreach (var collider in Lehrer.GetComponentsInChildren<Collider>())
                {
                    collider.enabled = value;
                }

                foreach (var renderer in Lehrer.GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = value;
                }

                // Damit der Lehrer nicht redet, während er nicht in der Schule ist
                Lehrer.Sprache.Enabled = value;

                Lehrer.FragenManager.Enabled = value;
            }
        }
    }
}