using System.Collections;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Sprache;
using HerderGames.Schule;
using HerderGames.Zeit;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    public class UnterrichtenGoal : GoalBase
    {
        private readonly Klassenraum UnterrichtsRaum;
        private readonly Vector3 Standpunkt;
        private readonly TriggerBase Trigger;
        private readonly StundenData StundeImStundenplan;
        private readonly float ReputationsAenderungBeiFehlzeit;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeAufDemWegZumRaum;
        private readonly ISaetzeMoeglichkeitenEinmalig SaetzeBegruessung;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeWaehrendUnterricht;

        public bool LehrerArrived { get; private set; }
        public bool SchuelerFreigestelltDieseStunde { get; set; }
        private Coroutine GoToRoomCoroutine;
        private Coroutine CheckAnwesenheitCoroutine;

        public UnterrichtenGoal(
            Lehrer lehrer,
            Klassenraum unterrichtsRaum,
            Vector3 standpunkt,
            TriggerBase trigger,
            StundenData stundeImStundenplan,
            float reputationsAenderungBeiFehlzeit,
            ISaetzeMoeglichkeitenMehrmals saetzeAufDemWegZumRaum = null,
            ISaetzeMoeglichkeitenEinmalig saetzeBegruessung = null,
            ISaetzeMoeglichkeitenMehrmals saetzeWaehrendUnterricht = null
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
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Trigger.Resolve();
        }

        public override void OnGoalStart()
        {
            GoToRoomCoroutine = Lehrer.AI.StartCoroutine(GoToRoom());
            CheckAnwesenheitCoroutine = Lehrer.AI.StartCoroutine(CheckAnwesenheit());
        }

        public override void OnGoalEnd()
        {
            LehrerArrived = false;
            SchuelerFreigestelltDieseStunde = false;
            Lehrer.AI.StopCoroutine(GoToRoomCoroutine);
            Lehrer.AI.StopCoroutine(CheckAnwesenheitCoroutine);
        }

        public IEnumerator GoToRoom()
        {
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAufDemWegZumRaum;
            Lehrer.Agent.destination = Standpunkt;
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
