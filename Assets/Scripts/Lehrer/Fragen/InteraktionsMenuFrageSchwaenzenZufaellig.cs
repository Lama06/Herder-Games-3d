using HerderGames.Lehrer.AI.Goals;
using HerderGames.Lehrer.Sprache;

namespace HerderGames.Lehrer.Fragen
{
    public class InteraktionsMenuFrageSchwaenzenZufaellig : InteraktionsMenuFrageZufaellig
    {
        public InteraktionsMenuFrageSchwaenzenZufaellig(
            Lehrer lehrer,
            Player.Player player,
            string interaktionsMenuName,
            int kosten,
            float annahmeWahrscheinlichkeit,
            float reputationsAenderungBeiAnnahme,
            SaetzeMoeglichkeitenEinmalig annahmeAntworten,
            float reputationsAenderungBeiAblehnen,
            SaetzeMoeglichkeitenEinmalig ablehnenAntworten
        ) : base(
            lehrer,
            player,
            interaktionsMenuName,
            kosten,
            annahmeWahrscheinlichkeit,
            reputationsAenderungBeiAnnahme,
            annahmeAntworten,
            reputationsAenderungBeiAblehnen,
            ablehnenAntworten
        )
        {
        }

        public override bool ShouldShow()
        {
            return Lehrer.AI.CurrentGoal is UnterrichtenGoal {LehrerArrived: true, SchuelerFreigestelltDieseStunde: false} unterrichten && unterrichten.GetKlassenraum().PlayerInside;
        }

        protected override void OnAnnehmen()
        {
            if (Lehrer.AI.CurrentGoal is not UnterrichtenGoal unterrichten)
            {
                return;
            }

            unterrichten.SchuelerFreigestelltDieseStunde = true;
        }
    }
}
