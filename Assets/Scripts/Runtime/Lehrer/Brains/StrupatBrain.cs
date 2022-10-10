using System.Collections.Generic;
using HerderGames.Lehrer.AI;
using HerderGames.Lehrer.AI.Goals;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Fragen;
using HerderGames.Lehrer.Sprache;
using HerderGames.Schule;
using HerderGames.Util;
using HerderGames.Zeit;
using UnityEngine;

namespace HerderGames.Lehrer.Brains
{
    public class StrupatBrain : BrainBase
    {
        [SerializeField] private Player.Player Player;
        [SerializeField] private Klassenraum Klassenraum;
        [SerializeField] private Transform UnterrichtsStandpunkt;
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private Transform MauerSteineZaehlen;
        [SerializeField] private Transform Tonkeller;
        [SerializeField] private Transform Buecherregal;
        [SerializeField] private Transform Lehrerzimmer;
        [SerializeField] private List<Transform> Patrolienroute;

        protected override void RegisterGoals(AIController ai)
        {
            #region Sätze

            var unterrichtBegruessung = new SaetzeMoeglichkeitenEinmalig(
                "Guten Morgen 9d".ZufaelligGrossKlein(),
                "Hallo 9d".ZufaelligGrossKlein()
            );

            var unterricht = new SaetzeMoeglichkeitenMehrmals(
                "Gock Gock Gock Gock. Ich bin ein Huhn. Ich hab ein EI gelegt!".ZufaelligGrossKlein(),
                "Hey??? Trinkst du da etwa im Kunstraum?? Das ist frauenfeindlich!!".ZufaelligGrossKlein(),
                "Dein Tonmodell ist heil rausgekommen: Drei Minus! Dein Tonmodell ist expodiert und zu Staub zerfallen: Eins Plus Super gemacht!".ZufaelligGrossKlein(),
                "HEY! DAS IST FRAUENFEINDLICH",
                "NEIN!!! ICH BEANTWORTE DIR DEINE FRAGE NICHT!!!!",
                "ICH BIN EINE LESBISCH, EINE HARDCORE FEMINISTIN, KOMUNISTIN UND WILL DEN SYSTEMUNSTURZ UND ICH STEHE DAZU!!!!",
                "GEH GLEICH IN DEN T-RAUM!!!",
                "Kunst ist sinnlos!".ZufaelligGrossKlein(),
                "Kunst ist Dreck!".ZufaelligGrossKlein()
            );

            #endregion

            void Unterrichten(WoechentlicheZeitspannen wann, UnterrichtenGoal.StundenData stunde)
            {
                ai.AddGoal(new UnterrichtenGoal(
                    lehrer: Lehrer,
                    unterrichtsRaum: Klassenraum,
                    standpunkt: UnterrichtsStandpunkt.position,
                    trigger: new ZeitspanneTrigger(TimeManager, wann),
                    stundeImStundenplan: stunde,
                    reputationsAenderungBeiFehlzeit: -1f,
                    saetzeBegruessung: unterrichtBegruessung,
                    saetzeWaehrendUnterricht: unterricht
                ));
            }

            void FakeUnterrichten(WoechentlicheZeitspannen wann)
            {
                ai.AddGoal(new MoveToAndStandAtGoal(
                    lehrer: Lehrer,
                    trigger: new ZeitspanneTrigger(TimeManager, wann),
                    position: UnterrichtsStandpunkt.position,
                    saetzeAngekommenEinmalig: unterrichtBegruessung,
                    saetzeAngekommen: unterricht
                ));
            }

            void SteineZaehlen(WoechentlicheZeitspannen wann)
            {
                ai.AddGoal(new MoveToAndStandAtGoal( // Freistunde Steine zählen
                    lehrer: Lehrer,
                    trigger: new ZeitspanneTrigger(TimeManager, wann),
                    position: MauerSteineZaehlen.position,
                    saetzeWeg: new SaetzeMoeglichkeitenMehrmals(
                        "Ich geh jetzt Steine zählen damit ich meinen Schülern eine Sechs geben kann wenn sie sich verzählen HAHAHA".ZufaelligGrossKlein()
                    ),
                    saetzeAngekommen: new ZaehlenSaetze()
                ));
            }

            void InTonkellerGehen(WoechentlicheZeitspannen wann)
            {
                ai.AddGoal(new MoveToAndStandAtGoal(
                    lehrer: Lehrer,
                    trigger: new ZeitspanneTrigger(TimeManager, wann),
                    position: Tonkeller.position,
                    saetzeWeg: new SaetzeMoeglichkeitenMehrmals(
                        "Hey mach Platz, ich muss in den Tonkeller! Das ist frauenfeindlich! " +
                        "Ich bin lesbisch und ich stehe dazu! Ich bin eine wahre Feministin".ZufaelligGrossKlein()
                    ),
                    saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                        "Ups mir ist das Tonmodell hingefal...Äähm...Ich meine das Tonmodell ist im Ofen geplatzt und zu Staub zerfallen"
                    )
                ));
            }

