using System;
using System.Collections;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Sprache;
using HerderGames.Zeit;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    public class UnterrichtenGoal : GoalBase
    {
        private readonly Klassenraum UnterrichtsRaum;
        private readonly TriggerBase Trigger;
        private readonly StundenData StundeImStundenplan;
        private readonly float ReputationsAenderungBeiFehlzeit;
        private readonly SaetzeMoeglichkeitenMehrmals SaetzeAufDemWegZumRaum;
        private readonly SaetzeMoeglichkeitenEinmalig SaetzeBegruessung;
        private readonly SaetzeMoeglichkeitenMehrmals SaetzeWaehrendUnterricht;

        public bool LehrerArrived { get; private set; }
        public bool SchuelerFreigestelltDieseStunde { get; set; }
        private Coroutine GoToRoomCoroutine;
        private Coroutine CheckAnwesenheitCoroutine;

        public UnterrichtenGoal(Lehrer lehrer,
            Klassenraum unterrichtsRaum,
            TriggerBase trigger,
            StundenData stundeImStundenplan,
            float reputationsAenderungBeiFehlzeit,
            SaetzeMoeglichkeitenMehrmals saetzeAufDemWegZumRaum,
            SaetzeMoeglichkeitenEinmalig saetzeBegruessung,
            SaetzeMoeglichkeitenMehrmals saetzeWaehrendUnterricht
        ) : base(lehrer)
        {
            UnterrichtsRaum = unterrichtsRaum;
            Trigger = trigger;
            StundeImStundenplan = stundeImStundenplan;
            ReputationsAenderungBeiFehlzeit = reputationsAenderungBeiFehlzeit;
            SaetzeAufDemWegZumRaum = saetzeAufDemWegZumRaum;
            SaetzeBegruessung = saetzeBegruessung;
            SaetzeWaehrendUnterricht = saetzeWaehrendUnterricht;
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Trigger.Resolve();
        }

        public override void OnGoalStart()
        {
            LehrerArrived = false;
            SchuelerFreigestelltDieseStunde = false;
            GoToRoomCoroutine = Lehrer.StartCoroutine(GoToRoom());
            CheckAnwesenheitCoroutine = Lehrer.StartCoroutine(CheckAnwesenheit());
        }

        public override void OnGoalEnd(GoalEndReason reason)
        {
            Lehrer.StopCoroutine(GoToRoomCoroutine);
            Lehrer.StopCoroutine(CheckAnwesenheitCoroutine);
        }

        public IEnumerator GoToRoom()
        {
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAufDemWegZumRaum;
            Lehrer.Agent.destination = UnterrichtsRaum.GetLehrerStandpunkt();
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            yield return new WaitForSeconds(5);
            Lehrer.Sprache.Say(SaetzeBegruessung);
            LehrerArrived = true;
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeWaehrendUnterricht;
        }

        private IEnumerator CheckAnwesenheit()
        {
            while (true)
            {
                if (LehrerArrived && !UnterrichtsRaum.PlayerInside && !SchuelerFreigestelltDieseStunde)
                {
                    Lehrer.Reputation.AddReputation(ReputationsAenderungBeiFehlzeit);
                    yield break;
                }

                yield return null;
            }
        }

        public Klassenraum GetKlassenraum()
        {
            return UnterrichtsRaum;
        }

        public StundenData GetStundeImStundenplan()
        {
            return StundeImStundenplan;
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
