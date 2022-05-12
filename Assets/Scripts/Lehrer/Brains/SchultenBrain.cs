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
    public class SchultenBrain : BrainBase
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private AlarmManager AlarmManager;
        [SerializeField] private Player.Player Player;
        [SerializeField] private Klassenraum UnterrichtsRaum;
        [SerializeField] private Transform SchuleHaupteingang;
        [SerializeField] private Transform AlarmSammelpunkt;
        [SerializeField] private Transform ToiletteLehrerzimmer;
        [SerializeField] private VergiftbaresEssen KaffeemaschineMiniRaum;
        [SerializeField] private Transform LehrerzimmerFach;
        [SerializeField] private Transform Rauchen;
        [SerializeField] private Transform Schulleitung;

        private readonly SaetzeMoeglichkeitenMehrmals UnterrichtWeg = new(
            "Ich hoffe meine Schüler haben den Schimmelreiter gelesen",
            "Jetzt mal ganz ehrlich: Treppensteigen war auch mal einfacher"
        );

        private readonly SaetzeMoeglichkeitenEinmalig UnterrichtBegruessung = new(
            "Na ihr Hasis?"
        );

        private readonly SaetzeMoeglichkeitenMehrmals UnterrichtSaetze = new(
            "Jetzt mal ganz ehrlich: Wer hat den Schimmelreiter fertig gelesen",
            "Weiß jemand von euch wie der OHP angeht? Mit solcher modernen Technik bin Ich nicht vertraut",
            "Der Klassenraum sieht heute aber wieder aus. Schmutzfinke!",
            "An alle Damen und Herren der Schöpfung: Wie hat euch das Buch gefallen?",
            "Als nächstes lesen wie Gedichte von Göthe der Flöte",
            "Der PC geht schon wieder nicht an. Das ist ja zum Mäuse melken!",
            "Das ist mir wumpe",
            "Wir müssen jeztzt pö a pö die Stilmittel durcharbeiten",
            "Hand aufs Herz: Warum habt ihr das Buch noch nicht gelsen? Ihr wisst doch wie der Hase läuft?",
            "Nehmt auf dem Ausflug bitte genug Fressalien mit",
            "Warum habt ihr das nicht aufgeschrieben? Ich hab euch das doch gerade in die Feder hineindiktiert",
            "Wie der Franzose sage Hälfte Hälfte",
            "Als ich in der neuten am Gymnasium war, hab ich sowas gewusst",
            "Jetzt mal unter uns: Hat jemand von euch ncoh schwarzen Pfeffer zu Hause",
            "Das ist ein schöner Alt deutscher Begriff",
            "Hauke Haien: Der Teufel himself",
            "Ihr wollt wissen, was in der Arbeit drankommt? Das weiß ich selber nicht, ich tue aber so und meine, dass es offensichtlich wäre",
            "Dar würde ich mir einen grünen Stift nehmen und das zwei mal unterstreichen",
            "25 durch 5? Nach Adam Riese wären das ... 4",
            "24 - 2 ... Das sind Summa Sumarum ... 21",
            "In Mathe war Ich damals nie gut",
            "Jetzt bist du im richtigen Boot ... oder Pferd",
            "Das ist ja zum Mäuse melken!"
        );

        private readonly SaetzeMoeglichkeitenMehrmals RauchenWeg = new(
            "Hand aufs Herz: Wir alle wollen auch mal Rauchen",
            "Jetzt mal ganz ehrlich: Ich muss noch mal kurz Rauchen gehen",
            "Jetzt mal ganz ehrlich: Rauchen kann auch gut für die Gesundheit sein"
        );

        private readonly SaetzeMoeglichkeitenMehrmals RauchenAngekommen = new(
            "Jetzt müsst ihr leider zugucken wie ich rauche weil ich meine Sucht nicht für eine Minute länger unterdrücken kann"
        );

        private readonly SaetzeMoeglichkeitenMehrmals KaffeeWeg = new(
            "Hand aufs Herz: Kaffee am morgen vertreibt Kummer und Sorgen",
            "Ich hoffe die Kaffeemaschine funktioniert noch",
            "Ihr fragt euch warum ich einen eigenen mini Raum habe? Naja irgendwo müssen die Zigaretten ja hin"
        );

        private readonly SaetzeMoeglichkeitenMehrmals KaffeeAngekommen = new(
            "Jetzt mal ganz ehrlich: Kaffee tut gut!",
            "Jetzt mal ganz ehrlich: Wir alle trinken auch mal Kaffee"
        );

        protected override void RegisterGoals(AIController ai)
        {
            var zeitInSchule = new WoechentlicheZeitspannen(
                new WoechentlicheZeitspannen.Eintrag(
                    new EigenschaftWochentagAuswahl(WochentagEigenschaft.Schultag),
                    new WoechentlicheZeitspannen.Zeitspanne(
                        new WoechentlicheZeitspannen.Zeitpunkt(new SchuleBeginnZeitRelativitaet(), -2f),
                        new WoechentlicheZeitspannen.Zeitpunkt(new SchuleEndeZeitRelativitaet(), 0.5f)
                    )
                )
            );
            ai.AddGoal(new SchuleVerlassenGoal(
                lehrer: Lehrer,
                trigger: new CallbackTrigger(() => !zeitInSchule.IsInside(TimeManager)),
                eingang: SchuleHaupteingang.position,
                ausgang: SchuleHaupteingang.position,
                saetzeBeimVerlassen: new SaetzeMoeglichkeitenMehrmals(
                    "Jetzt mal ganz ehrlich, Wir alle wollen auch mal frei haben!"
                )
            ));

            ai.AddGoal(new SchuleVerlassenGoal( // Krankheit Normal
                lehrer: Lehrer,
                trigger: new CallbackTrigger(() => Lehrer.Vergiftung.Syntome && Lehrer.Vergiftung.VergiftungsType == VergiftungsType.Normal),
                eingang: SchuleHaupteingang.position,
                ausgang: SchuleHaupteingang.position
            ));

            ai.AddGoal(new MoveToAndStandAtGoal( // Feueralarm
                lehrer: Lehrer,
                trigger: new CallbackTrigger(() => AlarmManager.IsAlarm()),
                position: AlarmSammelpunkt.position,
                saetzeWeg: new SaetzeMoeglichkeitenMehrmals(
                    "Hilfeee!!!",
                    "Rettet mich!!!",
                    "Es brennt!!!"
                ),
                saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                    "Oh nein! Ich habe meine Zigaretten im Raum vergessen!",
                    "Dass ich das auf meine alten Tage noch erleben muss!"
                )
            ));

            ai.AddGoal(new SchuleVerlassenGoal( // Krankheit Orthomol
                lehrer: Lehrer,
                trigger: new CallbackTrigger(() => Lehrer.Vergiftung is {Vergiftet: true, VergiftungsType: VergiftungsType.Orthamol}),
                eingang: ToiletteLehrerzimmer.position,
                ausgang: ToiletteLehrerzimmer.position,
                saetzeBeimVerlassen: new SaetzeMoeglichkeitenMehrmals(
                    "Aus dem Weg, Ich muss mal ganz dringend auf Toilette",
                    "Mmmhhhhm Ich mag mein Orthomol!",
                    "Jetzt mal ganz ehrlich wir alle müssen mal auf Toilette",
                    "Hand aufs Herz: Ich hab zu viele Fressalien gegessen"
                )
            ));
            
            ai.AddGoal(new VerbrechenErkennenGoal(
                lehrer: Lehrer,
                trigger: new AlwaysTrueTrigger(),
                player: Player,
                reaktion: new SaetzeMoeglichkeitenEinmalig(
                    "Hey Was machst du denn da? Ich bin sauer! Nein ich bin wütend"    
                )
            ));

            // Unterrichten

            var unterrichtAnfang = TimeUtility.MinutesToFloat(-5);
            var unterrichtEnde = TimeUtility.MinutesToFloat(5);

            ai.AddGoal(new UnterrichtenGoal(
                lehrer: Lehrer,
                unterrichtsRaum: UnterrichtsRaum,
                trigger: new ZeitspanneTrigger(
                    TimeManager,
                    new WoechentlicheZeitspannen(
                        new WoechentlicheZeitspannen.Eintrag(
                            new ManuelleWochentagAuswahl(Wochentag.Montag),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 0, AnfangOderEnde.Anfang), unterrichtAnfang),
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 0, AnfangOderEnde.Ende), unterrichtEnde)
                            )
                        )
                    )
                ),
                stundeImStundenplan: new UnterrichtenGoal.StundenData(Wochentag.Montag, 0, "Deutsch"),
                reputationsAenderungBeiFehlzeit: -1f,
                saetzeAufDemWegZumRaum: UnterrichtWeg,
                saetzeBegruessung: UnterrichtBegruessung,
                saetzeWaehrendUnterricht: UnterrichtSaetze
            ));

            ai.AddGoal(new MoveToAndStandAtGoal(
                lehrer: Lehrer,
                trigger: new ZeitspanneTrigger(
                    TimeManager,
                    new WoechentlicheZeitspannen(
                        new WoechentlicheZeitspannen.Eintrag(
                            new EigenschaftWochentagAuswahl(WochentagEigenschaft.Schultag),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, AnfangOderEnde.Anfang), unterrichtAnfang),
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, AnfangOderEnde.Ende), unterrichtEnde)
                            ),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Lernzeit, AnfangOderEnde.Anfang), unterrichtAnfang),
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Lernzeit, AnfangOderEnde.Ende), unterrichtEnde)
                            )
                        )
                    )
                ),
                position: UnterrichtsRaum.GetLehrerStandpunkt(),
                saetzeWeg: UnterrichtWeg,
                saetzeAngekommenEinmalig: UnterrichtBegruessung,
                saetzeAngekommen: UnterrichtSaetze
            ));

            // Pausen

            ai.AddGoal(new VerbrechenMeldenGoal(
                lehrer: Lehrer,
                player: Player,
                trigger: new ZeitspanneTrigger(
                    TimeManager,
                    new WoechentlicheZeitspannen(
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
                    )
                ),
                schulleitungsBuero: Schulleitung.position,
                saetzeWeg: new SaetzeMoeglichkeitenMehrmals(
                    "Jetzt mal ganz ehrlich: So kann das nicht weiter gehen!",
                    "Ich bin sauer! Nein das sind nur Zitronen. Ich bin wütend!"
                )
            ));
            
            ai.AddGoal(new MoveToAndStandAtGoal( // Rauchen
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
                position: Rauchen.position,
                saetzeWeg: RauchenWeg,
                saetzeAngekommen: RauchenAngekommen
            ));

            ai.AddGoal(new VergiftbaresEssenEssenGoal( // Kaffee
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
                vergiftbaresEssen: KaffeemaschineMiniRaum,
                saetzeWeg: KaffeeWeg,
                saetzeAngekommen: KaffeeAngekommen
            ));

            // Vor der Schulzeit

            ai.AddGoal(new VergiftbaresEssenEssenGoal( // Kaffee
                lehrer: Lehrer,
                trigger: new ZeitspanneTrigger(
                    TimeManager,
                    new WoechentlicheZeitspannen(
                        new WoechentlicheZeitspannen.Eintrag(
                            new EigenschaftWochentagAuswahl(WochentagEigenschaft.Schultag),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new SchuleBeginnZeitRelativitaet(), -2f),
                                new WoechentlicheZeitspannen.Zeitpunkt(new SchuleBeginnZeitRelativitaet(), -1.5f)
                            )
                        )
                    )
                ),
                vergiftbaresEssen: KaffeemaschineMiniRaum,
                saetzeWeg: KaffeeWeg,
                saetzeAngekommen: KaffeeAngekommen
            ));

            ai.AddGoal(new MoveToAndStandAtGoal( // Lehrerzimmer Fach
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
                position: LehrerzimmerFach.position,
                saetzeWeg: new SaetzeMoeglichkeitenMehrmals(
                    "Mmmhhm mal gucken ob in meinem Fach was liegt. Da hat bestimmt jemand in der Nacht was reigelegt",
                    "Ich muss nochmal kurz ins Lehrerzimmer"
                ),
                saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                    "Komisch, warum liegt denn gar nichts in meinem Fach. " +
                    "Ich bin doch gestern schon als letzte gegangen und heute erst als erste wieder in die Schule gekommen"
                )
            ));

            ai.AddGoal(new MoveToAndStandAtGoal( // Rauchen
                lehrer: Lehrer,
                trigger: new ZeitspanneTrigger(
                    TimeManager,
                    new WoechentlicheZeitspannen(
                        new WoechentlicheZeitspannen.Eintrag(
                            new EigenschaftWochentagAuswahl(WochentagEigenschaft.Schultag),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new SchuleBeginnZeitRelativitaet(), -1f),
                                new WoechentlicheZeitspannen.Zeitpunkt(new SchuleBeginnZeitRelativitaet(), 0f)
                            )
                        )
                    )
                ),
                position: Rauchen.position,
                saetzeWeg: RauchenWeg,
                saetzeAngekommen: RauchenAngekommen
            ));
        }

        protected override void RegisterFragen(InteraktionsMenuFragenManager fragen)
        {
            fragen.AddFrage(new InteraktionsMenuFrageAnnehmenAblehnen(
                lehrer: Lehrer,
                player: Player,
                shouldShowPredicate: InteraktionsMenuFrageUtil.ShowNearby(),
                interaktionsMenuName: "Schwarzen Pfeffer schenken",
                annahmeWahrscheinlichkeit: 0.5f,
                reputationsAenderungBeiAnnahme: 0.3f,
                annahmeAntworten: new SaetzeMoeglichkeitenEinmalig(
                    "Du weißt, wie der Hase läuft! Ich hab im Internetz gelesen, dass das gegen Athrose hilft"    
                ),
                reputationsAenderungBeiAblehnen: -0.1f,
                ablehnenAntworten: new SaetzeMoeglichkeitenEinmalig(
                    "Was will Ich denn mit schwarzem Pfeffer"    
                ),
                kosten: 20
            ));
            
            fragen.AddFrage(new InteraktionsMenuFrageEinfach(
                lehrer: Lehrer,
                player: Player,
                shouldShowPredicate: InteraktionsMenuFrageUtil.ShowNearby(),
                interaktionsMenuName: "Nach Klassenfahrt fragen",
                reputationsAenderung: -1f,
                antworten: new SaetzeMoeglichkeitenEinmalig(
                    "Verantwortung ist nicht eröterbar".ZufaelligGrossKlein(),
                    "Es ist mir egal, ob ihr euch mit Corona ansteckt. Ich will nur nicht die Verantwortung dafür tragen".ZufaelligGrossKlein()
                )
            ));
            
            fragen.AddFrage(new InteraktionsMenuFrageAnnehmenAblehnen(
                lehrer: Lehrer,
                player: Player,
                shouldShowPredicate: InteraktionsMenuFrageUtil.ShowWhileKannSchwaenzen(),
                interaktionsMenuName: "Fragen, auf Toilette zu gehen",
                annahmeWahrscheinlichkeit: 0.8f,
                annahmeAntworten: new SaetzeMoeglichkeitenEinmalig(
                    "Ja du kannst gehen. Aber beeile dich!",
                    "Dieses mal darfst du gehen, aber Summa Sumarum warst du dieses Jahr schon 3 mal auf Toilette. Das muss sich ändern!"
                ),
                onAnnehmen: InteraktionsMenuFrageUtil.AusUnterrichtFreistellen(),
                reputationsAenderungBeiAblehnen: -0.1f,
                ablehnenAntworten: new SaetzeMoeglichkeitenEinmalig(
                    "Jetzt mal ganz ehrlich: Du schaffst es auch noch bis zur Pause"
                )
            ));
        }
    }
}