            void ImRaumBuecherLesen(WoechentlicheZeitspannen wann)
            {
                ai.AddGoal(new MoveToAndStandAtGoal(
                    lehrer: Lehrer,
                    trigger: new ZeitspanneTrigger(TimeManager, wann),
                    position: Buecherregal.position,
                    saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                        "Warum fehlt denn eine Seite in meinem wichtigsten Buch?!".ZufaelligGrossKlein()
                    )
                ));
            }

            void InsLehrerzimmerGehen(WoechentlicheZeitspannen wann)
            {
                ai.AddGoal(new MoveToAndStandAtGoal(
                    lehrer: Lehrer,
                    trigger: new ZeitspanneTrigger(TimeManager, wann),
                    position: Lehrerzimmer.position
                ));
            }

            void PatrolienRouteGehen(WoechentlicheZeitspannen wann)
            {
                
            }
        }

        protected override void RegisterFragen(InteraktionsMenuFragenManager fragen)
        {
            fragen.AddFrage(new InteraktionsMenuFrageEinfach(
                lehrer: Lehrer,
                player: Player,
                shouldShowPredicate: InteraktionsMenuFrageUtil.ShowNearby(),
                interaktionsMenuName: "Fragen, was wir heute im Unterricht machen",
                reputationsAenderung: -1f,
                antworten: new SaetzeMoeglichkeitenEinmalig(
                    "Hey! Wie wagst du es mir eine Frage zu stellen??! Das ist frauenfeindlich!!!".ZufaelligGrossKlein()
                )
            ));

            fragen.AddFrage(new InteraktionsMenuFrageEinfach(
                lehrer: Lehrer,
                player: Player,
                shouldShowPredicate: InteraktionsMenuFrageUtil.ShowNearby(),
                interaktionsMenuName: "Moralfrage stellen (Achtung: Lebensgefahr)",
                reputationsAenderung: -1f,
                antworten: new SaetzeMoeglichkeitenEinmalig(
                    "Wie kannst du es wagen? Ich werde dich und deine Familie verbrennen!! Keine Moralfragen in meinem Unterricht!!!!".ZufaelligGrossKlein()
                )
            ));
        }

        private class ZaehlenSaetze : ISaetzeMoeglichkeitenMehrmals
        {
            private int Current;

            public (string satz, float? delay) NextSatz
            {
                get
                {
                    if (Random.Range(0, 100) == 0)
                    {
                        Current = 0;
                        return ("Mist jetzt hab ich mich verzählt! Das ist frauenfeindlich!!".ZufaelligGrossKlein(), 5f);
                    }

                    Current++;
                    if (Current % 6 == 0) // Frau Strupat kann nicht zählen
                    {
                        Current++;
                    }

                    return (Current.ToString(), 1);
                }
            }
        }
    }
}
