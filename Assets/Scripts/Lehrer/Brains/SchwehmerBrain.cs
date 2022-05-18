using HerderGames.Lehrer.AI;
using HerderGames.Lehrer.AI.Goals;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Fragen;
using HerderGames.Lehrer.Sprache;
using HerderGames.Schule;
using HerderGames.Zeit;
using UnityEngine;

namespace HerderGames.Lehrer.Brains
{
    public class SchwehmerBrain : BrainBase
    {
        [SerializeField] private Player.Player Player;
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private Transform RauchenVorne;
        [SerializeField] private Transform RauchenHinten;
        [SerializeField] private Klassenraum UnterrichtsRaum;
        [SerializeField] private Transform SchuleEingang;
        [SerializeField] private VergiftbaresEssen Kaffeemaschine;
        [SerializeField] private Transform Sammelpunkt;
        [SerializeField] private AlarmManager AlarmManager;
        [SerializeField] private Transform Toilette;
        [SerializeField] private Transform EingangFahhradKontrolle;

        private readonly SaetzeMoeglichkeitenMehrmals RauchenWeg = new(
            "Ich hab ja noch 5 Minuten, da kann ich ja noch eine Rauchen gehen"
        );

        private readonly SaetzeMoeglichkeitenMehrmals Rauchen = new(
            "Macht euch keine Sorgen: Ich rauche ja nur gelegentlich",
            "Nehmt mich bitte nicht als Vorbild, was das Rauchen angeht",
            "Hast du mal nen Fünfziger, Ich hab mein Feuerzeug gerade nicht dabei",
            "Ich rauche ja nicht auf Lunge",
            "Es ist so vieles ungesund ... und ich rauche ja auch schon seit Cäsars tot und lebe immer noch"
        );

        private readonly SaetzeMoeglichkeitenMehrmals UnterrichtWeg = new(
            "Ich muss mich beilen. Der Unterricht beginnt ja gleich und Ich muss heute noch bis Lektion 1000 kommen"
        );

        private readonly SaetzeMoeglichkeitenEinmalig UnterrichtBegruessung = new("Moin Moin");

        private readonly SaetzeMoeglichkeitenMehrmals Unterricht = new(
            "Denkt dran: Jeden Tag ein bischen",
            "Lieber weniger sorgfältig als mehr sorgfältig!",
            "Ihr müsst die verdammten Prädikate lernen!",
            "Stellt Fragen!",
            "Jetzt kommt der Lösungsblätterabholservice!",
            "Ich empfehele allen in diesem Kurs einen Latein LK zu wählen",
            "Wenn ihr gute Menschen sein wollt, dann wählt in der EF Latein, Informatik und Altgrieschich",
            "Wenn ihr nochmal zu spät kommt, werde ich zu einem bösen Drachen und fresse euch mit Haut und Haaren",
            "Ich hatte euch doch heute ein Vokabelvergnügen versprochen, oder?",
            "Hey! Weg mit deiner Stiftspitze von meiner Tischoberfläche!",
            "Die Gier des Goldes",
            "Ich lass mich einsargen!",
            "Ihr lernt hier vor allem Deutsch",
            "Ich könnte mich zwar mit den Römern unterhalen aber in die Antike zurück will ich trotzdem nicht: Da gab es keine Zahärtzte"
        );

        private readonly SaetzeMoeglichkeitenMehrmals UnterrichtWegKrank = new(
            "Ich glaube es geht mir so schlecht weil ich heute so wenig geraucht habe"
        );

        private readonly SaetzeMoeglichkeitenMehrmals UnterrichtKrank = new(
            "Mir geht es heute gar nicht gut, macht bitte im Lernplan weiter",
            "Ich muss mir irgendwie den Magen verstimmt haben",
            "Ich bräuchte mal eine Feder"
        );

        protected override void RegisterGoals(AIController ai)
        {
            var zeitInDerSchule = new WoechentlicheZeitspannen(
                new WoechentlicheZeitspannen.Eintrag(
                    new EigenschaftWochentagAuswahl(WochentagEigenschaft.Schultag),
                    new WoechentlicheZeitspannen.Zeitspanne(
                        new WoechentlicheZeitspannen.Zeitpunkt(new SchuleBeginnZeitRelativitaet(), -1.5f),
                        new WoechentlicheZeitspannen.Zeitpunkt(new SchuleEndeZeitRelativitaet(), 0f)
                    )
                )
            );
            ai.AddGoal(new SchuleVerlassenGoal(
                lehrer: Lehrer,
                trigger: new CallbackTrigger(() => !zeitInDerSchule.IsInside(TimeManager)),
                eingang: SchuleEingang.position,
                ausgang: SchuleEingang.position
            ));

            ai.AddGoal(new MoveToAndStandAtGoal(
                lehrer: Lehrer,
                trigger: new CallbackTrigger(() => AlarmManager.IsAlarm()),
                position: Sammelpunkt.position,
                saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                    "Ich hoffe die Lateinbücher werdens überleben",
                    "Ich hoffe die Lernplanlösungen werdens überleben"
                )
            ));

