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
    public class SchultenBrain : BrainBase
    {
        [SerializeField] private Player.Player Player;
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private Klassenraum UnterrichtsRaum;
        [SerializeField] private Transform SchuleEingang;

        private readonly SaetzeMoeglichkeitenMehrmals SaetzeWegUnterricht = new(
            "Ich hoffe meine Schüler haben den Schimmelreiter gelesen"
        );

        private readonly SaetzeMoeglichkeitenEinmalig SaetzeUnterrichtBegruessung = new(
            "Gute Tag"
        );

        private readonly SaetzeMoeglichkeitenMehrmals SaetzeAngekommenUnterricht = new(
            "Jetzt mal ganz ehrlich, habt ihr den Schimmelreiter gelesen",
            "Jetzt mal ganz ehrlich, wir müssen jetzt pö a pö mit dem Schimmelreiter weitermachen",
            "Jetzt mal Hand aus Herz: Ihr findet den Schimmelreiter doch auch spannend"
        );

        protected override void RegisterGoals(AIController ai)
        {
            var zeitInDerSchule = new WoechentlicheZeitspannen(
                new WoechentlicheZeitspannen.Eintrag(
                    new EigenschaftWochentagAuswahl(WochentagEigenschaft.Schultag),
                    new WoechentlicheZeitspannen.Zeitspanne(
                        new WoechentlicheZeitspannen.Zeitpunkt(new SchuleBeginnZeitRelativitaet(), TimeUtility.MinutesToFloat(-30f)),
                        new WoechentlicheZeitspannen.Zeitpunkt(new SchuleEndeZeitRelativitaet(), TimeUtility.MinutesToFloat(30f))
                    )
                )
            );
            ai.AddGoal(new SchuleVerlassenGoal(
                lehrer: Lehrer,
                trigger: new CallbackTrigger(() => !zeitInDerSchule.IsInside(TimeManager.GetCurrentWochentag(), TimeManager.GetCurrentTime())),
                eingang: SchuleEingang.position,
                ausgang: SchuleEingang.position,
                new SaetzeMoeglichkeitenMehrmals(
                    "Jetz mal ganz ehrlich wir alle wollen auch mal frei haben"
                )
            ));

            ai.AddGoal(new ErschoepfungGoal(
                lehrer: Lehrer,
                maximaleDistanzProMinute: 30f,
                maxiamleHoeheProMinute: 15f,
                laengeDerPause: 5f,
                saetze: new SaetzeMoeglichkeitenMehrmals(
                    "Jetzt mal ganz ehrlich wir alle sind auch mal erschöpft",
                    "Hand aus Herz, wir alle brauchen auch mal eine Pause"
                )
            ));

            ai.AddGoal(new VerbrechenMeldenGoal(
                lehrer: Lehrer,
                trigger: new AlwaysTrueTrigger(),
                player: Player,
                schulleitungsBuero: SchuleEingang,
                saetzeWeg: new SaetzeMoeglichkeitenMehrmals(
                    "So geht das nicht weiter",
                    "Das gibt eine Verwarnung"
                ),
                saetzeAngekommenEinmalig: null,
                saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                    "Der Schüler muss gehen"
                )
            ));

            // Unterrichten
            ai.AddGoal(new UnterrichtenGoal(
                lehrer: Lehrer,
                unterrichtsRaum: UnterrichtsRaum,
                trigger: new ZeitspanneTrigger(TimeManager, new WoechentlicheZeitspannen(
                    new WoechentlicheZeitspannen.Eintrag(
                        new ManuelleWochentagAuswahl(Wochentag.Montag),
                        new WoechentlicheZeitspannen.Zeitspanne(
                            new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 0, AnfangOderEnde.Anfang), TimeUtility.MinutesToFloat(-5)),
                            new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 0, AnfangOderEnde.Ende), TimeUtility.MinutesToFloat(5))
                        )
                    )
                )),
                stundeImStundenplan: new UnterrichtenGoal.StundenData(Wochentag.Montag, 0, "Deutsch"),
                reputationsAenderungBeiFehlzeit: -0.1f,
                saetzeAufDemWegZumRaum: SaetzeWegUnterricht,
                saetzeBegruessung: SaetzeUnterrichtBegruessung,
                saetzeWaehrendUnterricht: SaetzeAngekommenUnterricht
            ));

            // Zum Unterrichtsraum gehen
            ai.AddGoal(new MoveToAndStandAtGoal(
                lehrer: Lehrer,
                trigger: new ZeitspanneTrigger(TimeManager, new WoechentlicheZeitspannen(
                    new WoechentlicheZeitspannen.Eintrag(
                        new EigenschaftWochentagAuswahl(WochentagEigenschaft.Schultag),
                        new WoechentlicheZeitspannen.Zeitspanne(
                            new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, AnfangOderEnde.Anfang), 0f),
                            new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, AnfangOderEnde.Ende), 0f)
                        ),
                        new WoechentlicheZeitspannen.Zeitspanne(
                            new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Lernzeit, AnfangOderEnde.Anfang), 0f),
                            new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Lernzeit, AnfangOderEnde.Ende), 0f)
                        )
                    )
                )),
                position: UnterrichtsRaum.GetLehrerStandpunkt(),
                saetzeAngekommen: SaetzeAngekommenUnterricht,
                saetzeWeg: SaetzeWegUnterricht,
                saetzeAngekommenEinmalig: SaetzeUnterrichtBegruessung
            ));

            // Rauchen gehen
            ai.AddGoal(new MoveToAndStandAtGoal(
                lehrer: Lehrer,
                trigger: new ZeitspanneTrigger(TimeManager, new WoechentlicheZeitspannen(
                    new WoechentlicheZeitspannen.Eintrag(
                        new EigenschaftWochentagAuswahl(WochentagEigenschaft.Schultag),
                        new WoechentlicheZeitspannen.Zeitspanne(
                            new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Kurzpause, AnfangOderEnde.Anfang), 0f),
                            new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Kurzpause, AnfangOderEnde.Ende), 0f)
                        ),
                        new WoechentlicheZeitspannen.Zeitspanne(
                            new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Mittagspause, AnfangOderEnde.Anfang), 0f),
                            new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Mittagspause, AnfangOderEnde.Ende), 0f)
                        ),
                        new WoechentlicheZeitspannen.Zeitspanne(
                            new WoechentlicheZeitspannen.Zeitpunkt(new SchuleBeginnZeitRelativitaet(), TimeUtility.MinutesToFloat(-30f)),
                            new WoechentlicheZeitspannen.Zeitpunkt(new SchuleBeginnZeitRelativitaet(), 0f)
                        )
                    )
                )),
                position: SchuleEingang.position,
                saetzeWeg: new SaetzeMoeglichkeitenMehrmals(
                    "Hand aufs Herz: Ich kann mir das Rauchen auch einfach pö a pö abgewöhnen",
                    "Jetzt mal ganz ehrlich, wir alle haben schon einmal geraucht"
                ),
                saetzeAngekommenEinmalig: null,
                saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                    "Rauchen am morgen vertreibt Kummer und sorgen",
                    "Jetzt mal ganz ehrlich, stoppt ihr die Zeit wie lange Ich rauche",
                    "Ich rauche nur gelegentlich"
                )
            ));
        }

        protected override void RegisterFragen(InteraktionsMenuFragenManager fragen)
        {
            fragen.AddFrage(new InteraktionsMenuFrageNaeheZufaellig(
                lehrer: Lehrer,
                player: Player,
                interaktionsMenuName: "Beleidigen",
                kosten: 0,
                annahmeWahrscheinlichkeit: 0f,
                reputationsAenderungBeiAnnahme: 0f,
                annahmeAntworten: null,
                reputationsAenderungBeiAblehnen: -1f,
                ablehnenAntworten: new SaetzeMoeglichkeitenEinmalig(
                    "Ich bitte dich"    
                )
            ));
            
            fragen.AddFrage(new InteraktionsMenuFrageNaeheZufaellig(
                lehrer: Lehrer,
                player: Player,
                interaktionsMenuName: "Geldgeschenk anbieten",
                kosten: 50,
                annahmeWahrscheinlichkeit: 0.5f,
                reputationsAenderungBeiAnnahme: 0.5f,
                annahmeAntworten: new SaetzeMoeglichkeitenEinmalig(
                    "Danke",
                    "Das halten wir aber unter uns"
                ),
                reputationsAenderungBeiAblehnen: -0.5f,
                ablehnenAntworten: new SaetzeMoeglichkeitenEinmalig(
                    "Nein danke",
                    "Das darf ich nicht annehmen"
                )
            ));
            
            fragen.AddFrage(new InteraktionsMenuFrageSchwaenzenZufaellig(
                lehrer: Lehrer,
                player: Player,
                interaktionsMenuName: "Fragen, aufs Klo zu gehen",
                kosten: 0,
                annahmeWahrscheinlichkeit:0.7f,
                reputationsAenderungBeiAnnahme: -0.1f,
                annahmeAntworten: new SaetzeMoeglichkeitenEinmalig(
                    "Ja, du kannst gehen"  
                ),
                reputationsAenderungBeiAblehnen: -0.2f,
                ablehnenAntworten: new SaetzeMoeglichkeitenEinmalig(
                    "Du kannst auch noch bis zum Ende der Stunde warten"
                )
            ));
        }
    }
}
