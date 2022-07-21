using System;
using HerderGames.Lehrer.Sprache;
using HerderGames.Player;

namespace HerderGames.Lehrer.Fragen
{
    public class InteraktionsMenuFrageEinfach : InteraktionsMenuFrage
    {
        private readonly Lehrer Lehrer;
        private readonly Player.Player Player;

        private readonly Func<Player.Player, Lehrer, bool> ShouldShowPredicate;
        private readonly int Kosten;
        private readonly Item? RequiredItem;
        private readonly float ReputationsAenderung;
        private readonly ISaetzeMoeglichkeitenEinmalig Antworten;
        private readonly Action<Player.Player, Lehrer> ClickCallback;

        public InteraktionsMenuFrageEinfach(
            Lehrer lehrer,
            Player.Player player,
            Func<Player.Player, Lehrer, bool> shouldShowPredicate,
            string interaktionsMenuName,
            int kosten = 0,
            Item? requiredItem = null,
            float reputationsAenderung = 0f,
            ISaetzeMoeglichkeitenEinmalig antworten = null,
            Action<Player.Player, Lehrer> clickCallback = null
        )
        {
            Lehrer = lehrer;
            Player = player;
            ShouldShowPredicate = shouldShowPredicate;
            Text = interaktionsMenuName;
            Kosten = kosten;
            RequiredItem = requiredItem;
            ReputationsAenderung = reputationsAenderung;
            Antworten = antworten;
            ClickCallback = clickCallback;
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
            
            Player.GeldManager.Pay(Kosten);
            if (RequiredItem != null)
            {
                Player.Inventory.RemoveItem((Item) RequiredItem);
            }

            Lehrer.Sprache.Say(Antworten);
            Lehrer.Reputation.AddReputation(ReputationsAenderung);
            ClickCallback?.Invoke(Player, Lehrer);
        }
    }
}
