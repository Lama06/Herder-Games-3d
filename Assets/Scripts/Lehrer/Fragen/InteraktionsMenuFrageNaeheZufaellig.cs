using HerderGames.Lehrer.Sprache;
using HerderGames.Player;
using UnityEngine;

namespace HerderGames.Lehrer.Fragen
{
    public class InteraktionsMenuFrageNaeheZufaellig : InteraktionsMenuFrageZufaellig
    {
        public InteraktionsMenuFrageNaeheZufaellig(
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
            return Vector3.Distance(Player.transform.position, Lehrer.transform.position) <= 10f;
        }
    }
}
