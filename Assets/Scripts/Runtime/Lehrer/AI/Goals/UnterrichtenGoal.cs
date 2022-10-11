using System;
using System.Collections;
using System.Collections.Generic;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Animation;
using HerderGames.Lehrer.Sprache;
using HerderGames.Schule;
using HerderGames.Zeit;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    public class UnterrichtenGoal : GoalBase
    {
        public Klassenraum UnterrichtsRaum { get; }
        private readonly Transform Standpunkt;
        private readonly TriggerBase Trigger;
        public StundenData StundeImStundenplan { get; }
        private readonly float ReputationsAenderungBeiFehlzeit;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeAufDemWegZumRaum;
        private readonly ISaetzeMoeglichkeitenEinmalig SaetzeBegruessung;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeWaehrendUnterricht;
        private readonly AbstractAnimation AnimationWeg;
        private readonly AbstractAnimation AnimationAngekommen;

        public bool LehrerArrived { get; private set; }
        public bool SchuelerFreigestelltDieseStunde { get; set; }
        private Coroutine GoToRoomCoroutine;
        private Coroutine CheckAnwesenheitCoroutine;

        public UnterrichtenGoal(
            Lehrer lehrer,
            Klassenraum unterrichtsRaum,
            Transform standpunkt,
            TriggerBase trigger,
            StundenData stundeImStundenplan,
            float reputationsAenderungBeiFehlzeit,
            ISaetzeMoeglichkeitenMehrmals saetzeAufDemWegZumRaum = null,
            ISaetzeMoeglichkeitenEinmalig saetzeBegruessung = null,
            ISaetzeMoeglichkeitenMehrmals saetzeWaehrendUnterricht = null,
            AbstractAnimation animationWeg = null,
            AbstractAnimation animationAngekommen = null
        ) : base(lehrer)
        {
            UnterrichtsRaum = unterrichtsRaum;
            Standpunkt = standpunkt;
            Trigger = trigger;
            StundeImStundenplan = stundeImStundenplan;
            ReputationsAenderungBeiFehlzeit = reputationsAenderungBeiFehlzeit;
            SaetzeAufDemWegZumRaum = saetzeAufDemWegZumRaum;
            SaetzeBegruessung = saetzeBegruessung;
            SaetzeWaehrendUnterricht = saetzeWaehrendUnterricht;
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
            
            unexpectedGoalEndCallback.Push(() => SchuelerFreigestelltDieseStunde = false); // 1
            
            Lehrer.AnimationManager.CurrentAnimation = AnimationWeg;
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAufDemWegZumRaum;

            foreach (var _ in NavMeshUtil.Pathfind(Lehrer, Standpunkt))
            {
                if (!Trigger.ShouldRun)
                {
                    unexpectedGoalEndCallback.Pop()(); // 1
                    yield break;
                }

                yield return null;
            }
            
            Lehrer.AnimationManager.CurrentAnimation = AnimationAngekommen;
            Lehrer.Sprache.Say(SaetzeBegruessung);
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeWaehrendUnterricht;
            LehrerArrived = true;
            unexpectedGoalEndCallback.Push(() => LehrerArrived = false); // 2

            var schuelerBestraft = false;
            while (Trigger.ShouldRun)
            {
                if (!schuelerBestraft && LehrerArrived && !UnterrichtsRaum.PlayerInside && !SchuelerFreigestelltDieseStunde)
                {
                    Lehrer.Reputation.AddReputation(ReputationsAenderungBeiFehlzeit);
                    schuelerBestraft = true;
                }

                yield return null;
            }

            unexpectedGoalEndCallback.Pop()(); // 1
            unexpectedGoalEndCallback.Pop()(); // 2
        }

        public class StundenData
        {
            public readonly Wochentag Wochentag;
            public readonly int FachIndex;
            public readonly string Fach;

            public StundenData(Wochentag wochentag, int fachIndex, string fach)
            {
                Wochentag = wochentag;
                FachIndex = fachIndex;
                Fach = fach;
            }
        }
    }
}
