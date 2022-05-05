using HerderGames.Lehrer.AI.Goals;
using HerderGames.Lehrer.Sprache;
using HerderGames.Player;

namespace HerderGames.Lehrer.Fragen
{
    public class InteraktionsMenuFrageSchwaenzenZufaellig : InteraktionsMenuFrageZufaellig
    {
        public InteraktionsMenuFrageSchwaenzenZufaellig(
            Lehrer lehrer,
            Player.Player player,
            string interaktionsMenuName,
            float annahmeWahrscheinlichkeit,
            float reputationsAenderungBeiAnnahme,
            SaetzeMoeglichkeitenEinmalig annahmeAntworten,
            float reputationsAenderungBeiAblehnen,
            SaetzeMoeglichkeitenEinmalig ablehnenAntworten,
            int kosten = 0,
            Item? requiredItem = null
        ) : base(
            lehrer,
            player,
            interaktionsMenuName,
            annahmeWahrscheinlichkeit,
            reputationsAenderungBeiAnnahme,
            annahmeAntworten,
            reputationsAenderungBeiAblehnen,
            ablehnenAntworten,
            kosten,
            requiredItem
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
