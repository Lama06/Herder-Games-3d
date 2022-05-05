using HerderGames.Lehrer.Sprache;
using HerderGames.Player;
using HerderGames.Util;

namespace HerderGames.Lehrer.Fragen
{
    public abstract class InteraktionsMenuFrageZufaellig : InteraktionsMenuFrage
    {
        protected readonly Lehrer Lehrer;
        protected readonly Player.Player Player;

        private readonly string InteraktionsMenuName;
        private readonly int Kosten;
        private readonly Item? RequiredItem;
        private readonly float AnnahmeWahrscheinlichkeit;

        private readonly float ReputationsAenderungBeiAnnahme;
        private readonly SaetzeMoeglichkeitenEinmalig AnnahmeAntworten;

        private readonly float ReputationsAenderungBeiAblehnen;
        private readonly SaetzeMoeglichkeitenEinmalig AblehnenAntworten;

        protected InteraktionsMenuFrageZufaellig(
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
        )
        {
            Lehrer = lehrer;
            Player = player;
            InteraktionsMenuName = interaktionsMenuName;
            Kosten = kosten;
            RequiredItem = requiredItem;
            AnnahmeWahrscheinlichkeit = annahmeWahrscheinlichkeit;
            ReputationsAenderungBeiAnnahme = reputationsAenderungBeiAnnahme;
            AnnahmeAntworten = annahmeAntworten;
            ReputationsAenderungBeiAblehnen = reputationsAenderungBeiAblehnen;
            AblehnenAntworten = ablehnenAntworten;
        }

        public override string GetText()
        {
            return InteraktionsMenuName;
        }

        protected virtual void OnAnnehmen()
        {
        }

        protected virtual void OnAblehnen()
        {
        }

        public override void OnClick(int id)
        {
            if (!Player.GeldManager.CanPay(Kosten))
            {
                Player.Chat.SendChatMessage($"Du benötigst {Kosten}€ um diese Aktion durchzuführen");
                return;
            }

            if (RequiredItem != null && !Player.Inventory.HasItem((Item) RequiredItem))
            {
                Player.Chat.SendChatMessage($"Du benötigst dieses Item um die Aktion durchzuführen: {RequiredItem}");
                return;
            }

            if (Utility.TrueWithPercent(AnnahmeWahrscheinlichkeit))
            {
                Player.GeldManager.Pay(Kosten);
                if (RequiredItem != null)
                {
                    Player.Inventory.RemoveItem((Item) RequiredItem);
                }
                
                Lehrer.Sprache.Say(AnnahmeAntworten);
                Lehrer.Reputation.AddReputation(ReputationsAenderungBeiAnnahme);
                OnAnnehmen();
            }
            else
            {
                Lehrer.Sprache.Say(AblehnenAntworten);
                Lehrer.Reputation.AddReputation(ReputationsAenderungBeiAblehnen);
                OnAblehnen();
            }
        }
    }
}