            ai.AddGoal(new SchuleVerlassenGoal(
                lehrer: Lehrer,
                trigger: new CallbackTrigger(() => Lehrer.Vergiftung is {Syntome: true, VergiftungsType: VergiftungsType.Orthamol}),
                eingang: Toilette.position,
                ausgang: Toilette.position,
                saetzeBeimVerlassen: new SaetzeMoeglichkeitenMehrmals(
                    "Ich muss mal dringend auf Toilette"
                )
            ));

            // Unterrichten

            var unterrichtZeit = new WoechentlicheZeitspannen(
                new WoechentlicheZeitspannen.Eintrag(
                    new ManuelleWochentagAuswahl(Wochentag.Montag),
                    new WoechentlicheZeitspannen.Zeitspanne(
                        new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 2, AnfangOderEnde.Anfang), TimeUtility.MinutesToFloat(-5f)),
                        new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 2, AnfangOderEnde.Ende), TimeUtility.MinutesToFloat(6f))
                    )
                )
            );
            var unterrichtZeitAllgemein = new WoechentlicheZeitspannen(
                new WoechentlicheZeitspannen.Eintrag(
                    new ManuelleWochentagAuswahl(Wochentag.Montag),
                    new WoechentlicheZeitspannen.Zeitspanne(
                        new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, AnfangOderEnde.Anfang), TimeUtility.MinutesToFloat(-5f)),
                        new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, AnfangOderEnde.Ende), TimeUtility.MinutesToFloat(6f))
                    )
                )
            );

            ai.AddGoal(new UnterrichtenGoal(
                lehrer: Lehrer,
                unterrichtsRaum: UnterrichtsRaum,
                trigger: new CallbackTrigger(() => unterrichtZeit.IsInside(TimeManager) && Lehrer.Vergiftung is {Syntome: true, VergiftungsType: VergiftungsType.Normal}),
                stundeImStundenplan: new UnterrichtenGoal.StundenData(Wochentag.Montag, 2, "Latein"),
                reputationsAenderungBeiFehlzeit: -0.4f,
                saetzeAufDemWegZumRaum: UnterrichtWegKrank,
                saetzeBegruessung: UnterrichtBegruessung,
                saetzeWaehrendUnterricht: UnterrichtKrank
            ));

            ai.AddGoal(new MoveToAndStandAtGoal(
                lehrer: Lehrer,
                position: UnterrichtsRaum.GetLehrerStandpunkt(),
                trigger: new CallbackTrigger(() => unterrichtZeitAllgemein.IsInside(TimeManager) && Lehrer.Vergiftung is {Syntome: true, VergiftungsType: VergiftungsType.Normal}),
                saetzeWeg: UnterrichtWegKrank,
                saetzeAngekommenEinmalig: UnterrichtBegruessung,
                saetzeAngekommen: UnterrichtKrank
            ));

            ai.AddGoal(new UnterrichtenGoal(
                lehrer: Lehrer,
                unterrichtsRaum: UnterrichtsRaum,
                trigger: new CallbackTrigger(() => unterrichtZeit.IsInside(TimeManager)),
                stundeImStundenplan: new UnterrichtenGoal.StundenData(Wochentag.Montag, 2, "Latein"),
                reputationsAenderungBeiFehlzeit: -0.4f,
                saetzeAufDemWegZumRaum: UnterrichtWeg,
                saetzeBegruessung: UnterrichtBegruessung,
                saetzeWaehrendUnterricht: Unterricht
            ));

            ai.AddGoal(new MoveToAndStandAtGoal(
                lehrer: Lehrer,
                position: UnterrichtsRaum.GetLehrerStandpunkt(),
                trigger: new CallbackTrigger(() => unterrichtZeitAllgemein.IsInside(TimeManager)),
                saetzeWeg: UnterrichtWeg,
                saetzeAngekommenEinmalig: UnterrichtBegruessung,
                saetzeAngekommen: Unterricht
            ));

            // Pausen

            ai.AddGoal(new VergiftbaresEssenEssenGoal(
                lehrer: Lehrer,
                trigger: new CallbackTrigger(() =>
                {
                    var zeit = new WoechentlicheZeitspannen(
                        new WoechentlicheZeitspannen.Eintrag(
                            new EigenschaftWochentagAuswahl(WochentagEigenschaft.Schultag),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Kurzpause, AnfangOderEnde.Anfang), 0f),
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Kurzpause, AnfangOderEnde.Ende), 0f)
                            ),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Mittagspause, AnfangOderEnde.Anfang), 0f),
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Mittagspause, AnfangOderEnde.Ende), 0f)
                            )
                        )
                    );

                    return zeit.IsInside(TimeManager) && Lehrer.Vergiftung is {Syntome: true, VergiftungsType: VergiftungsType.Normal};
                }),
                vergiftbaresEssen: Kaffeemaschine,
                saetzeWeg: new SaetzeMoeglichkeitenMehrmals(
                    "Ein Kaffee sollte mich wieder auf Trab bringen",
                    "Ein Kaffee sollte Wunder wirken",
                    "Der Kaffee wird mich heilen"
                )
            ));

            ai.AddGoal(new MoveToAndStandAtGoal(
                lehrer: Lehrer,
                trigger: new ZeitspanneTrigger(
                    TimeManager,
                    new WoechentlicheZeitspannen(
                        new WoechentlicheZeitspannen.Eintrag(
                            new EigenschaftWochentagAuswahl(WochentagEigenschaft.Schultag),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Kurzpause, AnfangOderEnde.Anfang), 0f),
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Kurzpause, AnfangOderEnde.Anfang), TimeUtility.MinutesToFloat(15f))
                            ),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Mittagspause, AnfangOderEnde.Anfang), 0f),
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Mittagspause, AnfangOderEnde.Anfang), TimeUtility.MinutesToFloat(30f))
                            )
                        )
                    )
                ),
                position: RauchenVorne.position,
                saetzeWeg: RauchenWeg,
                saetzeAngekommen: Rauchen
            ));

            ai.AddGoal(new MoveToAndStandAtGoal(
                lehrer: Lehrer,
                trigger: new ZeitspanneTrigger(
                    TimeManager,
                    new WoechentlicheZeitspannen(
                        new WoechentlicheZeitspannen.Eintrag(
                            new EigenschaftWochentagAuswahl(WochentagEigenschaft.Schultag),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Kurzpause, AnfangOderEnde.Anfang), TimeUtility.MinutesToFloat(15f)),
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Kurzpause, AnfangOderEnde.Ende), 0f)
                            ),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Mittagspause, AnfangOderEnde.Anfang), TimeUtility.MinutesToFloat(30f)),
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Mittagspause, AnfangOderEnde.Ende), 0f)
                            )
                        )
                    )
                ),
                position: RauchenHinten.position,
                saetzeWeg: RauchenWeg,
                saetzeAngekommen: Rauchen
            ));

            // Vor der Schule

            ai.AddGoal(new VergiftbaresEssenEssenGoal(
                lehrer: Lehrer,
                trigger: new ZeitspanneTrigger(
                    TimeManager,
                    new WoechentlicheZeitspannen(
                        new WoechentlicheZeitspannen.Eintrag(
                            new EigenschaftWochentagAuswahl(WochentagEigenschaft.Schultag),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new SchuleBeginnZeitRelativitaet(), -1.5f),
                                new WoechentlicheZeitspannen.Zeitpunkt(new SchuleBeginnZeitRelativitaet(), -1f)
                            )
                        )
                    )
                ),
                vergiftbaresEssen: Kaffeemaschine,
                saetzeWeg: new SaetzeMoeglichkeitenMehrmals(
                    "Ein Kaffe am Morgen hat noch niemandem geschadet"
                )
            ));

            ai.AddGoal(new MoveToAndStandAtGoal(
                lehrer: Lehrer,
                trigger: new ZeitspanneTrigger(
                    TimeManager,
                    new WoechentlicheZeitspannen(
                        new WoechentlicheZeitspannen.Eintrag(
                            new EigenschaftWochentagAuswahl(WochentagEigenschaft.Schultag),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new SchuleBeginnZeitRelativitaet(), -1f),
                                new WoechentlicheZeitspannen.Zeitpunkt(new SchuleBeginnZeitRelativitaet(), -0.5f)
                            )
                        )
                    )
                ),
                position: RauchenVorne.position,
                saetzeWeg: RauchenWeg,
                saetzeAngekommen: Rauchen
            ));

            ai.AddGoal(new MoveToAndStandAtGoal(
                lehrer: Lehrer,
                trigger: new ZeitspanneTrigger(
                    TimeManager,
                    new WoechentlicheZeitspannen(
                        new WoechentlicheZeitspannen.Eintrag(
                            new EigenschaftWochentagAuswahl(WochentagEigenschaft.Schultag),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new SchuleBeginnZeitRelativitaet(), -0.5f),
                                new WoechentlicheZeitspannen.Zeitpunkt(new SchuleBeginnZeitRelativitaet(), 0f)
                            )
                        )
                    )
                ),
                position: EingangFahhradKontrolle.position,
                saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                    "Absteigen bitte!",
                    "Bitte nicht auf dem Schulhof mit dem Drahtesel fahren!"
                )
            ));
        }

        protected override void RegisterFragen(InteraktionsMenuFragenManager fragen)
        {
            fragen.AddFrage(new InteraktionsMenuFrageEinfach(
                lehrer: Lehrer,
                player: Player,
                shouldShowPredicate: InteraktionsMenuFrageUtil.ShowNearby(),
                interaktionsMenuName: "Fragen, was wir heute machen",
                antworten: new SaetzeMoeglichkeitenEinmalig(
                    "Wir machen immer etwas schönes!"
                )
            ));
            
            fragen.AddFrage(new InteraktionsMenuFrageEinfach(
                lehrer: Lehrer,
                player: Player,
                shouldShowPredicate: InteraktionsMenuFrageUtil.ShowNearby(),
                interaktionsMenuName: "Husten nachspielen",
                reputationsAenderung: -0.6f,
                antworten: new SaetzeMoeglichkeitenEinmalig("STOP! ... Und ja, es trifft mich!")
            ));
        }
    }
}
