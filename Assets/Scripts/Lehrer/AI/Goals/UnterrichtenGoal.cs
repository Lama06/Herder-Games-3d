using System;
using System.Collections;
using HerderGames.Lehrer.Sprache;
using HerderGames.Time;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    public class UnterrichtenGoal : GoalBase
    {
        [Header("Allgemein")] [SerializeField] private Klassenraum UnterrichtsRaum;
        [SerializeField] private string Fach;
        [SerializeField] private float ReputationsAenderungBeiFehlzeit;

        [Header("Wann")] [SerializeField] private StundenData StundeImStundenplan;
        [SerializeField] private Trigger.Trigger Trigger;

        [Header("Sätze")] [SerializeField] private SaetzeMoeglichkeitenMehrmals SaetzeAufDemWegZumRaum;
        [SerializeField] private SaetzeMoeglichkeitenEinmalig SaetzeBegruessung;
        [SerializeField] private SaetzeMoeglichkeitenMehrmals SaetzeWaehrendUnterricht;
        
        public bool LehrerArrived { get; private set; }
        public bool SchuelerFreigestelltDieseStunde { get; set; }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Trigger.Resolve();
        }

        public override void OnGoalStart()
        {
            LehrerArrived = false;
            SchuelerFreigestelltDieseStunde = false;
            StartCoroutine(GoToRoom());
            StartCoroutine(CheckAnwesenheit());
        }

        public IEnumerator GoToRoom()
        {
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAufDemWegZumRaum;
            Lehrer.Agent.destination = UnterrichtsRaum.GetLehrerStandpunkt().position;
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

        public string GetFach()
        {
            return Fach;
        }

        public Klassenraum GetKlassenraum()
        {
            return UnterrichtsRaum;
        }

        public StundenData GetStundeImStundenplan()
        {
            return StundeImStundenplan;
        }

        [Serializable]
        public class StundenData
        {
            public Wochentag Wochentag;
            public int FachIndex;
        }
    }
}