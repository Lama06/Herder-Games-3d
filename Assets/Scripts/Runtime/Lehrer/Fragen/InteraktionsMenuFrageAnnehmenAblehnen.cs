using System;
using HerderGames.Lehrer.Sprache;
using HerderGames.Player;
using HerderGames.Util;

namespace HerderGames.Lehrer.Fragen
{
    public class InteraktionsMenuFrageAnnehmenAblehnen : InteraktionsMenuFrage
    {
        private readonly Lehrer Lehrer;
        private readonly Player.Player Player;

        private readonly Func<Player.Player, Lehrer, bool> ShouldShowPredicate;
        private readonly int Kosten;
        private readonly Item? RequiredItem;
        private readonly float AnnahmeWahrscheinlichkeit;

        private readonly float ReputationsAenderungBeiAnnahme;
        private readonly ISaetzeMoeglichkeitenEinmalig AnnahmeAntworten;
        private readonly Action<Player.Player, Lehrer> OnAnnehmen;

        private readonly float ReputationsAenderungBeiAblehnen;
        private readonly ISaetzeMoeglichkeitenEinmalig AblehnenAntworten;
        private readonly Action<Player.Player, Lehrer> OnAblehnen;

        public InteraktionsMenuFrageAnnehmenAblehnen(
            Lehrer lehrer,
            Player.Player player,
            Func<Player.Player, Lehrer, bool> shouldShowPredicate,
            string interaktionsMenuName,
            float annahmeWahrscheinlichkeit,
            float reputationsAenderungBeiAnnahme = 0f,
            ISaetzeMoeglichkeitenEinmalig annahmeAntworten = null,
            Action<Player.Player, Lehrer> onAnnehmen = null,
            float reputationsAenderungBeiAblehnen = 0f,
            ISaetzeMoeglichkeitenEinmalig ablehnenAntworten = null,
            Action<Player.Player, Lehrer> onAblehnen = null,
            int kosten = 0,
            Item? requiredItem = null
        )
        {
            Lehrer = lehrer;
            Player = player;
            ShouldShowPredicate = shouldShowPredicate;
            Text = interaktionsMenuName;
            Kosten = kosten;
            RequiredItem = requiredItem;
            AnnahmeWahrscheinlichkeit = annahmeWahrscheinlichkeit;
            ReputationsAenderungBeiAnnahme = reputationsAenderungBeiAnnahme;
            AnnahmeAntworten = annahmeAntworten;
            OnAnnehmen = onAnnehmen;
            ReputationsAenderungBeiAblehnen = reputationsAenderungBeiAblehnen;
            AblehnenAntworten = ablehnenAntworten;
            OnAblehnen = onAblehnen;
        }

        public override bool ShouldShow => ShouldShowPredicate(Player, Lehrer);

        public override string Text { get; }

        public override void OnClick()
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
                OnAnnehmen?.Invoke(Player, Lehrer);
            }
            else
            {
                Lehrer.Sprache.Say(AblehnenAntworten);
                Lehrer.Reputation.AddReputation(ReputationsAenderungBeiAblehnen);
                OnAblehnen?.Invoke(Player, Lehrer);
            }
        }
    }
}
