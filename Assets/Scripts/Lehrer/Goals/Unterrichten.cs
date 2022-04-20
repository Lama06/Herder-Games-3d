using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HerderGames.AI;
using HerderGames.Lehrer.Sprache;
using HerderGames.Player;
using HerderGames.Time;
using HerderGames.Util;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    public class Unterrichten : LehrerGoalBase
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private Player.Player Player;
        
        [Header("Allgemein")]
        [SerializeField] private Klassenraum UnterrichtsRaum;
        [SerializeField] private FachType Fach;
        [SerializeField] private Saetze SaetzeAufDemWegZumRaum;
        [SerializeField] private Saetze SaetzeWaehrendUnterricht;
        
        [Header("Wann")]
        [SerializeField] private StundenData[] Stunden;
        [SerializeField] private float ZeitPufferVorher;
        [SerializeField] private float Ueberziehungszeit;
        
        [Header("Reputation")]
        [SerializeField] private float ReputationsVerlustBeiFehlzeit;
        
        [Header("Auf Klo gehen")]
        [SerializeField] private float ErlaubtKloPercent;
        [SerializeField] private Saetze KloErlaubtResponse;
        [SerializeField] private Saetze KloNichtErlaubtResponse;
        [SerializeField] private float ReputationsVerlustBeiKlo;
        
        [Header("Krankheit vortäuschen")]
        [SerializeField] private float ErlaubtFehlenWegenKrankheitPercent;
        [SerializeField] private Saetze KrankheitErlaubtResponse;
        [SerializeField] private Saetze KrankheitNichtErlaubtResponse;
        [SerializeField] private float ReputationsVerlustBeiKrankheit;

        private WoechentlicheZeitspannen BakedZeitspannen;
        private bool LehrerArrived;
        private bool SchuelerFreigestelltDieseStunde;
        private List<int> InteraktionsMenuIds;

        protected override void Awake()
        {
            base.Awake();
            BakedZeitspannen = BakeZeitspannenFromStundenData();
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return BakedZeitspannen.IsInside(TimeManager.GetCurrentWochentag(), TimeManager.GetCurrentTime());
        }

        public override void OnStarted()
        {
            LehrerArrived = false;
            SchuelerFreigestelltDieseStunde = false;
            InteraktionsMenuIds = new List<int>();
            StartCoroutine(GoToRoom());
            StartCoroutine(CheckAnwesenheit());
            StartCoroutine(ManageInteraktionsMenu());
        }

        public override void OnEnd(GoalEndReason reason)
        {
            foreach (var id in InteraktionsMenuIds)
            {
                Player.InteraktionsMenu.RemoveEintrag(id);
            }
        }

        public IEnumerator GoToRoom()
        {
            Lehrer.Sprache.SetSatzSource(SaetzeAufDemWegZumRaum);
            Lehrer.Agent.destination = UnterrichtsRaum.GetLehrerStandpunkt().position;
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
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

        private IEnumerator ManageInteraktionsMenu()
        {
            InteraktionsMenuIds = new List<int>();
            var hasEintraege = false;

            while (true)
            {
                if (LehrerArrived && UnterrichtsRaum.PlayerInside && !hasEintraege)
                {
                    InteraktionsMenuIds.Add(Player.InteraktionsMenu.AddEintrag(new InteraktionsMenuEintrag
                    {
                        Name = "Fragen, auf die Toilette gehen zu dürfen",
                        Callback = id =>
                        {
                            if (Utility.TrueWithPercent(ErlaubtKloPercent))
                            {
                                SchuelerFreigestelltDieseStunde = true;
                                Lehrer.Sprache.SayRandomNow(KloErlaubtResponse);
                            }
                            else
                            {
                                Lehrer.Sprache.SayRandomNow(KloNichtErlaubtResponse);
                            }

                            Lehrer.Reputation.AddReputation(ReputationsVerlustBeiKlo);
                        }
                    }));

                    InteraktionsMenuIds.Add(Player.InteraktionsMenu.AddEintrag(new InteraktionsMenuEintrag
                    {
                        Name = "Krankheit vortäuschen",
                        Callback = id =>
                        {
                            if (Utility.TrueWithPercent(ErlaubtFehlenWegenKrankheitPercent))
                            {
                                SchuelerFreigestelltDieseStunde = true;
                                Lehrer.Sprache.SayRandomNow(KrankheitErlaubtResponse);
                            }
                            else
                            {
                                Lehrer.Sprache.SayRandomNow(KrankheitNichtErlaubtResponse);
                            }

                            Lehrer.Reputation.AddReputation(ReputationsVerlustBeiKrankheit);
                        }
                    }));

                    hasEintraege = true;
                }

                if (!UnterrichtsRaum.PlayerInside && hasEintraege)
                {
                    foreach (var id in InteraktionsMenuIds)
                    {
                        Player.InteraktionsMenu.RemoveEintrag(id);
                    }

                    hasEintraege = false;
                }

                yield return null;
            }
        }
        
        private WoechentlicheZeitspannen BakeZeitspannenFromStundenData()
        {
            var result = new WoechentlicheZeitspannen();

            var wochentageEintraege = new List<WoechentlicheZeitspannen.WochentagEintrag>();
            foreach (var wochentag in Enum.GetValues(typeof(Wochentag)).Cast<Wochentag>())
            {
                var wochentagEintrag = new WoechentlicheZeitspannen.WochentagEintrag
                {
                    AuswahlArt = WochentagAuswahlArt.Manuell,
                    Manuell = new[] {wochentag}
                };

                var zeitspannen = new List<WoechentlicheZeitspannen.WochentagEintrag.ZeitspanneEintrag>();
                foreach (var stunde in Stunden)
                {
                    if (stunde.Wochentag != wochentag)
                    {
                        continue;
                    }

                    var zeitspanne = new WoechentlicheZeitspannen.WochentagEintrag.ZeitspanneEintrag();
                    
                    var anfang = new WoechentlicheZeitspannen.WochentagEintrag.ZeitspanneEintrag.Zeitpunkt
                    {
                        RelativZu = ZeitRelativitaet.FachAnfangN,
                        RelativZuN = stunde.FachIndex,
                        Zeit = -ZeitPufferVorher
                    };
                    zeitspanne.Anfang = anfang;
                    
                    var ende = new WoechentlicheZeitspannen.WochentagEintrag.ZeitspanneEintrag.Zeitpunkt
                    {
                        RelativZu = ZeitRelativitaet.FachEndeN,
                        RelativZuN = stunde.FachIndex,
                        Zeit = -Ueberziehungszeit
                    };
                    zeitspanne.Ende = ende;
                    
                    zeitspannen.Add(zeitspanne);
                }

                wochentagEintrag.Zeitspannen = zeitspannen.ToArray();

                wochentageEintraege.Add(wochentagEintrag);
            }

            result.Wochentage = wochentageEintraege.ToArray();

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

        public StundenData[] GetStunden()
        {
            return Stunden;
        }

        [System.Serializable]
        public class StundenData
        {
            public Wochentag Wochentag;
            public int FachIndex;
        }
    }
}