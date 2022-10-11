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

        public override IEnumerable<GoalStatus> ExecuteGoal(IList<Action> goalEndCallback)
        {
            yield return new GoalStatus.CanStartIf(Trigger.ShouldRun);
            
            goalEndCallback.Add(() => SchuelerFreigestelltDieseStunde = false);
            
            Lehrer.AnimationManager.CurrentAnimation = AnimationWeg;
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAufDemWegZumRaum;

            foreach (var _ in NavMeshUtil.Pathfind(Lehrer, Standpunkt))
            {
                yield return new GoalStatus.ContinueIf(Trigger.ShouldRun);
            }
            
            Lehrer.AnimationManager.CurrentAnimation = AnimationAngekommen;
            Lehrer.Sprache.Say(SaetzeBegruessung);
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeWaehrendUnterricht;
            LehrerArrived = true;
            goalEndCallback.Add(() => LehrerArrived = false);

            var schuelerBestraft = false;
            while (Trigger.ShouldRun)
            {
                if (!schuelerBestraft && LehrerArrived && !UnterrichtsRaum.PlayerInside && !SchuelerFreigestelltDieseStunde)
                {
                    Lehrer.Reputation.AddReputation(ReputationsAenderungBeiFehlzeit);
                    schuelerBestraft = true;
                }

                yield return new GoalStatus.Continue();
            }
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
