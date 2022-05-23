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
        [SerializeField] private Transform SchulPc;
        [SerializeField] private Transform LehrerStuhl;
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private InternetManager Internet;
        [SerializeField] private AlarmManager AlarmManager;

        protected override void RegisterGoals(AIController ai)
        {
            #region Sätze

            var unterricht = new SaetzeMoeglichkeitenMehrmals(
                "Eins Plus, Kein Fehler!",
                "Sechs!",
                "Das ist ja zum Mäuse melken",
                "Ich hab vergessen diese geklaute Folie auf Deutsch zu übersetz....Äähm.....Ich meine ich hab sie extra für euch auf Englisch übersetzt",
                "Ich...Äähm...Jemand hat gerade geraucht. Eine Nichtraucherin richt das sofort. Vor dem Unterricht Pfefferminz nehmen.",
                "Ihr wollt wissen was Ssshreeadhs in Java sind? Einfach das Buch schenken lassen!",
                "Schade das die MLPD bei der letzten Wahl nicht gewonnen hat! Das ist ja zum Mäuse melken",
                "Wenn ihr fragen habt, fragt mich einfach. Vielleicht werde ich die Fragen beantworten, vielleicht werde ich aber auch sagen: Das musst du selber wissen",
                "Die DDR ist zusammengefallen? Das ist ja zum Mäuse melken!",
                "Hast du in deinem Leben schon mal Google benutzt?! Sechs!",
                "Hey, Was machst du da auf Google?! Sechs!",
                "Ach, wie ich doch Windows XP vermisse!",
                "Ich empfehle nicht Informatik in der Oberstufe zu wählen! (weil ich es selber nicht kann)",
                "Ach sieh mal an, der Beamer spiegelt sich in der Plexiglas scheibe",
                "Wir haben eine superhohe Inzidenz in Köln und du trägst im Untericht keine Maske? (sagte sie, ohne eine Maske zu tragen)"
            );

            var unterrichtKeinInternet = new SaetzeMoeglichkeitenMehrmals(
                "Ich hab kein Internet! Das ist ja zum Mäuse melken",
                "Wie soll ich denn jetzt Unterricht machen, wo ich gar nicht aus dem Internet mein Unterrichtsmaterial klauen kann",
                "Meine komische Spionagesoftware funktioniert nicht! Wie soll ich denn so unterrichten? Ich kann da ja gar nicht mehr die Schüler demütigen!",
                "Wenn das Internet nicht bald wieder funktioniert geh ich nach Hause!"
            );

            var unterrichtKeinInternetDannach = new SaetzeMoeglichkeitenMehrmals(
                "Ihr könnt jetzt nach Hause gehen, das ist ja nicht auszuhalten! Aber seid vorsichtig!"
            );

            #endregion

            const float unterrichtLaengeWennKeinInternet = 25f;

            void EchtUnterrichten(Wochentag wochentag, int index)
            {
                ai.AddGoal(new UnterrichtenGoal(
                    lehrer: Lehrer,
                    unterrichtsRaum: UnterrichtsRaum,
                    standpunkt: LehrerStuhl.position,
                    trigger: new CallbackTrigger(() =>
                    {
                        var zeit = new WoechentlicheZeitspannen(
                            new WoechentlicheZeitspannen.Eintrag(
                                new ManuelleWochentagAuswahl(wochentag),
                                new WoechentlicheZeitspannen.Zeitspanne(
                                    new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, index, AnfangOderEnde.Anfang), 0f),
                                    new WoechentlicheZeitspannen.Zeitpunkt(
                                        new StundeZeitRelativitaet(StundenType.Fach, index, AnfangOderEnde.Anfang), TimeUtility.MinutesToFloat(unterrichtLaengeWennKeinInternet))
                                )
                            )
                        );

                        return zeit.IsInside(TimeManager) && !Internet.IsInternetVerfuegbar();
                    }),
                    stundeImStundenplan: new UnterrichtenGoal.StundenData(wochentag, index, "Informatik"),
                    reputationsAenderungBeiFehlzeit: -0.1f,
                    saetzeWaehrendUnterricht: unterrichtKeinInternet
                ));

                ai.AddGoal(new MoveToAndStandAtGoal(
                    lehrer: Lehrer,
                    position: LehrerStuhl.position,
                    trigger: new CallbackTrigger(() =>
                    {
                        var zeit = new WoechentlicheZeitspannen(
                            new WoechentlicheZeitspannen.Eintrag(
                                new ManuelleWochentagAuswahl(wochentag),
                                new WoechentlicheZeitspannen.Zeitspanne(
                                    new WoechentlicheZeitspannen.Zeitpunkt(
                                        new StundeZeitRelativitaet(StundenType.Fach, index, AnfangOderEnde.Anfang), TimeUtility.MinutesToFloat(unterrichtLaengeWennKeinInternet)),
                                    new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, index, AnfangOderEnde.Ende), 0f)
                                )
                            )
                        );

                        return zeit.IsInside(TimeManager) && !Internet.IsInternetVerfuegbar();
                    }),
                    saetzeAngekommen: unterrichtKeinInternetDannach
                ));

                ai.AddGoal(new UnterrichtenGoal(
                    lehrer: Lehrer,
                    unterrichtsRaum: UnterrichtsRaum,
                    standpunkt: LehrerStuhl.position,
                    trigger: new ZeitspanneTrigger(
                        TimeManager,
                        new  WoechentlicheZeitspannen(wochentag, StundenType.Fach, index)
                    ),
                    stundeImStundenplan: new UnterrichtenGoal.StundenData(wochentag, index, "Informatik"),
                    reputationsAenderungBeiFehlzeit: -0.3f,
                    saetzeWaehrendUnterricht: unterricht
                ));
            }

            void FakeUnterrichten(Wochentag wochentag, int index)
            {
                ai.AddGoal(new MoveToAndStandAtGoal(
                    lehrer: Lehrer,
                    position: LehrerStuhl.position,
                    trigger: new CallbackTrigger(() =>
                    {
                        var zeit = new WoechentlicheZeitspannen(
                            new WoechentlicheZeitspannen.Eintrag(
                                new ManuelleWochentagAuswahl(wochentag),
                                new WoechentlicheZeitspannen.Zeitspanne(
                                    new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, index, AnfangOderEnde.Anfang), 0f),
                                    new WoechentlicheZeitspannen.Zeitpunkt(
                                        new StundeZeitRelativitaet(StundenType.Fach, index, AnfangOderEnde.Anfang), TimeUtility.MinutesToFloat(unterrichtLaengeWennKeinInternet))
                                )
                            )
                        );

                        return zeit.IsInside(TimeManager) && !Internet.IsInternetVerfuegbar();
                    }),
                    saetzeAngekommen: unterrichtKeinInternet
                ));

                ai.AddGoal(new MoveToAndStandAtGoal(
                    lehrer: Lehrer,
                    position: LehrerStuhl.position,
                    trigger: new CallbackTrigger(() =>
                    {
                        var zeit = new WoechentlicheZeitspannen(
                            new WoechentlicheZeitspannen.Eintrag(
                                new ManuelleWochentagAuswahl(wochentag),
                                new WoechentlicheZeitspannen.Zeitspanne(
                                    new WoechentlicheZeitspannen.Zeitpunkt(
                                        new StundeZeitRelativitaet(StundenType.Fach, index, AnfangOderEnde.Anfang), TimeUtility.MinutesToFloat(unterrichtLaengeWennKeinInternet)),
                                    new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, index, AnfangOderEnde.Ende), 0f)
                                )
                            )
                        );

                        return zeit.IsInside(TimeManager) && !Internet.IsInternetVerfuegbar();
                    }),
                    saetzeAngekommen: unterrichtKeinInternetDannach
                ));

                ai.AddGoal(new MoveToAndStandAtGoal(
                    lehrer: Lehrer,
                    trigger: new ZeitspanneTrigger(
                        TimeManager,
                        new  WoechentlicheZeitspannen(wochentag, StundenType.Fach, index)
                    ),
                    position: LehrerStuhl.position,
                    saetzeAngekommen: unterricht
                ));
            }

            void SchulPcReparieren(WoechentlicheZeitspannen wann)
            {
                ai.AddGoal(new MoveToAndStandAtGoal(
                    lehrer: Lehrer,
                    trigger: new ZeitspanneTrigger(TimeManager, wann),
                    position: SchulPc.position,
                    saetzeWeg: new SaetzeMoeglichkeitenMehrmals(
                        "Der PC ist schon wieder kaputt. Das ist ja zum Mäuse melken! Mal gucken was ich da machen kann"
                    ),
                    saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                        "Das ist ja zum Mäuse melken! Wie soll ich denn diesen PC reparieren? Ich bin einfach zu dumm!",
                        "Das ist ja zum Mäuse melken! Wer hat diesen PC denn schon wieder kaputt gemacht!",
                        "Ich glaube ich schreibe da einfach defekt dran. Dann hat sich die Sache gegessen"
                    )
                ));
            }

            void SpielSpielen(WoechentlicheZeitspannen wann)
            {
                ai.AddGoal(new MoveToAndStandAtGoal(
                    lehrer: Lehrer,
                    trigger: new CallbackTrigger(() => !Internet.IsInternetVerfuegbar() && wann.IsInside(TimeManager)),
                    position: LehrerStuhl.position,
                    saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                        "Das ist ja zum Mäuse melken! Das Internet funktioniert schon wieder nicht! Wie soll ich denn so meine Spiele spielen!"
                    )
                ));

                ai.AddGoal(new MoveToAndStandAtGoal(
                    lehrer: Lehrer,
                    trigger: new ZeitspanneTrigger(TimeManager, wann),
                    position: LehrerStuhl.position,
                    saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                        "Mir gehen langsam die Spiele aus, die Ich spielen kann!" +
                        "Ich sag meinen Schülern einfach, dass sie mir mal neue Spiele mit Mario programmieren sollten!",
                        "Gut, dass ich mir einen Gaming Stuhl und einen Curved Screen gekauft habe!" +
                        "Die sind super zum spielen...Äähm ich meine die sind super zum arbeiten",
                        "Das ist ein gutes Spiel! Aber da fehlen noch ein paar Animationen!"
                    )
                ));
            }

            void ImInternetSurfen(WoechentlicheZeitspannen wann)
            {
                ai.AddGoal(new MoveToAndStandAtGoal(
                    lehrer: Lehrer,
                    trigger: new CallbackTrigger(() => !Internet.IsInternetVerfuegbar() && wann.IsInside(TimeManager)),
                    position: LehrerStuhl.position,
                    saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                        "Das ist ja zum Mäuse melken! Das Internet funktioniert schon wieder nicht!"
                    )
                ));

                ai.AddGoal(new MoveToAndStandAtGoal(
                    lehrer: Lehrer,
                    trigger: new ZeitspanneTrigger(TimeManager, wann),
                    position: LehrerStuhl.position,
                    saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                        "Mmmhn Wie kann man denn Aufkleber selber basteln?",
                        "Was? ulrike.strupat@hotmail.com fordert eine Belehrung darüber an, wie man seine 'Anmeldeschlüssel' zurücksetzt???"
                    )
                ));
            }

            #region Hohe Priorität

            ai.AddGoal(new MoveToAndStandAtGoal(
                lehrer: Lehrer,
                trigger: new CallbackTrigger(() => AlarmManager.IsAlarm()),
                position: LehrerStuhl.position,
                saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                    "Ach das ist doch bestimmt wiedermal ein Fehlalarm! Das ist ja zum Mäuse melken!",
                    "Diese leichte Beschallung ist ja in Ordnung, aber dieser Alarm geht ja mit der Zeit auch auf die Nerven"
                )
            ));

            ai.AddGoal(new MoveToAndStandAtGoal(
                lehrer: Lehrer,
                trigger: new CallbackTrigger(() => Lehrer.Vergiftung is {Syntome: true, VergiftungsType: VergiftungsType.Normal}),
                position: LehrerStuhl.position,
                saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                    "Mir geht es gerade gar nicht gut! Das ist zum Mäuse melken!",
                    "Kannst du mir mal bitte kurz den Mülleimer reichen?"
                )
            ));

            ai.AddGoal(new MoveToAndStandAtGoal(
                lehrer: Lehrer,
                trigger: new CallbackTrigger(() => Lehrer.Vergiftung is {Syntome: true, VergiftungsType: VergiftungsType.Orthamol}),
                position: LehrerStuhl.position,
                saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                    "Ich muss schon wieder auf Klo. Das ist ja zum Mäuse melken",
                    "Ich muss mal ganz dringend auf Toilette! Ach egal, hier in dem Raum stinkt es ja sowieso schon",
                    "Kannst du mir mal bitte kurz den Mülleimer reichen?"
                )
            ));

            #endregion

            #region Montag

            FakeUnterrichten(Wochentag.Montag, 0);
            SchulPcReparieren(new WoechentlicheZeitspannen(Wochentag.Montag, StundenType.Kurzpause, 0));
            EchtUnterrichten(Wochentag.Montag, 1);
            SpielSpielen(new WoechentlicheZeitspannen(Wochentag.Montag, StundenType.Kurzpause, 1));
            FakeUnterrichten(Wochentag.Montag, 2);
            ImInternetSurfen(new WoechentlicheZeitspannen(Wochentag.Montag, StundenType.Mittagspause, 0));
            FakeUnterrichten(Wochentag.Montag, 3);

            #endregion

            #region Dienstag

            FakeUnterrichten(Wochentag.Dienstag, 0);
            SpielSpielen(new WoechentlicheZeitspannen(Wochentag.Dienstag, StundenType.Kurzpause, 0));
            FakeUnterrichten(Wochentag.Dienstag, 1);
            ImInternetSurfen(new WoechentlicheZeitspannen(Wochentag.Dienstag, StundenType.Kurzpause, 1));
            FakeUnterrichten(Wochentag.Dienstag, 2);

            #endregion

            #region Mittwoch

            FakeUnterrichten(Wochentag.Mittwoch, 0);
            SpielSpielen(new WoechentlicheZeitspannen(Wochentag.Mittwoch, StundenType.Kurzpause, 0));
            FakeUnterrichten(Wochentag.Mittwoch, 1);
            SchulPcReparieren(new WoechentlicheZeitspannen(Wochentag.Mittwoch, StundenType.Kurzpause, 1));
            SpielSpielen(new WoechentlicheZeitspannen(Wochentag.Mittwoch, StundenType.Lernzeit, 0));
            SpielSpielen(new WoechentlicheZeitspannen(Wochentag.Mittwoch, StundenType.Mittagspause, 0));
            FakeUnterrichten(Wochentag.Mittwoch, 2);

            #endregion

            #region Donnerstag

            FakeUnterrichten(Wochentag.Donnernstag, 0);
            SchulPcReparieren(new WoechentlicheZeitspannen(Wochentag.Donnernstag, StundenType.Kurzpause, 0));
            FakeUnterrichten(Wochentag.Donnernstag, 1);
            ImInternetSurfen(new WoechentlicheZeitspannen(Wochentag.Donnernstag, StundenType.Kurzpause, 1));
            SpielSpielen(new WoechentlicheZeitspannen(Wochentag.Donnernstag, StundenType.Lernzeit, 0));
            SpielSpielen(new WoechentlicheZeitspannen(Wochentag.Donnernstag, StundenType.Mittagspause, 0));
            FakeUnterrichten(Wochentag.Donnernstag, 2);

            #endregion

            #region Freitag

            FakeUnterrichten(Wochentag.Freitag, 0);
            SpielSpielen(new WoechentlicheZeitspannen(Wochentag.Freitag, StundenType.Kurzpause, 0));
            FakeUnterrichten(Wochentag.Freitag, 1);
            SpielSpielen(new WoechentlicheZeitspannen(Wochentag.Freitag, StundenType.Kurzpause, 1));
            FakeUnterrichten(Wochentag.Freitag, 2);

            #endregion
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
                onAnnehmen: (_, _) => Lehrer.Vergiftung.Vergiften(VergiftungsType.Orthamol),
                reputationsAenderungBeiAblehnen: -0.1f,
                ablehnenAntworten: new SaetzeMoeglichkeitenEinmalig(
                    "Ich hab jezt keinen Hunger auf Kuchen! Sechs!"
                )
            ));
        }
    }
}
