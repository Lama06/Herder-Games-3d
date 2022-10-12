using System;
using HerderGames.Lehrer.Sprache;
using HerderGames.Player;

namespace HerderGames.Lehrer.Fragen
{
    public class InteraktionsMenuFrageEinfach : InteraktionsMenuFrage
    {
        private readonly Func<Player.Player, Lehrer, bool> ShouldShowPredicate;
        private readonly int Kosten;
        private readonly Item? RequiredItem;
        private readonly float ReputationsAenderung;
        private readonly ISaetzeMoeglichkeitenEinmalig Antworten;
        private readonly Action<Player.Player, Lehrer> ClickCallback;

        public InteraktionsMenuFrageEinfach(
            Func<Player.Player, Lehrer, bool> shouldShowPredicate,
            string interaktionsMenuName,
            int kosten = 0,
            Item? requiredItem = null,
            float reputationsAenderung = 0f,
            ISaetzeMoeglichkeitenEinmalig antworten = null,
            Action<Player.Player, Lehrer> clickCallback = null
        )
        {
            ShouldShowPredicate = shouldShowPredicate;
            Text = interaktionsMenuName;
            Kosten = kosten;
            RequiredItem = requiredItem;
            ReputationsAenderung = reputationsAenderung;
            Antworten = antworten;
            ClickCallback = clickCallback;
        }

        public override bool ShouldShow => ShouldShowPredicate(Lehrer.Player, Lehrer);

        public override string Text { get; }

        public override void OnClick()
        {
            if (!Lehrer.Player.GeldManager.CanPay(Kosten))
            {
                Lehrer.Player.Chat.SendChatMessage($"Du benötigst {Kosten}€ um diese Aktion durchzuführen");
                return;
            }

            if (RequiredItem != null && !Lehrer.Player.Inventory.HasItem((Item) RequiredItem))
            {
                Lehrer.Player.Chat.SendChatMessage($"Du benötigst dieses Item um die Aktion durchzuführen: {RequiredItem}");
                return;
            }
            
            Lehrer.Player.GeldManager.Pay(Kosten);
            if (RequiredItem != null)
            {
                Lehrer.Player.Inventory.RemoveItem((Item) RequiredItem);
            }

            Lehrer.Sprache.Say(Antworten);
            Lehrer.Reputation.AddReputation(ReputationsAenderung);
            ClickCallback?.Invoke(Lehrer.Player, Lehrer);
        }
    }
}
