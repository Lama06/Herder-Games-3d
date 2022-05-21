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
        [SerializeField] private List<Transform> Patrolienroute;

        private readonly SaetzeMoeglichkeitenEinmalig UnterrichtBegruessung = new(
            "Guten Morgen 9d".ZufaelligGrossKlein(),
            "Hallo 9d".ZufaelligGrossKlein()
        );

        private readonly SaetzeMoeglichkeitenMehrmals Unterricht = new(
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

        protected override void RegisterGoals(AIController ai)
        {
            #region Unterrichtszeit
            
            // Zeitplan während Unterrichtszeit:
            // Montag: Unterricht, Steine, Steine, Unterricht
            // LangtagNormal: Tonkeller, Unterrichten, Steine
            // Kurztag: Unterrichten, Tonkeller, Unterrichten
            
            ai.AddGoal(new UnterrichtenGoal(
                lehrer: Lehrer,
                unterrichtsRaum: Klassenraum,
                standpunkt: UnterrichtsStandpunkt.position,
                trigger: new ZeitspanneTrigger(
                    TimeManager,
                    new WoechentlicheZeitspannen(
                        new WoechentlicheZeitspannen.Eintrag(
                            new ManuelleWochentagAuswahl(Wochentag.Montag),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 3, AnfangOderEnde.Anfang), 0f),
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 3, AnfangOderEnde.Ende), 0f)
                            )
                        )
                    )
                ),
                stundeImStundenplan: new UnterrichtenGoal.StundenData(Wochentag.Montag, 3, "Kunst (schlechtestes Fach der Welt)"),
                reputationsAenderungBeiFehlzeit: -1f,
                saetzeBegruessung: UnterrichtBegruessung,
                saetzeWaehrendUnterricht: Unterricht
            ));

            ai.AddGoal(new MoveToAndStandAtGoal( // Unterrichten
                lehrer: Lehrer,
                trigger: new ZeitspanneTrigger(
                    TimeManager,
                    new WoechentlicheZeitspannen(
                        new WoechentlicheZeitspannen.Eintrag(
                            new ManuelleWochentagAuswahl(Wochentag.Montag),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 0, AnfangOderEnde.Anfang), 0f),
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 0, AnfangOderEnde.Ende), 0f)
                            )
                        ),
                        new WoechentlicheZeitspannen.Eintrag(
                            new EigenschaftWochentagAuswahl(WochentagEigenschaft.LangtagNormal),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 1, AnfangOderEnde.Anfang), 0f),
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 1, AnfangOderEnde.Ende), 0f)
                            )
                        ),
                        new WoechentlicheZeitspannen.Eintrag(
                            new EigenschaftWochentagAuswahl(WochentagEigenschaft.Kurztag),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 0, AnfangOderEnde.Anfang), 0f),
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 0, AnfangOderEnde.Ende), 0f)
                            ),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 2, AnfangOderEnde.Anfang), 0f),
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 2, AnfangOderEnde.Ende), 0f)
                            )
                        )
                    )
                ),
                position: UnterrichtsStandpunkt.position,
                saetzeAngekommenEinmalig: UnterrichtBegruessung,
                saetzeAngekommen: Unterricht
            ));

            ai.AddGoal(new MoveToAndStandAtGoal( // Freistunde Steine zählen
                lehrer: Lehrer,
                trigger: new ZeitspanneTrigger(
                    TimeManager,
                    new WoechentlicheZeitspannen(
                        new WoechentlicheZeitspannen.Eintrag(
                            new ManuelleWochentagAuswahl(Wochentag.Montag),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 1, AnfangOderEnde.Anfang), 0f),
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 1, AnfangOderEnde.Ende), 0f)
                            ),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 2, AnfangOderEnde.Anfang), 0f),
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 2, AnfangOderEnde.Ende), 0f)
                            )
                        ),
                        new WoechentlicheZeitspannen.Eintrag(
                            new EigenschaftWochentagAuswahl(WochentagEigenschaft.LangtagNormal),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 2, AnfangOderEnde.Anfang), 0f),
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 2, AnfangOderEnde.Ende), 0f)
                            )
                        )
                    )
                ),
                position: MauerSteineZaehlen.position,
                saetzeAngekommen: new ZaehlenSaetze()
            ));

            ai.AddGoal(new MoveToAndStandAtGoal( // In den Tonkeller gehen
                lehrer: Lehrer,
                trigger: new ZeitspanneTrigger(
                    TimeManager,
                    new WoechentlicheZeitspannen(
                        new WoechentlicheZeitspannen.Eintrag(
                            new EigenschaftWochentagAuswahl(WochentagEigenschaft.Kurztag),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 1, AnfangOderEnde.Anfang), 0f),
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 1, AnfangOderEnde.Ende), 0f)
                            )
                        ),
                        new WoechentlicheZeitspannen.Eintrag(
                            new EigenschaftWochentagAuswahl(WochentagEigenschaft.LangtagNormal),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 0, AnfangOderEnde.Anfang), 0f),
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 0, AnfangOderEnde.Ende), 0f)
                            )
                        )
                    )
                ),
                position: Tonkeller.position,
                saetzeWeg: new SaetzeMoeglichkeitenMehrmals(
                    "Hey mach Platz, ich muss in den Tonkeller! Das ist frauenfeindlich! " +
                    "Ich bin lesbisch und ich stehe dazu! Ich bin eine wahre Feministin".ZufaelligGrossKlein()
                ),
                saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                    "Ups mir ist das Tonmodell hingefal...Äähm...Ich meine das Tonmodell ist im Ofen geplatzt und zu Staub zerfallen"
                )
            ));
            
            #endregion
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

            public (string satz, float? delay) GetNextSatz()
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
