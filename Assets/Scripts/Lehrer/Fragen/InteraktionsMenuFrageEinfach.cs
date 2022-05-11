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
        private readonly string InteraktionsMenuName;
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
            InteraktionsMenuName = interaktionsMenuName;
            Kosten = kosten;
            RequiredItem = requiredItem;
            ReputationsAenderung = reputationsAenderung;
            Antworten = antworten;
            ClickCallback = clickCallback;
        }

        public override bool ShouldShow()
        {
            return ShouldShowPredicate(Player, Lehrer);
        }

        public override string GetText()
        {
            return InteraktionsMenuName;
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
            
            Player.GeldManager.Pay(Kosten);
            if (RequiredItem != null)
            {
                Player.Inventory.RemoveItem((Item) RequiredItem);
            }

            Lehrer.Sprache.Say(Antworten);
            Lehrer.Reputation.AddReputation(ReputationsAenderung);
            if (ClickCallback != null)
            {
                ClickCallback(Player, Lehrer);
            }
        }
    }
}
