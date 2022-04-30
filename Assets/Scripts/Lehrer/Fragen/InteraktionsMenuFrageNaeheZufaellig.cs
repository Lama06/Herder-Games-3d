using HerderGames.Lehrer.Sprache;
using UnityEngine;

namespace HerderGames.Lehrer.Fragen
{
    public class InteraktionsMenuFrageNaeheZufaellig : InteraktionsMenuFrageZufaellig
    {
        public InteraktionsMenuFrageNaeheZufaellig(
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
            return Vector3.Distance(Player.transform.position, Lehrer.transform.position) <= 10f;
        }
    }
}
