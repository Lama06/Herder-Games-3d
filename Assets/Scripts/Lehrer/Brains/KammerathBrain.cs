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

        private readonly SaetzeMoeglichkeitenMehrmals Unterricht = new(
            "Das ist ja zum Mäuse melken",
            "Ich hab vergessen diese geklaufte Folie auf Deutsch zu übersetz....Äähm.....Ich meine ich hab sie extra für euch auf Englisch übersetzt",
            "Ich...Äähm...Jemand hat gerade geraucht. Eine Nichtraucherin richt das sofort. Vor dem Unterricht Pfefferminz nehmen",
            "Ihr wollt wissen was Ssshreeadhs in Java sind? Einfach das Buch schenken lassen!",
            "Schade das die MLPD bei der letzten Wahl nicht gewonnen hat! Das iset ja zum Mäuse melken",
            "Wenn ihr fragen habt, fragt mich einfach. Vielleicht werde ich die Fragen beantworten, vielleicht werde ich aber auch sagen: Das musst du selber wissen",
            "Die DDR ist zusammengefallen? Das ist ja zum Mäuse melken!",
            "Hast du in deinem Leben schon mal Google benutzt?!",
            "Hey, Was machst du da auf Google?!",
            "Ach, Wie ich doch Windows XP vermisse!",
            "Ich empfehle nicht Informatik in der Oberstufe zu wählen! (weil ich es selber nicht kann)",
            "Ach sieh mal an, der Beamer spiegelt sich in der Plexiglas scheibe",
            "Wir haben eine superhohe Inzidenz in Köln und du trägst im Untericht keine Maske? (sagte sie, ohne eine Maske zu tragen)"
        );
        
        protected override void RegisterGoals(AIController ai)
        {
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
                reputationsAenderungBeiFehlzeit: -0.3f,
                saetzeWaehrendUnterricht: new SaetzeMoeglichkeitenMehrmals(
                    "Ich hab kein Internet! Das ist ja zum Mäuse melken",
                    "Wie soll ich denn jetzt Unterricht machen, wo ich gar nicht aus dem Internet mein Unterrichtsmaterial klauen kann",
                    "Meine komische Spionagesoftware funktioniert nicht"
                )    
            ));
            
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
                                new WoechentlicheZeitspannen.Zeitpunkt(new StundeZeitRelativitaet(StundenType.Fach, 1, AnfangOderEnde.Ende), 0f)
                            )
                        )
                    );

                    return zeit.IsInside(TimeManager) && Internet.IsInternetVerfuegbar();
                }),
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
        }
    }
}
