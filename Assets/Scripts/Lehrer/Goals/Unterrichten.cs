using System;
using System.Collections;
using HerderGames.AI;
using HerderGames.Lehrer.Sprache;
using HerderGames.Time;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    public class Unterrichten : LehrerGoalBase
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private Player.Player Player;

        [Header("Allgemein")] [SerializeField] private Klassenraum UnterrichtsRaum;
        [SerializeField] private FachType Fach;

        [Header("Sätze")] [SerializeField] private SaetzeMoeglichkeitenMehrmals SaetzeAufDemWegZumRaum;
        [SerializeField] private SaetzeMoeglichkeitenMehrmals SaetzeWaehrendUnterricht;
        [SerializeField] private SaetzeMoeglichkeitenEinmalig SaetzeBegruessung;

        [Header("Wann")] [SerializeField] private StundenData Stunde;
        [SerializeField] private float ZeitPufferVorher;
        [SerializeField] private float Ueberziehungszeit;

        [Header("Reputation")] [SerializeField]
        private float ReputationsVerlustBeiFehlzeit;

        private WoechentlicheZeitspannen BakedZeitspannen;
        public bool LehrerArrived { get; private set; }
        [NonSerialized] public bool SchuelerFreigestelltDieseStunde;

        protected override void Awake()
        {
            base.Awake();
            BakedZeitspannen = BakeZeitspannenFromStundenData();
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return BakedZeitspannen.IsInside(TimeManager.GetCurrentWochentag(), TimeManager.GetCurrentTime());
        }

        public override void StartGoal()
        {
            base.StartGoal();
            LehrerArrived = false;
            SchuelerFreigestelltDieseStunde = false;
            StartCoroutine(GoToRoom());
            StartCoroutine(CheckAnwesenheit());
        }

        public IEnumerator GoToRoom()
        {
            Lehrer.Sprache.SetSatzSource(SaetzeAufDemWegZumRaum);
            Lehrer.Agent.destination = UnterrichtsRaum.GetLehrerStandpunkt().position;
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            yield return new WaitForSeconds(5);
            Lehrer.Sprache.SayRandomNow(SaetzeBegruessung);
            LehrerArrived = true;
            Lehrer.Sprache.SetSatzSource(SaetzeWaehrendUnterricht);
        }

        private IEnumerator CheckAnwesenheit()
        {
            while (true)
            {
                if (LehrerArrived && !UnterrichtsRaum.PlayerInside && !SchuelerFreigestelltDieseStunde)
                {
                    Lehrer.Reputation.AddReputation(ReputationsVerlustBeiFehlzeit);
                    yield break;
                }

                yield return null;
            }
        }

        private WoechentlicheZeitspannen BakeZeitspannenFromStundenData()
        {
            var result = new WoechentlicheZeitspannen();

            var wochentagEintrag = new WoechentlicheZeitspannen.WochentagEintrag
            {
                AuswahlArt = WochentagAuswahlArt.Manuell,
                Manuell = new[] {Stunde.Wochentag}
            };

            var zeitspanne = new WoechentlicheZeitspannen.WochentagEintrag.ZeitspanneEintrag();

            var anfang = new WoechentlicheZeitspannen.WochentagEintrag.ZeitspanneEintrag.Zeitpunkt
            {
                RelativZu = ZeitRelativitaet.FachAnfangN,
                RelativZuN = Stunde.FachIndex,
                Zeit = -ZeitPufferVorher
            };
            zeitspanne.Anfang = anfang;

            var ende = new WoechentlicheZeitspannen.WochentagEintrag.ZeitspanneEintrag.Zeitpunkt
            {
                RelativZu = ZeitRelativitaet.FachEndeN,
                RelativZuN = Stunde.FachIndex,
                Zeit = -Ueberziehungszeit
            };
            zeitspanne.Ende = ende;

            wochentagEintrag.Zeitspannen = new[] {zeitspanne};

            result.Wochentage = new[] {wochentagEintrag};

            return result;
        }

        public FachType GetFach()
        {
            return Fach;
        }

        public Klassenraum GetKlassenraum()
        {
            return UnterrichtsRaum;
        }

        public StundenData GetStunde()
        {
            return Stunde;
        }
    
        [Serializable]
        public class StundenData
        {
            public Wochentag Wochentag;
            public int FachIndex;
        }
    }
}