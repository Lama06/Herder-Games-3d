using System;
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
    public class KammerathBrain : BrainBase
    {
        [SerializeField] private Player.Player Player;
        [SerializeField] private Klassenraum UnterrichtsRaum;
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private InternetManager Internet;
        [SerializeField] private AlarmManager AlarmManager;
        
        private readonly SaetzeMoeglichkeitenMehrmals Unterricht = new(
            "Eins Plus Kein Fehler!",
            "Sechs!",
            "Das ist ja zum Mäuse melken",
            "Ich hab vergessen diese geklaufte Folie auf Deutsch zu übersetz....Äähm.....Ich meine ich hab sie extra für euch auf Englisch übersetzt",
            "Ich...Äähm...Jemand hat gerade geraucht. Eine Nichtraucherin richt das sofort. Vor dem Unterricht Pfefferminz nehmen",
            "Ihr wollt wissen was Ssshreeadhs in Java sind? Einfach das Buch schenken lassen!",
            "Schade das die MLPD bei der letzten Wahl nicht gewonnen hat! Das iset ja zum Mäuse melken",
            "Wenn ihr fragen habt, fragt mich einfach. Vielleicht werde ich die Fragen beantworten, vielleicht werde ich aber auch sagen: Das musst du selber wissen",
            "Die DDR ist zusammengefallen? Das ist ja zum Mäuse melken!",
            "Hast du in deinem Leben schon mal Google benutzt?! Sechs!",
            "Hey, Was machst du da auf Google?! Sechs!",
            "Ach, Wie ich doch Windows XP vermisse!",
            "Ich empfehle nicht Informatik in der Oberstufe zu wählen! (weil ich es selber nicht kann)",
            "Ach sieh mal an, der Beamer spiegelt sich in der Plexiglas scheibe",
            "Wir haben eine superhohe Inzidenz in Köln und du trägst im Untericht keine Maske? (sagte sie, ohne eine Maske zu tragen)"
        );

        private readonly SaetzeMoeglichkeitenMehrmals UnterrichtKeinInternet = new(
            "Ich hab kein Internet! Das ist ja zum Mäuse melken",
            "Wie soll ich denn jetzt Unterricht machen, wo ich gar nicht aus dem Internet mein Unterrichtsmaterial klauen kann",
            "Meine komische Spionagesoftware funktioniert nicht! Wie soll ich denn so unterrichten? Ich kann da ja gar nicht mehr die Schüler demütigen!",
            "Wenn das Internet nicht bald wieder funktioniert geh ich nach Hause!"
        );

        private readonly SaetzeMoeglichkeitenMehrmals UnterrichtKeinInternetDannach = new(
            "Ihr könnt jetzt nach Hause gehen, das ist ja nicht auszuhalten! Aber seid vorsichtig!"
        );

        protected override void RegisterGoals(AIController ai)
        {
            ai.AddGoal(new MoveToAndStandAtGoal(
                lehrer: Lehrer,
                trigger: new CallbackTrigger(() => AlarmManager.IsAlarm()),
                position: UnterrichtsRaum.GetLehrerStandpunkt(),
                saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                    "Ach das ist doch bestimmt wiedermal ein Fehlalarm! Das ist ja zum Mäuse melken!",
                    "Diese leichte Beschallung ist ja in Ordnung, aber dieser Alarm geht ja mit der Zeit auch auf die Nerven"
                )
            ));
            
            ai.AddGoal(new MoveToAndStandAtGoal(
                lehrer: Lehrer,
                trigger: new CallbackTrigger(() => Lehrer.Vergiftung is {Syntome: true, VergiftungsType: VergiftungsType.Normal}),
                position: UnterrichtsRaum.GetLehrerStandpunkt(),
                saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                    "Mir geht es gerade gar nicht gut! Das ist zum Mäuse melken!",
                    "Kannst du mir mal bitte kurz den Mülleimer reichen?"
                )
            ));

            ai.AddGoal(new MoveToAndStandAtGoal(
                lehrer: Lehrer,
                trigger: new CallbackTrigger(() => Lehrer.Vergiftung is {Syntome: true, VergiftungsType: VergiftungsType.Orthamol}),
                position: UnterrichtsRaum.GetLehrerStandpunkt(),
                saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                    "Ich muss schon wieder auf Klo. Das ist ja zum Mäuse melken",
                    "Ich muss mal ganz dringend auf Toilette! Ach egal, hier in dem Raum stinkt es ja sowieso schon",
                    "Kannst du mir mal bitte kurz den Mülleimer reichen?"
                )
            ));

            // Unterricht Kein Internet

            ai.AddGoal(new UnterrichtenGoal(
                lehrer: Lehrer,
                unterrichtsRaum: UnterrichtsRaum,
                trigger: new CallbackTrigger(() =>
                {
                    var zeit = new WoechentlicheZeitspannen(
                        new WoechentlicheZeitspannen.Eintrag(
                            new ManuelleWochentagAuswahl(Wochentag.Montag),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 1, AnfangOderEnde.Anfang), 0f),
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 1, AnfangOderEnde.Anfang), TimeUtility.MinutesToFloat(25f))
                            )
                        )
                    );

                    return zeit.IsInside(TimeManager) && !Internet.IsInternetVerfuegbar();
                }),
                stundeImStundenplan: new UnterrichtenGoal.StundenData(Wochentag.Montag, 1, "Informatik"),
                reputationsAenderungBeiFehlzeit: -0.1f,
                saetzeWaehrendUnterricht: UnterrichtKeinInternet
            ));
            
            ai.AddGoal(new MoveToAndStandAtGoal(
                lehrer: Lehrer,
                position: UnterrichtsRaum.GetLehrerStandpunkt(),
                trigger: new CallbackTrigger(() =>
                {
                    var zeit = new WoechentlicheZeitspannen(
                        new WoechentlicheZeitspannen.Eintrag(
                            new ManuelleWochentagAuswahl(Wochentag.Montag),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 1, AnfangOderEnde.Anfang), TimeUtility.MinutesToFloat(25f)),
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 1, AnfangOderEnde.Ende), 0f)
                            )
                        )
                    );

                    return zeit.IsInside(TimeManager) && !Internet.IsInternetVerfuegbar();
                }),
                saetzeAngekommen: UnterrichtKeinInternetDannach
            ));

            ai.AddGoal(new MoveToAndStandAtGoal(
                lehrer: Lehrer,
                position: UnterrichtsRaum.GetLehrerStandpunkt(),
                trigger: new CallbackTrigger(() =>
                {
                    var zeit = new WoechentlicheZeitspannen(
                        new WoechentlicheZeitspannen.Eintrag(
                            new ManuelleWochentagAuswahl(Wochentag.Montag),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, AnfangOderEnde.Anfang), 0f),
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, AnfangOderEnde.Anfang), TimeUtility.MinutesToFloat(25f))
                            )
                        )
                    );

                    return zeit.IsInside(TimeManager) && !Internet.IsInternetVerfuegbar();
                }),
                saetzeAngekommen: UnterrichtKeinInternet
            ));
            
            ai.AddGoal(new MoveToAndStandAtGoal(
                lehrer: Lehrer,
                position: UnterrichtsRaum.GetLehrerStandpunkt(),
                trigger: new CallbackTrigger(() =>
                {
                    var zeit = new WoechentlicheZeitspannen(
                        new WoechentlicheZeitspannen.Eintrag(
                            new ManuelleWochentagAuswahl(Wochentag.Montag),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, AnfangOderEnde.Anfang), TimeUtility.MinutesToFloat(25f)),
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, AnfangOderEnde.Ende), 0f)
                            )
                        )
                    );

                    return zeit.IsInside(TimeManager) && !Internet.IsInternetVerfuegbar();
                }),
                saetzeAngekommen: UnterrichtKeinInternetDannach
            ));
            
            // Unterricht Normal

            ai.AddGoal(new UnterrichtenGoal(
                lehrer: Lehrer,
                unterrichtsRaum: UnterrichtsRaum,
                trigger: new ZeitspanneTrigger(
                    TimeManager,
                    new WoechentlicheZeitspannen(
                        new WoechentlicheZeitspannen.Eintrag(
                            new ManuelleWochentagAuswahl(Wochentag.Montag),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 1, AnfangOderEnde.Anfang), 0f),
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 1, AnfangOderEnde.Ende), 0f)
                            )
                        )
                    )
                ),
                stundeImStundenplan: new UnterrichtenGoal.StundenData(Wochentag.Montag, 1, "Informatik"),
                reputationsAenderungBeiFehlzeit: -0.3f,
                saetzeWaehrendUnterricht: Unterricht
            ));

            ai.AddGoal(new MoveToAndStandAtGoal(
                lehrer: Lehrer,
                trigger: new ZeitspanneTrigger(
                    TimeManager,
                    new WoechentlicheZeitspannen(
                        new WoechentlicheZeitspannen.Eintrag(
                            new EigenschaftWochentagAuswahl(WochentagEigenschaft.Schultag),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, AnfangOderEnde.Anfang), 0f),
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, AnfangOderEnde.Ende), 0f)
                            )
                        )
                    )
                ),
                position: UnterrichtsRaum.GetLehrerStandpunkt(),
                saetzeAngekommen: Unterricht
            ));
            
            ai.AddGoal(new MoveToAndStandAtGoal(
                lehrer: Lehrer,
                trigger: new AlwaysTrueTrigger(),
                position: UnterrichtsRaum.GetLehrerStandpunkt()
            ));
        }

        protected override void RegisterFragen(InteraktionsMenuFragenManager fragen)
        {
            fragen.AddFrage(new InteraktionsMenuFrageEinfach(
                lehrer: Lehrer,
                player: Player,
                shouldShowPredicate: InteraktionsMenuFrageUtil.ShowNearby(),
                interaktionsMenuName: "Auf Fehler im Unterricht ansprechen",
                reputationsAenderung: -0.3f,
                antworten: new SaetzeMoeglichkeitenEinmalig(
                    "Da hast du wohl was falsch abgeschrieben! Sechs!",
                    "Da hast du wohl was falsch verstanden! Sechs!",
                    "Stell keine dummen Fragen! Sechs!"
                )
            ));

            fragen.AddFrage(new InteraktionsMenuFrageAnnehmenAblehnen(
                lehrer: Lehrer,
                player: Player,
                shouldShowPredicate: InteraktionsMenuFrageUtil.ShowNearby(),
                interaktionsMenuName: "Auf Korrekturfehler in Klassenarbeit hinweisen",
                annahmeWahrscheinlichkeit: 0.3f,
                annahmeAntworten: new SaetzeMoeglichkeitenEinmalig(
                    "Gut, dass du es sagst. Da muss mir wohl ein Feher bei der Korrektur unterlaufen sein"
                ),
                reputationsAenderungBeiAblehnen: -0.2f,
                ablehnenAntworten: new SaetzeMoeglichkeitenEinmalig(
                    "Das würde zwar genau richtig funktieren, aber es steht nicht 1 zu 1 so in meiner Musterlösung! Sechs!",
                    "Sechs!",
                    "Deswegen empfehle ich auch nicht Informatik in der EF zu wählen"
                )
            ));

            fragen.AddFrage(new InteraktionsMenuFrageEinfach(
                lehrer: Lehrer,
                player: Player,
                shouldShowPredicate: InteraktionsMenuFrageUtil.ShowNearby(),
                interaktionsMenuName: "Frage zum Informatikunterricht stellen",
                antworten: new SaetzeMoeglichkeitenEinmalig(
                    "Die Frage beantworte ich nicht! Sechs!",
                    "Da hast du wohl nicht richtig im Unterricht aufgepasst, noch? Sechs!",
                    "Das musst du selber wissen! Sechs!",
                    "Woher soll ich das denn wissen? Du meinst doch nicht ernsthaft, dass ich Informatik kann? Sechs!"
                ),
                reputationsAenderung: -0.2f
            ));

            fragen.AddFrage(new InteraktionsMenuFrageEinfach(
                lehrer: Lehrer,
                player: Player,
                interaktionsMenuName: "Sagen, dass die Computer nicht funktionieren",
                shouldShowPredicate: InteraktionsMenuFrageUtil.ShowWhileKannSchwaenzen(),
                antworten: new SaetzeMoeglichkeitenEinmalig(
                    "Das war ja mal wieder klar! An dieser Schule funktioniert ja auch wirklich nichts! Du kannst gehen. Aber sei vorsichtig!"
                ),
                clickCallback: InteraktionsMenuFrageUtil.AusUnterrichtFreistellen()
            ));
            
            fragen.AddFrage(new InteraktionsMenuFrageAnnehmenAblehnen(
                lehrer: Lehrer,
                player: Player,
                shouldShowPredicate: InteraktionsMenuFrageUtil.ShowNearby(),
                interaktionsMenuName: "Mit Orthomol vergifteten Kuchen anbieten",
                kosten: 20,
                annahmeWahrscheinlichkeit: 0.8f,
                reputationsAenderungBeiAnnahme: 0.5f,
                annahmeAntworten: new SaetzeMoeglichkeitenEinmalig(
                    "Danke das kann ich gut vertragen. Ich bin ja auch so dünn und habe erst 42 Stücke Kuchen gegessen.",
                    "Mmmh, Knusprig!"
                ),
                onAnnehmen: (_, _) =>
                {
                    Lehrer.Vergiftung.Vergiften(VergiftungsType.Orthamol);
                },
                reputationsAenderungBeiAblehnen: -0.1f,
                ablehnenAntworten: new SaetzeMoeglichkeitenEinmalig(
                    "Ich hab jezt keinen Hunger auf Kuchen! Sechs!"
                )
            ));
        }
    }
}
