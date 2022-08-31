using HerderGames.Lehrer.AI;
using HerderGames.Lehrer.AI.Goals;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Animation;
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
        [SerializeField] private InternetManager Internet;
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private Transform RauchenVorne;
        [SerializeField] private Transform RauchenHinten;
        [SerializeField] private Klassenraum UnterrichtsRaum;
        [SerializeField] private Transform UnterrichtsStandpunkt;
        [SerializeField] private Transform SchuleEingang;
        [SerializeField] private VergiftbaresEssen KaffeemaschineRaucherLehrerzimmer;
        [SerializeField] private Transform Sammelpunkt;
        [SerializeField] private AlarmManager AlarmManager;
        [SerializeField] private Transform Toilette;
        [SerializeField] private Transform EingangFahhradKontrolle;
        [SerializeField] private Transform Schulleitung;

        protected override void RegisterGoals(AIController ai)
        {
            var unterrichtVerspaetung = (-4f).MinutesToHours();
            var unterrichtUeberziehung = 7f.MinutesToHours();

            #region Sätze

            var unterrichtWeg = new SaetzeMoeglichkeitenMehrmals(
                "Ich muss mich beilen. Der Unterricht beginnt ja gleich und Ich muss heute noch bis Lektion 1000 kommen"
            );

            var unterrichtBegruessung = new SaetzeMoeglichkeitenEinmalig("Moin Moin");

            var unterricht = new SaetzeMoeglichkeitenMehrmals(
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
                "Ich könnte mich zwar mit den Römern unterhalen aber in die Antike zurück will ich trotzdem nicht: Da gab es keine Zahärtzte",
                "Was heißt 'eilen' auf Latein?"
            );

            var unterrichtWegKrank = new SaetzeMoeglichkeitenMehrmals(
                "Ich glaube es geht mir so schlecht weil ich heute so wenig geraucht habe"
            );

            var unterrichtKrank = new SaetzeMoeglichkeitenMehrmals(
                "Mir geht es heute gar nicht gut, macht bitte im Lernplan weiter",
                "Ich muss mir irgendwie den Magen verstimmt haben",
                "Ich bräuchte mal eine Feder"
            );

            var unterrichtKeinInternet = new SaetzeMoeglichkeitenMehrmals(
                "Ach das Internet geht schon wieder nicht! Das trage ich direkt in meine Tabelle ein!",
                "Das Internet ist wieder kaputt! Ich sage doch, wir müssen die Digitalisierung rückgängig machen dann kriegen wir mal wieder richtig Stoff durch!"
            );

            var rauchenWeg = new SaetzeMoeglichkeitenMehrmals(
                "Ich hab ja noch 5 Minuten, da kann ich ja noch eine Rauchen gehen"
            );

            var rauchen = new SaetzeMoeglichkeitenMehrmals(
                "Macht euch keine Sorgen: Ich rauche ja nur gelegentlich",
                "Nehmt mich bitte nicht als Vorbild, was das Rauchen angeht",
                "Hast du mal nen Fünfziger, Ich hab mein Feuerzeug gerade nicht dabei",
                "Ich rauche ja nicht auf Lunge",
                "Es ist so vieles ungesund ... und ich rauche ja auch schon seit Cäsars tot und lebe immer noch"
            );

            #endregion

            #region Animationen

            var gehenAnimation = new SimpleAnimation(AnimationType.Gehen);

            var unterrichtenAnimationNormal = new RepeatAnimation(new ShuffleAnimation(
                new ShuffleAnimation.Choice(10, new SimpleAnimation(AnimationType.Reden, 7f)),
                new ShuffleAnimation.Choice(2, new SimpleAnimation(AnimationType.RedenAggressiv, 7f)),
                new ShuffleAnimation.Choice(1, new SimpleAnimation(AnimationType.Stehen, 7f))
            ));

            var unterrichtenAnimationKeinInternet = new RepeatAnimation(new ShuffleAnimation(
                new ShuffleAnimation.Choice(10, new SimpleAnimation(AnimationType.RedenAggressiv, 7f)),
                new ShuffleAnimation.Choice(2, new SimpleAnimation(AnimationType.Reden, 7f)),
                new ShuffleAnimation.Choice(1, new SimpleAnimation(AnimationType.Stehen, 7f))
            ));

            var unterrichtenAnimationKrank = new RepeatAnimation(new ShuffleAnimation(
                new ShuffleAnimation.Choice(10, new SimpleAnimation(AnimationType.Stehen, 7f)),
                new ShuffleAnimation.Choice(2, new SimpleAnimation(AnimationType.RedenAggressiv, 7f)),
                new ShuffleAnimation.Choice(1, new SimpleAnimation(AnimationType.Stehen, 7f))
            ));

            #endregion

            void Unterrichten(WoechentlicheZeitspannen wann, UnterrichtenGoal.StundenData stunde)
            {
                ai.AddGoal(new UnterrichtenGoal(
                    lehrer: Lehrer,
                    unterrichtsRaum: UnterrichtsRaum,
                    standpunkt: UnterrichtsStandpunkt.position,
                    trigger: new CallbackTrigger(() => wann.IsInside(TimeManager) && Lehrer.Vergiftung is {Syntome: true, VergiftungsType: VergiftungsType.Normal}),
                    stundeImStundenplan: stunde,
                    reputationsAenderungBeiFehlzeit: -0.2f,
                    saetzeAufDemWegZumRaum: unterrichtWegKrank,
                    saetzeBegruessung: unterrichtBegruessung,
                    saetzeWaehrendUnterricht: unterrichtKrank,
                    animationWeg: gehenAnimation,
                    animationAngekommen: unterrichtenAnimationKrank
                ));

                ai.AddGoal(new UnterrichtenGoal(
                    lehrer: Lehrer,
                    unterrichtsRaum: UnterrichtsRaum,
                    standpunkt: UnterrichtsStandpunkt.position,
                    trigger: new CallbackTrigger(() => wann.IsInside(TimeManager) && !Internet.IsInternetVerfuegbar),
                    stundeImStundenplan: stunde,
                    reputationsAenderungBeiFehlzeit: -0.4f,
                    saetzeAufDemWegZumRaum: unterrichtWeg,
                    saetzeBegruessung: unterrichtBegruessung,
                    saetzeWaehrendUnterricht: unterrichtKeinInternet,
                    animationWeg: gehenAnimation,
                    animationAngekommen: unterrichtenAnimationKeinInternet
                ));

                ai.AddGoal(new UnterrichtenGoal(
                    lehrer: Lehrer,
                    unterrichtsRaum: UnterrichtsRaum,
                    standpunkt: UnterrichtsStandpunkt.position,
                    trigger: new CallbackTrigger(() => wann.IsInside(TimeManager)),
                    stundeImStundenplan: stunde,
                    reputationsAenderungBeiFehlzeit: -0.4f,
                    saetzeAufDemWegZumRaum: unterrichtWeg,
                    saetzeBegruessung: unterrichtBegruessung,
                    saetzeWaehrendUnterricht: unterricht,
                    animationWeg: gehenAnimation,
                    animationAngekommen: unterrichtenAnimationNormal
                ));
            }

            void FakeUnterrichten(WoechentlicheZeitspannen wann)
            {
                ai.AddGoal(new MoveToAndStandAtGoal(
                    lehrer: Lehrer,
                    position: UnterrichtsStandpunkt.position,
                    trigger: new CallbackTrigger(() => wann.IsInside(TimeManager) && Lehrer.Vergiftung is {Syntome: true, VergiftungsType: VergiftungsType.Normal}),
                    saetzeWeg: unterrichtWegKrank,
                    saetzeAngekommenEinmalig: unterrichtBegruessung,
                    saetzeAngekommen: unterrichtKrank
                ));
                
                ai.AddGoal(new MoveToAndStandAtGoal(
                    lehrer: Lehrer,
                    position: UnterrichtsStandpunkt.position,
                    trigger: new CallbackTrigger(() => wann.IsInside(TimeManager) && !Internet.IsInternetVerfuegbar),
                    saetzeWeg: unterrichtWeg,
                    saetzeAngekommenEinmalig: unterrichtBegruessung,
                    saetzeAngekommen: unterrichtKeinInternet,
                    animationWeg: gehenAnimation,
                    animationAngekommen: unterrichtenAnimationKeinInternet
                ));

                ai.AddGoal(new MoveToAndStandAtGoal(
                    lehrer: Lehrer,
                    position: UnterrichtsStandpunkt.position,
                    trigger: new CallbackTrigger(() => wann.IsInside(TimeManager)),
                    saetzeWeg: unterrichtWeg,
                    saetzeAngekommenEinmalig: unterrichtBegruessung,
                    saetzeAngekommen: unterricht
                ));
            }

            void RauchenGehen(WoechentlicheZeitspannen wann, Vector3 position)
            {
                ai.AddGoal(new MoveToAndStandAtGoal(
                    lehrer: Lehrer,
                    trigger: new ZeitspanneTrigger(TimeManager, wann),
                    position: position,
                    saetzeWeg: rauchenWeg,
                    saetzeAngekommen: rauchen
                ));
            }

            void KurzpauseRauchenRoutine(Wochentag wochentag, int index, float davor, float dannach)
            {
                RauchenGehen(new WoechentlicheZeitspannen(
                    wochentag,
                    new StundeZeitRelativitaet(StundenType.Kurzpause, index, AnfangOderEnde.Anfang), davor,
                    new StundeZeitRelativitaet(StundenType.Kurzpause, index, AnfangOderEnde.Anfang), 15f.MinutesToHours()
                ), RauchenVorne.position);

                RauchenGehen(new WoechentlicheZeitspannen(
                    wochentag,
                    new StundeZeitRelativitaet(StundenType.Kurzpause, index, AnfangOderEnde.Anfang), 15f.MinutesToHours(),
                    new StundeZeitRelativitaet(StundenType.Kurzpause, index, AnfangOderEnde.Ende), dannach
                ), RauchenHinten.position);
            }

            void MittagspauseRauchenRoutine(Wochentag wochentag, float davor, float dannach)
            {
                RauchenGehen(new WoechentlicheZeitspannen(
                    wochentag,
                    new StundeZeitRelativitaet(StundenType.Mittagspause, AnfangOderEnde.Anfang), davor,
                    new StundeZeitRelativitaet(StundenType.Mittagspause, AnfangOderEnde.Anfang), 15f.MinutesToHours()
                ), RauchenVorne.position);

                RauchenGehen(new WoechentlicheZeitspannen(
                    wochentag,
                    new StundeZeitRelativitaet(StundenType.Mittagspause, AnfangOderEnde.Anfang), 15f.MinutesToHours(),
                    new StundeZeitRelativitaet(StundenType.Mittagspause, AnfangOderEnde.Anfang), 30f.MinutesToHours()
                ), RauchenHinten.position);

                KaffeTrinken(new WoechentlicheZeitspannen(
                    wochentag,
                    new StundeZeitRelativitaet(StundenType.Mittagspause, AnfangOderEnde.Anfang), 30f.MinutesToHours(),
                    new StundeZeitRelativitaet(StundenType.Mittagspause, AnfangOderEnde.Anfang), 45f.MinutesToHours()
                ));

                RauchenGehen(new WoechentlicheZeitspannen(
                    wochentag,
                    new StundeZeitRelativitaet(StundenType.Mittagspause, AnfangOderEnde.Anfang), 45f.MinutesToHours(),
                    new StundeZeitRelativitaet(StundenType.Mittagspause, AnfangOderEnde.Ende), dannach
                ), RauchenVorne.position);
            }

            void FahrradKontrolle(WoechentlicheZeitspannen wann)
            {
                ai.AddGoal(new MoveToAndStandAtGoal(
                    lehrer: Lehrer,
                    trigger: new ZeitspanneTrigger(TimeManager, wann),
                    position: EingangFahhradKontrolle.position,
                    saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                        "Absteigen bitte!",
                        "Bitte nicht auf dem Schulhof mit dem Drahtesel fahren!"
                    )
                ));
            }

            void KaffeTrinken(WoechentlicheZeitspannen wann)
            {
                ai.AddGoal(new VergiftbaresEssenEssenGoal(
                    lehrer: Lehrer,
                    trigger: new ZeitspanneTrigger(TimeManager, wann),
                    vergiftbaresEssen: KaffeemaschineRaucherLehrerzimmer,
                    saetzeWeg: new SaetzeMoeglichkeitenMehrmals(
                        "Ein Kaffe am Morgen hat noch niemandem geschadet"
                    )
                ));
            }

            void LateinNachhilfe(WoechentlicheZeitspannen wann)
            {
                ai.AddGoal(new MoveToAndStandAtGoal(
                    lehrer: Lehrer,
                    trigger: new ZeitspanneTrigger(TimeManager, wann),
                    position: UnterrichtsStandpunkt.position,
                    saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                        "Hat Frau Kropp in der letzten Woche denn keine Nachhilfe gemacht? Komisch, dass die immer krank ist.",
                        "Auch in der Nachhilfe gilt: Lieber weniger sorgfältig als mehr sorgfältig"
                    )
                ));
            }

            #region Hohe Priorität

            var zeitInDerSchule = new WoechentlicheZeitspannen(
                new WoechentlicheZeitspannen.Eintrag(
                    new PredicateWochentagAuswahl(w => w != Wochentag.Dienstag && w.GetEigenschaften().Contains(WochentagEigenschaft.Schultag)),
                    new WoechentlicheZeitspannen.Zeitspanne(
                        new WoechentlicheZeitspannen.Zeitpunkt(new SchuleBeginnZeitRelativitaet(), -1f),
                        new WoechentlicheZeitspannen.Zeitpunkt(new SchuleEndeZeitRelativitaet(), 0.5f)
                    )
                ),
                new WoechentlicheZeitspannen.Eintrag(
                    new ManuelleWochentagAuswahl(Wochentag.Dienstag),
                    new WoechentlicheZeitspannen.Zeitspanne(
                        new WoechentlicheZeitspannen.Zeitpunkt(new SchuleBeginnZeitRelativitaet(), -1f),
                        new WoechentlicheZeitspannen.Zeitpunkt(new SchuleEndeZeitRelativitaet(), 2f)
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
                trigger: new CallbackTrigger(() => AlarmManager.IsAlarm),
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
                    "Ich muss mal dringend auf Toilette eilen"
                )
            ));

            ai.AddGoal(new VerbrechenErkennenGoal(
                lehrer: Lehrer,
                trigger: new AlwaysTrueTrigger(),
                player: Player,
                reaktion: new SaetzeMoeglichkeitenEinmalig(
                    "Ich bin ein böster Drache und ich werde dich fressen, wenn du das nochmal machst"
                )
            ));

            ai.AddGoal(new VerbrechenMeldenGoal(
                lehrer: Lehrer,
                trigger: new ZeitspanneTrigger(
                    TimeManager,
                    new WoechentlicheZeitspannen(
                        new WoechentlicheZeitspannen.Eintrag(
                            new EigenschaftWochentagAuswahl(WochentagEigenschaft.Schultag),
                            new WoechentlicheZeitspannen.Zeitspanne(
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Mittagspause, AnfangOderEnde.Anfang), 0f),
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Mittagspause, AnfangOderEnde.Ende), 0f)
                            )
                        )
                    )
                ),
                player: Player,
                schulleitungsBuero: Schulleitung.position,
                saetzeWeg: new SaetzeMoeglichkeitenMehrmals(
                    "Jetzt kann ich sogar nicht rauchen, weil ich diesen Schüler melden muss"
                )
            ));

            #endregion

            #region Montag

            RauchenGehen(new WoechentlicheZeitspannen(
                Wochentag.Montag,
                new SchuleBeginnZeitRelativitaet(), -1f,
                new SchuleBeginnZeitRelativitaet(), -0.5f
            ), RauchenVorne.position);
            FahrradKontrolle(new WoechentlicheZeitspannen(
                Wochentag.Montag,
                new SchuleBeginnZeitRelativitaet(), -0.5f,
                new SchuleBeginnZeitRelativitaet(), unterrichtVerspaetung
            ));

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Montag, StundenType.Fach, 0, unterrichtVerspaetung, unterrichtUeberziehung));

            KurzpauseRauchenRoutine(Wochentag.Montag, 0, unterrichtUeberziehung, unterrichtVerspaetung);

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Montag, StundenType.Fach, 1, unterrichtVerspaetung, unterrichtUeberziehung));

            KurzpauseRauchenRoutine(Wochentag.Montag, 1, unterrichtUeberziehung, unterrichtVerspaetung);

            Unterrichten(
                new WoechentlicheZeitspannen(Wochentag.Montag, StundenType.Fach, 2, unterrichtVerspaetung, unterrichtUeberziehung),
                new UnterrichtenGoal.StundenData(Wochentag.Montag, 2, "Latein")
            );

            MittagspauseRauchenRoutine(Wochentag.Montag, unterrichtUeberziehung, unterrichtVerspaetung);

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Montag, StundenType.Fach, 3, unterrichtVerspaetung, unterrichtUeberziehung));

            RauchenGehen(new WoechentlicheZeitspannen(
                Wochentag.Montag,
                new SchuleEndeZeitRelativitaet(), unterrichtUeberziehung,
                new SchuleEndeZeitRelativitaet(), 0.5f
            ), RauchenHinten.position);

            #endregion

            #region Dienstag

            RauchenGehen(new WoechentlicheZeitspannen(
                Wochentag.Dienstag,
                new SchuleBeginnZeitRelativitaet(), -1f,
                new SchuleBeginnZeitRelativitaet(), unterrichtVerspaetung
            ), RauchenVorne.position);

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Dienstag, StundenType.Fach, 0, unterrichtVerspaetung, unterrichtUeberziehung));

            KurzpauseRauchenRoutine(Wochentag.Dienstag, 0, unterrichtVerspaetung, unterrichtUeberziehung);

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Dienstag, StundenType.Fach, 1, unterrichtVerspaetung, unterrichtUeberziehung));

            KurzpauseRauchenRoutine(Wochentag.Dienstag, 1, unterrichtVerspaetung, unterrichtUeberziehung);

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Dienstag, StundenType.Fach, 2, unterrichtVerspaetung, unterrichtUeberziehung));

            LateinNachhilfe(new WoechentlicheZeitspannen(
                Wochentag.Dienstag,
                new SchuleEndeZeitRelativitaet(), unterrichtUeberziehung,
                new SchuleEndeZeitRelativitaet(), 1.5f
            ));
            RauchenGehen(new WoechentlicheZeitspannen(
                Wochentag.Dienstag,
                new SchuleEndeZeitRelativitaet(), 1.5f,
                new SchuleEndeZeitRelativitaet(), 2f
            ), RauchenHinten.position);

            #endregion

            #region Mittwoch

            RauchenGehen(new WoechentlicheZeitspannen(
                Wochentag.Mittwoch,
                new SchuleBeginnZeitRelativitaet(), -1f,
                new SchuleBeginnZeitRelativitaet(), -0.5f
            ), RauchenVorne.position);
            FahrradKontrolle(new WoechentlicheZeitspannen(
                Wochentag.Mittwoch,
                new SchuleBeginnZeitRelativitaet(), -0.5f,
                new SchuleBeginnZeitRelativitaet(), unterrichtVerspaetung
            ));

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Mittwoch, StundenType.Fach, 0, unterrichtVerspaetung, unterrichtUeberziehung));

            KurzpauseRauchenRoutine(Wochentag.Mittwoch, 0, unterrichtUeberziehung, unterrichtVerspaetung);

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Mittwoch, StundenType.Fach, 1, unterrichtVerspaetung, unterrichtUeberziehung));

            KurzpauseRauchenRoutine(Wochentag.Mittwoch, 1, unterrichtUeberziehung, unterrichtVerspaetung);

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Mittwoch, StundenType.Lernzeit, 0, unterrichtVerspaetung, unterrichtUeberziehung));

            MittagspauseRauchenRoutine(Wochentag.Mittwoch, unterrichtUeberziehung, unterrichtVerspaetung);

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Mittwoch, StundenType.Fach, 2, unterrichtVerspaetung, unterrichtUeberziehung));

            RauchenGehen(new WoechentlicheZeitspannen(
                Wochentag.Mittwoch,
                new SchuleEndeZeitRelativitaet(), unterrichtUeberziehung,
                new SchuleEndeZeitRelativitaet(), 0.5f
            ), RauchenHinten.position);

            #endregion

            #region Donnerstag

            RauchenGehen(new WoechentlicheZeitspannen(
                Wochentag.Donnernstag,
                new SchuleBeginnZeitRelativitaet(), -1f,
                new SchuleBeginnZeitRelativitaet(), unterrichtVerspaetung
            ), RauchenVorne.position);

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Donnernstag, StundenType.Fach, 0, unterrichtVerspaetung, unterrichtUeberziehung));

            KurzpauseRauchenRoutine(Wochentag.Donnernstag, 0, unterrichtUeberziehung, unterrichtVerspaetung);

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Donnernstag, StundenType.Fach, 1, unterrichtVerspaetung, unterrichtUeberziehung));

            KurzpauseRauchenRoutine(Wochentag.Donnernstag, 1, unterrichtUeberziehung, unterrichtVerspaetung);

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Donnernstag, StundenType.Lernzeit, 0, unterrichtVerspaetung, unterrichtUeberziehung));

            MittagspauseRauchenRoutine(Wochentag.Donnernstag, unterrichtUeberziehung, unterrichtVerspaetung);

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Donnernstag, StundenType.Fach, 2, unterrichtVerspaetung, unterrichtUeberziehung));

            RauchenGehen(new WoechentlicheZeitspannen(
                Wochentag.Donnernstag,
                new SchuleEndeZeitRelativitaet(), unterrichtUeberziehung,
                new SchuleEndeZeitRelativitaet(), 0.5f
            ), RauchenVorne.position);

            #endregion

            #region Freitag

            RauchenGehen(new WoechentlicheZeitspannen(
                Wochentag.Freitag,
                new SchuleBeginnZeitRelativitaet(), -1f,
                new SchuleBeginnZeitRelativitaet(), -0.5f
            ), RauchenHinten.position);
            FahrradKontrolle(new WoechentlicheZeitspannen(
                Wochentag.Freitag,
                new SchuleBeginnZeitRelativitaet(), -0.5f,
                new SchuleBeginnZeitRelativitaet(), unterrichtVerspaetung
            ));

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Freitag, StundenType.Fach, 0, unterrichtVerspaetung, unterrichtUeberziehung));

            KurzpauseRauchenRoutine(Wochentag.Freitag, 0, unterrichtVerspaetung, unterrichtUeberziehung);

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Freitag, StundenType.Fach, 1, unterrichtVerspaetung, unterrichtUeberziehung));

            KurzpauseRauchenRoutine(Wochentag.Freitag, 1, unterrichtVerspaetung, unterrichtUeberziehung);

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Freitag, StundenType.Fach, 2, unterrichtVerspaetung, unterrichtUeberziehung));

            RauchenGehen(new WoechentlicheZeitspannen(
                Wochentag.Freitag,
                new SchuleEndeZeitRelativitaet(), unterrichtUeberziehung,
                new SchuleEndeZeitRelativitaet(), 0.5f
            ), RauchenHinten.position);

            #endregion
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

            fragen.AddFrage(new InteraktionsMenuFrageEinfach(
                lehrer: Lehrer,
                player: Player,
                shouldShowPredicate: InteraktionsMenuFrageUtil.ShowNearby(),
                interaktionsMenuName: "Zigaretten schenken",
                kosten: 20,
                reputationsAenderung: 0.5f,
                antworten: new SaetzeMoeglichkeitenEinmalig("Danke dir! Meine sind mir gerade ausgegangen und ich hatte schon Angst dass meine Lunge sich mal ein paar Minuten erholen kann!")
            ));
        }
    }
}
