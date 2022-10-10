using HerderGames.Lehrer.AI;
using HerderGames.Lehrer.AI.Goals;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Animation;
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
        [SerializeField] private InternetManager Internet;
        [SerializeField] private Player.Player Player;
        [SerializeField] private Klassenraum UnterrichtsRaum;
        [SerializeField] private Transform UnterrichtStandpunkt;
        [SerializeField] private Transform SchuleHaupteingang;
        [SerializeField] private Transform AlarmSammelpunkt;
        [SerializeField] private Transform ToiletteLehrerzimmer;
        [SerializeField] private VergiftbaresEssen KaffeemaschineMiniRaum;
        [SerializeField] private Transform LehrerzimmerFach;
        [SerializeField] private Transform Rauchen;
        [SerializeField] private Transform Schulleitung;
        [SerializeField] private Transform Drucker;
        [SerializeField] private Transform E202ZigarettenSchrank;

        protected override void RegisterGoals(AIController ai)
        {
            #region Sätze

            var unterrichtWeg = new SaetzeMoeglichkeitenMehrmals(
                "Ich hoffe meine Schüler haben den Schimmelreiter gelesen",
                "Jetzt mal ganz ehrlich: Treppensteigen war auch mal einfacher"
            );

            var unterrichtBegruessung = new SaetzeMoeglichkeitenEinmalig(
                "Na ihr Hasis?"
            );

            var unterrichtSaetze = new SaetzeMoeglichkeitenMehrmals(
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
                "Das ist ja zum Mäuse melken!",
                "Jetzt mal ganz ehrlich, kann mir jemand sagen, wie ich mich am Schul PC abmelde?",
                "Schwarzer Pfeffer hilft gegen Athrose? Das ist interessant",
                "Also wenn ich mir diese Gruselfilme, die ihr guckt, angucken müsste, würde ich mich hinter meinem Ohrensessel verkriechen!",
                "Gar nichts muss man ... außer sterben! Das sagte schon Göthe die Flöte.",
                "Moni hat Cooroni",
                "Also ich fand diesen graphische Würfelkonstruktions Übergang in deiner Präsentation sehr toll und gar nicht redundant.",
                "Klimaschutz ist ein sehr wichtiges Thema! Das liegt mir wirklich sehr am Herz. Sieht man ja auch daran, dass meine Echtschlangenleder Tasche fair-trade ist.",
                "Ne Tschick war so ein schlechtes Buch das lese ich nie wieder",
                "Der Schimmelreiter ist ein wunderbares Buch genauso wie der Autor Theodor Sturm ein vorbildlicher Mensch war",
                "Ich lebe übrigens ein sehr bescheidendes Leben in einer kleinen rieseigen Villa im Hahnenwald Villenviertel."
            );

            var druckenWeg = new SaetzeMoeglichkeitenMehrmals(
                "Ich muss noch mal kurz ein paar Arbeitsblätter ausdrucken gehen.",
                "Hoffentlich reicht mein Druckerkontigent noch."
            );

            #endregion

            #region Animationen

            var idleAnimation = new SimpleAnimation(AnimationName.Stehen);

            var gehenAnimation = new SimpleAnimation(AnimationName.GehenKrank);

            var unterrichtenAnimation = new RepeatAnimation(new ShuffleAnimation(
                new ShuffleAnimation.Choice(10, new SimpleAnimation(AnimationName.Reden, 5)),
                new ShuffleAnimation.Choice(1, new SimpleAnimation(AnimationName.RedenAggressiv, 10)),
                new ShuffleAnimation.Choice(2, new SimpleAnimation(AnimationName.Rueckenschmerzen, 5)),
                new ShuffleAnimation.Choice(2, new SimpleAnimation(AnimationName.Stehen, 8))
            ));

            #endregion

            var unterrichtVerspaetung = (-5f).MinutesToHours();
            var unterrichtUeberziehung = 5f.MinutesToHours();

            void EchtUnterrichten(WoechentlicheZeitspannen wann, UnterrichtenGoal.StundenData stunde)
            {
                ai.AddGoal(new UnterrichtenGoal(
                    lehrer: Lehrer,
                    unterrichtsRaum: UnterrichtsRaum,
                    standpunkt: UnterrichtStandpunkt.position,
                    trigger: new ZeitspanneTrigger(TimeManager, wann),
                    stundeImStundenplan: stunde,
                    reputationsAenderungBeiFehlzeit: -1f,
                    saetzeAufDemWegZumRaum: unterrichtWeg,
                    saetzeBegruessung: unterrichtBegruessung,
                    saetzeWaehrendUnterricht: unterrichtSaetze,
                    animationWeg: gehenAnimation,
                    animationAngekommen: unterrichtenAnimation
                ));
            }

            void FakeUnterrichten(WoechentlicheZeitspannen wann)
            {
                ai.AddGoal(new MoveToAndStandAtGoal(
                    lehrer: Lehrer,
                    trigger: new ZeitspanneTrigger(TimeManager, wann),
                    position: UnterrichtStandpunkt.position,
                    saetzeWeg: unterrichtWeg,
                    saetzeAngekommenEinmalig: unterrichtBegruessung,
                    saetzeAngekommen: unterrichtSaetze,
                    animationWeg: gehenAnimation,
                    animationAngekommen: unterrichtenAnimation
                ));
            }

            void RauchenGehenVorne(WoechentlicheZeitspannen wann)
            {
                ai.AddGoal(new MoveToAndStandAtGoal(
                    lehrer: Lehrer,
                    trigger: new ZeitspanneTrigger(TimeManager, wann),
                    position: Rauchen.position,
                    saetzeWeg: new SaetzeMoeglichkeitenMehrmals(
                        "Hand aufs Herz: Wir alle wollen auch mal Rauchen",
                        "Jetzt mal ganz ehrlich: Ich muss noch mal kurz Rauchen gehen",
                        "Jetzt mal ganz ehrlich: Rauchen kann auch gut für die Gesundheit sein",
                        "Mensch Schulten, du musst dir das Rauchen wirklich mal abgewöhnen!"
                    ),
                    saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                        "Jetzt müsst ihr leider zugucken wie ich rauche weil ich meine Sucht nicht für eine Minute länger unterdrücken kann",
                        "Hoppala, wo sind denn die ganzen Zigaretten hin? Naja dann muss ich gleich halt noch mal zu E202",
                        "Ohgottogottogott Diese Warnhinweise machen wir ja schon Angst!"
                    ),
                    animationWeg: gehenAnimation,
                    animationAngekommen: new RepeatAnimation(new ShuffleAnimation(
                        new ShuffleAnimation.Choice(3, new SimpleAnimation(AnimationName.Rauchen, 20)),
                        new ShuffleAnimation.Choice(4, new SimpleAnimation(AnimationName.Rueckenschmerzen, 3)),
                        new ShuffleAnimation.Choice(1, new SimpleAnimation(AnimationName.Stehen, 5))
                    ))
                ));
            }

            void KaffeeTrinkenGehenMiniraum(WoechentlicheZeitspannen wann)
            {
                ai.AddGoal(new VergiftbaresEssenEssenGoal(
                    lehrer: Lehrer,
                    trigger: new ZeitspanneTrigger(TimeManager, wann),
                    vergiftbaresEssen: KaffeemaschineMiniRaum,
                    saetzeWeg: new SaetzeMoeglichkeitenMehrmals(
                        "Hand aufs Herz: Kaffee am morgen vertreibt Kummer und Sorgen",
                        "Ich hoffe die Kaffeemaschine funktioniert noch",
                        "Ihr fragt euch warum ich einen eigenen Mini Raum habe? Naja irgendwo müssen die Zigaretten ja hin"
                    ),
                    saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                        "Jetzt mal ganz ehrlich: Kaffee tut gut!",
                        "Jetzt mal ganz ehrlich: Wir alle trinken auch mal Kaffee"
                    ),
                    animationWeg: gehenAnimation,
                    animationAngekommen: new SimpleAnimation(AnimationName.Trinken)
                ));
            }

            void ZumLehrerzimmerFachGehen(WoechentlicheZeitspannen wann)
            {
                ai.AddGoal(new MoveToAndStandAtGoal(
                    lehrer: Lehrer,
                    trigger: new ZeitspanneTrigger(TimeManager, wann),
                    position: LehrerzimmerFach.position,
                    saetzeWeg: new SaetzeMoeglichkeitenMehrmals(
                        "Mmmhhm mal gucken ob in meinem Fach was liegt. Da hat bestimmt jemand in der Nacht was reigelegt",
                        "Ich muss nochmal kurz ins Lehrerzimmer"
                    ),
                    saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                        "Komisch, warum liegt denn gar nichts in meinem Fach. " +
                        "Ich bin doch gestern schon als letzte gegangen und heute erst als erste wieder in die Schule gekommen"
                    ),
                    animationWeg: gehenAnimation,
                    animationAngekommen: idleAnimation
                ));
            }

            void LehrerzimmerDruckenGehen(WoechentlicheZeitspannen wann)
            {
                ai.AddGoal(new MoveToAndStandAtGoal(
                    lehrer: Lehrer,
                    trigger: new CallbackTrigger(() => wann.IsInside(TimeManager) && !Internet.IsInternetVerfuegbar),
                    position: Drucker.position,
                    saetzeWeg: druckenWeg,
                    saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                        "Ohgottogottogott, warum kann Ich denn nichts drucken? Warum funktioniert das Internet denn nicht?",
                        "Das Internet funktioniert nicht jetzt kann ich gar nicht meine hunterttausend Arbeitsblätter ausfrucken!"
                    ),
                    animationWeg: gehenAnimation,
                    animationAngekommen: idleAnimation
                ));

                ai.AddGoal(new MoveToAndStandAtGoal(
                    lehrer: Lehrer,
                    trigger: new ZeitspanneTrigger(TimeManager, wann),
                    position: Drucker.position,
                    saetzeWeg: druckenWeg,
                    saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                        "Hoppala, warum ist mein Druckerkontingent denn schon aufgebraucht?",
                        "Wie kann mein Druckerkontigent denn schon leer sein?? Ich drucke doch nur täglich ca 2000 Blätter. Das sollte doch eigentlich drinnen sein!"
                    ),
                    animationWeg: gehenAnimation,
                    animationAngekommen: idleAnimation
                ));
            }

            void E202ZigarettenHolen(WoechentlicheZeitspannen wann)
            {
                ai.AddGoal(new SchuleVerlassenGoal(
                    lehrer: Lehrer,
                    trigger: new ZeitspanneTrigger(TimeManager, wann),
                    ausgang: E202ZigarettenSchrank.position,
                    eingang: E202ZigarettenSchrank.position,
                    saetzeBeimVerlassen: new SaetzeMoeglichkeitenMehrmals(
                        "Ich muss noch mal kurz zu E202 und dort Zigarre...Äähm...etwas holen.",
                        "Ich hab meine Schlangenledertasche in E202 vergessen."
                    ),
                    animationBeimVerlassen: gehenAnimation
                ));
            }

            #region Hohe Priorität

            var zeitInSchule = new WoechentlicheZeitspannen(
                new WoechentlicheZeitspannen.Eintrag(
                    new ManuelleWochentagAuswahl(Wochentag.Montag),
                    new WoechentlicheZeitspannen.Zeitspanne(
                        new WoechentlicheZeitspannen.Zeitpunkt(new SchuleBeginnZeitRelativitaet(), -2f),
                        new WoechentlicheZeitspannen.Zeitpunkt(new SchuleEndeZeitRelativitaet(), unterrichtUeberziehung)
                    )
                ),
                new WoechentlicheZeitspannen.Eintrag(
                    new ManuelleWochentagAuswahl(Wochentag.Dienstag),
                    new WoechentlicheZeitspannen.Zeitspanne(
                        new WoechentlicheZeitspannen.Zeitpunkt(new SchuleBeginnZeitRelativitaet(), -2f),
                        new WoechentlicheZeitspannen.Zeitpunkt(new SchuleEndeZeitRelativitaet(), 1f)
                    )
                ),
                new WoechentlicheZeitspannen.Eintrag(
                    new EigenschaftWochentagAuswahl(WochentagEigenschaft.LangtagNormal),
                    new WoechentlicheZeitspannen.Zeitspanne(
                        new WoechentlicheZeitspannen.Zeitpunkt(new SchuleBeginnZeitRelativitaet(), -2f),
                        new WoechentlicheZeitspannen.Zeitpunkt(new SchuleEndeZeitRelativitaet(), 0.5f)
                    )
                ),
                new WoechentlicheZeitspannen.Eintrag(
                    new ManuelleWochentagAuswahl(Wochentag.Freitag),
                    new WoechentlicheZeitspannen.Zeitspanne(
                        new WoechentlicheZeitspannen.Zeitpunkt(new SchuleBeginnZeitRelativitaet(), -2f),
                        new WoechentlicheZeitspannen.Zeitpunkt(new SchuleEndeZeitRelativitaet(), 1.5f)
                    )
                )
            );

            ai.AddGoal(new SchuleVerlassenGoal(
                lehrer: Lehrer,
                trigger: new CallbackTrigger(() => !zeitInSchule.IsInside(TimeManager)),
                eingang: SchuleHaupteingang.position,
                ausgang: SchuleHaupteingang.position,
                saetzeBeimVerlassen: new SaetzeMoeglichkeitenMehrmals(
                    "Jetzt mal ganz ehrlich, Wir alle wollen auch mal frei haben!",
                    "Hand aufs Herz: Den Feierabend hab ich mir verdient!"
                ),
                animationBeimVerlassen: gehenAnimation
            ));

            ai.AddGoal(new SchuleVerlassenGoal( // Krankheit Normal
                lehrer: Lehrer,
                trigger: new CallbackTrigger(() => Lehrer.Vergiftung is {Syntome: true, VergiftungsType: VergiftungsType.Normal}),
                eingang: SchuleHaupteingang.position,
                ausgang: SchuleHaupteingang.position,
                animationBeimVerlassen: gehenAnimation
            ));

            ai.AddGoal(new MoveToAndStandAtGoal( // Feueralarm
                lehrer: Lehrer,
                trigger: new CallbackTrigger(() => AlarmManager.IsAlarm),
                position: AlarmSammelpunkt.position,
                saetzeWeg: new SaetzeMoeglichkeitenMehrmals(
                    "Hilfeee!!!",
                    "Rettet mich!!!",
                    "Es brennt!!!"
                ),
                saetzeAngekommen: new SaetzeMoeglichkeitenMehrmals(
                    "Oh nein! Ich habe meine Zigaretten im Raum vergessen!",
                    "Dass ich das auf meine alten Tage noch erleben muss!"
                ),
                animationWeg: new SimpleAnimation(AnimationName.BetrunkenRennen),
                animationAngekommen: idleAnimation
            ));

            ai.AddGoal(new ErschoepfungGoal(
                lehrer: Lehrer,
                trigger: new AlwaysTrueTrigger(),
                maxiamleHoeheProMinute: 5f,
                maximaleDistanzProMinute: 40f,
                laengeDerPause: 3.5f,
                saetze: new SaetzeMoeglichkeitenMehrmals(
                    "Jetzt mal ganz ehrlich: Ich brauche mal eine kurze Pause",
                    "Hand aufs Herz: Wir alle sind auch mal erschöpft!"
                ),
                animation: new RepeatAnimation(new TimelineAnimation(
                    new SimpleAnimation(AnimationName.SchmerzBoden, 6),
                    new SimpleAnimation(AnimationName.Rueckenschmerzen, 4)
                ))
            ));

            ai.AddGoal(new SchuleVerlassenGoal( // Krankheit Orthomol
                lehrer: Lehrer,
                trigger: new CallbackTrigger(() => Lehrer.Vergiftung is {Syntome: true, VergiftungsType: VergiftungsType.Orthamol}),
                eingang: ToiletteLehrerzimmer.position,
                ausgang: ToiletteLehrerzimmer.position,
                saetzeBeimVerlassen: new SaetzeMoeglichkeitenMehrmals(
                    "Aus dem Weg, Ich muss mal ganz dringend auf Toilette",
                    "Mmmhhhhm Ich mag mein Orthomol!",
                    "Jetzt mal ganz ehrlich wir alle müssen mal auf Toilette",
                    "Hand aufs Herz: Ich hab zu viele Fressalien gegessen"
                ),
                animationBeimVerlassen: gehenAnimation
            ));

            ai.AddGoal(new VerbrechenErkennenGoal(
                lehrer: Lehrer,
                trigger: new AlwaysTrueTrigger(),
                player: Player,
                reaktion: new SaetzeMoeglichkeitenEinmalig(
                    "Hey Was machst du denn da? Ich bin sauer! Nein ich bin wütend"
                ),
                animationWeg: new SimpleAnimation(AnimationName.BetrunkenRennen),
                animationAngekommen: new SimpleAnimation(AnimationName.RedenAggressiv)
            ));

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
                ),
                animationWeg: gehenAnimation,
                animationAngekommen: new SimpleAnimation(AnimationName.RedenAggressiv)
            ));

            #endregion

            #region Montag

            RauchenGehenVorne(new WoechentlicheZeitspannen(
                Wochentag.Montag,
                new SchuleBeginnZeitRelativitaet(), -2f,
                new SchuleBeginnZeitRelativitaet(), -1.5f
            ));
            KaffeeTrinkenGehenMiniraum(new WoechentlicheZeitspannen(
                Wochentag.Montag,
                new SchuleBeginnZeitRelativitaet(), -1.5f,
                new SchuleBeginnZeitRelativitaet(), -1f
            ));
            ZumLehrerzimmerFachGehen(new WoechentlicheZeitspannen(
                Wochentag.Montag,
                new SchuleBeginnZeitRelativitaet(), -1f,
                new SchuleBeginnZeitRelativitaet(), -0.5f
            ));
            RauchenGehenVorne(new WoechentlicheZeitspannen(
                Wochentag.Montag,
                new SchuleBeginnZeitRelativitaet(), -0.5f,
                new SchuleBeginnZeitRelativitaet(), unterrichtVerspaetung
            ));

            EchtUnterrichten(
                new WoechentlicheZeitspannen(Wochentag.Montag, StundenType.Fach, 0, unterrichtVerspaetung, unterrichtUeberziehung),
                new UnterrichtenGoal.StundenData(Wochentag.Montag, 0, "Deutsch")
            );

            RauchenGehenVorne(new WoechentlicheZeitspannen(
                Wochentag.Montag,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 0, AnfangOderEnde.Anfang), unterrichtUeberziehung,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 0, AnfangOderEnde.Anfang), 15f.MinutesToHours()
            ));
            LehrerzimmerDruckenGehen(new WoechentlicheZeitspannen(
                Wochentag.Montag,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 0, AnfangOderEnde.Anfang), 15f.MinutesToHours(),
                new StundeZeitRelativitaet(StundenType.Kurzpause, 0, AnfangOderEnde.Ende), unterrichtVerspaetung
            ));

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Montag, StundenType.Fach, 1, unterrichtVerspaetung, unterrichtUeberziehung));

            RauchenGehenVorne(new WoechentlicheZeitspannen(
                Wochentag.Montag,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 1, AnfangOderEnde.Anfang), unterrichtUeberziehung,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 1, AnfangOderEnde.Anfang), 15f.MinutesToHours()
            ));
            KaffeeTrinkenGehenMiniraum(new WoechentlicheZeitspannen(
                Wochentag.Montag,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 1, AnfangOderEnde.Anfang), 15f.MinutesToHours(),
                new StundeZeitRelativitaet(StundenType.Kurzpause, 1, AnfangOderEnde.Ende), unterrichtVerspaetung
            ));

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Montag, StundenType.Fach, 2, unterrichtVerspaetung, unterrichtUeberziehung));

            RauchenGehenVorne(new WoechentlicheZeitspannen(
                Wochentag.Montag,
                new StundeZeitRelativitaet(StundenType.Mittagspause, AnfangOderEnde.Anfang), unterrichtUeberziehung,
                new StundeZeitRelativitaet(StundenType.Mittagspause, AnfangOderEnde.Anfang), 15f.MinutesToHours()
            ));
            ZumLehrerzimmerFachGehen(new WoechentlicheZeitspannen(
                Wochentag.Montag,
                new StundeZeitRelativitaet(StundenType.Mittagspause, AnfangOderEnde.Anfang), 15f.MinutesToHours(),
                new StundeZeitRelativitaet(StundenType.Mittagspause, AnfangOderEnde.Anfang), 30f.MinutesToHours()
            ));
            RauchenGehenVorne(new WoechentlicheZeitspannen(
                Wochentag.Montag,
                new StundeZeitRelativitaet(StundenType.Mittagspause, AnfangOderEnde.Anfang), 30f.MinutesToHours(),
                new StundeZeitRelativitaet(StundenType.Mittagspause, AnfangOderEnde.Ende), unterrichtVerspaetung
            ));

            FakeUnterrichten(new WoechentlicheZeitspannen(
                Wochentag.Montag,
                new StundeZeitRelativitaet(StundenType.Fach, 3, AnfangOderEnde.Anfang), unterrichtVerspaetung,
                new StundeZeitRelativitaet(StundenType.Fach, 3, AnfangOderEnde.Ende), unterrichtUeberziehung
            ));

            #endregion

            #region Dienstag

            RauchenGehenVorne(new WoechentlicheZeitspannen(
                Wochentag.Dienstag,
                new SchuleBeginnZeitRelativitaet(), -2f,
                new SchuleBeginnZeitRelativitaet(), -1.5f
            ));
            KaffeeTrinkenGehenMiniraum(new WoechentlicheZeitspannen(
                Wochentag.Dienstag,
                new SchuleBeginnZeitRelativitaet(), -1.5f,
                new SchuleBeginnZeitRelativitaet(), -1f
            ));
            LehrerzimmerDruckenGehen(new WoechentlicheZeitspannen(
                Wochentag.Dienstag,
                new SchuleBeginnZeitRelativitaet(), -1f,
                new SchuleBeginnZeitRelativitaet(), -0.5f
            ));
            RauchenGehenVorne(new WoechentlicheZeitspannen(
                Wochentag.Dienstag,
                new SchuleBeginnZeitRelativitaet(), -0.5f,
                new SchuleBeginnZeitRelativitaet(), unterrichtVerspaetung
            ));

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Dienstag, StundenType.Fach, 0, unterrichtVerspaetung, unterrichtUeberziehung));

            RauchenGehenVorne(new WoechentlicheZeitspannen(
                Wochentag.Dienstag,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 0, AnfangOderEnde.Anfang), unterrichtUeberziehung,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 0, AnfangOderEnde.Anfang), 15f.MinutesToHours()
            ));
            ZumLehrerzimmerFachGehen(new WoechentlicheZeitspannen(
                Wochentag.Dienstag,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 0, AnfangOderEnde.Anfang), 15f.MinutesToHours(),
                new StundeZeitRelativitaet(StundenType.Kurzpause, 0, AnfangOderEnde.Ende), unterrichtVerspaetung
            ));

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Dienstag, StundenType.Fach, 1, unterrichtVerspaetung, unterrichtUeberziehung));

            RauchenGehenVorne(new WoechentlicheZeitspannen(
                Wochentag.Dienstag,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 1, AnfangOderEnde.Anfang), unterrichtUeberziehung,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 1, AnfangOderEnde.Anfang), 15f.MinutesToHours()
            ));
            ZumLehrerzimmerFachGehen(new WoechentlicheZeitspannen(
                Wochentag.Dienstag,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 1, AnfangOderEnde.Anfang), 15f.MinutesToHours(),
                new StundeZeitRelativitaet(StundenType.Kurzpause, 1, AnfangOderEnde.Ende), unterrichtVerspaetung
            ));

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Dienstag, StundenType.Fach, 2, unterrichtVerspaetung, unterrichtUeberziehung));

            RauchenGehenVorne(new WoechentlicheZeitspannen(
                Wochentag.Dienstag,
                new SchuleEndeZeitRelativitaet(), unterrichtUeberziehung,
                new SchuleEndeZeitRelativitaet(), 0.5f
            ));
            LehrerzimmerDruckenGehen(new WoechentlicheZeitspannen(
                Wochentag.Dienstag,
                new SchuleEndeZeitRelativitaet(), 0.5f,
                new SchuleEndeZeitRelativitaet(), 1f
            ));

            #endregion

            #region Mittwoch

            RauchenGehenVorne(new WoechentlicheZeitspannen(
                Wochentag.Mittwoch,
                new SchuleBeginnZeitRelativitaet(), -2f,
                new SchuleBeginnZeitRelativitaet(), -0.5f
            ));
            ZumLehrerzimmerFachGehen(new WoechentlicheZeitspannen(
                Wochentag.Mittwoch,
                new SchuleBeginnZeitRelativitaet(), -0.5f,
                new SchuleBeginnZeitRelativitaet(), unterrichtVerspaetung
            ));

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Mittwoch, StundenType.Fach, 0, unterrichtVerspaetung, unterrichtUeberziehung));

            RauchenGehenVorne(new WoechentlicheZeitspannen(
                Wochentag.Mittwoch,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 0, AnfangOderEnde.Anfang), unterrichtUeberziehung,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 0, AnfangOderEnde.Ende), unterrichtVerspaetung
            ));

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Mittwoch, StundenType.Fach, 1, unterrichtVerspaetung, unterrichtUeberziehung));

            RauchenGehenVorne(new WoechentlicheZeitspannen(
                Wochentag.Mittwoch,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 1, AnfangOderEnde.Anfang), unterrichtUeberziehung,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 1, AnfangOderEnde.Anfang), 15f.MinutesToHours()
            ));
            KaffeeTrinkenGehenMiniraum(new WoechentlicheZeitspannen(
                Wochentag.Mittwoch,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 1, AnfangOderEnde.Anfang), 15f.MinutesToHours(),
                new StundeZeitRelativitaet(StundenType.Kurzpause, 1, AnfangOderEnde.Ende), unterrichtVerspaetung
            ));

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Mittwoch, StundenType.Lernzeit, 0, unterrichtVerspaetung, unterrichtUeberziehung));

            RauchenGehenVorne(new WoechentlicheZeitspannen(Wochentag.Mittwoch, StundenType.Mittagspause, 0, unterrichtUeberziehung, unterrichtVerspaetung));

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Mittwoch, StundenType.Fach, 2, unterrichtVerspaetung, unterrichtUeberziehung));

            LehrerzimmerDruckenGehen(new WoechentlicheZeitspannen(
                Wochentag.Mittwoch,
                new SchuleEndeZeitRelativitaet(), unterrichtUeberziehung,
                new SchuleEndeZeitRelativitaet(), 0.5f
            ));

            #endregion

            #region Donnerstag

            RauchenGehenVorne(new WoechentlicheZeitspannen(
                Wochentag.Donnernstag,
                new SchuleBeginnZeitRelativitaet(), -2f,
                new SchuleBeginnZeitRelativitaet(), -0.5f
            ));
            ZumLehrerzimmerFachGehen(new WoechentlicheZeitspannen(
                Wochentag.Donnernstag,
                new SchuleBeginnZeitRelativitaet(), -0.5f,
                new SchuleBeginnZeitRelativitaet(), unterrichtVerspaetung
            ));

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Donnernstag, StundenType.Fach, 0, unterrichtVerspaetung, unterrichtUeberziehung));

            RauchenGehenVorne(new WoechentlicheZeitspannen(
                Wochentag.Donnernstag,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 0, AnfangOderEnde.Anfang), unterrichtUeberziehung,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 0, AnfangOderEnde.Ende), unterrichtVerspaetung
            ));

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Donnernstag, StundenType.Fach, 1, unterrichtVerspaetung, unterrichtUeberziehung));

            RauchenGehenVorne(new WoechentlicheZeitspannen(
                Wochentag.Donnernstag,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 1, AnfangOderEnde.Anfang), unterrichtUeberziehung,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 1, AnfangOderEnde.Anfang), 15f.MinutesToHours()
            ));
            KaffeeTrinkenGehenMiniraum(new WoechentlicheZeitspannen(
                Wochentag.Donnernstag,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 1, AnfangOderEnde.Anfang), 15f.MinutesToHours(),
                new StundeZeitRelativitaet(StundenType.Kurzpause, 1, AnfangOderEnde.Ende), unterrichtVerspaetung
            ));

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Donnernstag, StundenType.Lernzeit, 0, unterrichtVerspaetung, unterrichtUeberziehung));

            RauchenGehenVorne(new WoechentlicheZeitspannen(Wochentag.Donnernstag, StundenType.Mittagspause, 0, unterrichtUeberziehung, unterrichtVerspaetung));

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Donnernstag, StundenType.Fach, 2, unterrichtVerspaetung, unterrichtUeberziehung));

            LehrerzimmerDruckenGehen(new WoechentlicheZeitspannen(
                Wochentag.Donnernstag,
                new SchuleEndeZeitRelativitaet(), unterrichtUeberziehung,
                new SchuleEndeZeitRelativitaet(), 0.5f
            ));

            #endregion

            #region Freitag

            RauchenGehenVorne(new WoechentlicheZeitspannen(
                Wochentag.Freitag,
                new SchuleBeginnZeitRelativitaet(), -2f,
                new SchuleBeginnZeitRelativitaet(), -1.5f
            ));
            LehrerzimmerDruckenGehen(new WoechentlicheZeitspannen(
                Wochentag.Freitag,
                new SchuleBeginnZeitRelativitaet(), -1.5f,
                new SchuleBeginnZeitRelativitaet(), -0.5f
            ));
            RauchenGehenVorne(new WoechentlicheZeitspannen(
                Wochentag.Freitag,
                new SchuleBeginnZeitRelativitaet(), -0.5f,
                new SchuleBeginnZeitRelativitaet(), unterrichtVerspaetung
            ));

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Freitag, StundenType.Fach, 0, unterrichtVerspaetung, unterrichtUeberziehung));

            RauchenGehenVorne(new WoechentlicheZeitspannen(
                Wochentag.Freitag,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 0, AnfangOderEnde.Anfang), unterrichtUeberziehung,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 0, AnfangOderEnde.Anfang), 15f.MinutesToHours()
            ));
            ZumLehrerzimmerFachGehen(new WoechentlicheZeitspannen(
                Wochentag.Freitag,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 0, AnfangOderEnde.Anfang), 15f.MinutesToHours(),
                new StundeZeitRelativitaet(StundenType.Kurzpause, 0, AnfangOderEnde.Ende), unterrichtVerspaetung
            ));

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Freitag, StundenType.Fach, 1, unterrichtVerspaetung, unterrichtUeberziehung));

            RauchenGehenVorne(new WoechentlicheZeitspannen(
                Wochentag.Freitag,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 1, AnfangOderEnde.Anfang), unterrichtUeberziehung,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 1, AnfangOderEnde.Anfang), 15f.MinutesToHours()
            ));
            ZumLehrerzimmerFachGehen(new WoechentlicheZeitspannen(
                Wochentag.Freitag,
                new StundeZeitRelativitaet(StundenType.Kurzpause, 1, AnfangOderEnde.Anfang), 15f.MinutesToHours(),
                new StundeZeitRelativitaet(StundenType.Kurzpause, 1, AnfangOderEnde.Ende), unterrichtVerspaetung
            ));

            FakeUnterrichten(new WoechentlicheZeitspannen(Wochentag.Freitag, StundenType.Fach, 2, unterrichtVerspaetung, unterrichtUeberziehung));

            RauchenGehenVorne(new WoechentlicheZeitspannen(
                Wochentag.Freitag,
                new SchuleEndeZeitRelativitaet(), unterrichtUeberziehung,
                new SchuleEndeZeitRelativitaet(), 0.5f
            ));
            ZumLehrerzimmerFachGehen(new WoechentlicheZeitspannen(
                Wochentag.Freitag,
                new SchuleEndeZeitRelativitaet(), 0.5f,
                new SchuleEndeZeitRelativitaet(), 1f
            ));
            E202ZigarettenHolen(new WoechentlicheZeitspannen(
                Wochentag.Freitag,
                new SchuleEndeZeitRelativitaet(), 1f,
                new SchuleEndeZeitRelativitaet(), 1.5f
            ));

            #endregion
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

            fragen.AddFrage(new InteraktionsMenuFrageEinfach(
                lehrer: Lehrer,
                player: Player,
                shouldShowPredicate: InteraktionsMenuFrageUtil.ShowNearby(),
                interaktionsMenuName: "Fragen, warum wir den Schimmelreiter lesen",
                reputationsAenderung: -0.1f,
                antworten: new SaetzeMoeglichkeitenEinmalig(
                    "Ich bitte dich! Es handelt sich um veraltete Weltliteratur, die unnötige Probleme thematisiert, bei denen Ich so tun kann, als hätten sie auch " +
                    "heutzutage noch irgendwie Relevanz in irgendeiner Form!"
                )
            ));
        }
    }
}
