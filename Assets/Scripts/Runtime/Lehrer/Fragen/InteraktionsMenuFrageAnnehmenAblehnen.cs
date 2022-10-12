using System;
using HerderGames.Lehrer.Sprache;
using HerderGames.Player;
using HerderGames.Util;

namespace HerderGames.Lehrer.Fragen
{
    public class InteraktionsMenuFrageAnnehmenAblehnen : InteraktionsMenuFrage
    {
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

            if (Utility.TrueWithPercent(AnnahmeWahrscheinlichkeit))
            {
                Lehrer.Player.GeldManager.Pay(Kosten);
                if (RequiredItem != null)
                {
                    Lehrer.Player.Inventory.RemoveItem((Item) RequiredItem);
                }

                Lehrer.Sprache.Say(AnnahmeAntworten);
                Lehrer.Reputation.AddReputation(ReputationsAenderungBeiAnnahme);
                OnAnnehmen?.Invoke(Lehrer.Player, Lehrer);
            }
            else
            {
                Lehrer.Sprache.Say(AblehnenAntworten);
                Lehrer.Reputation.AddReputation(ReputationsAenderungBeiAblehnen);
                OnAblehnen?.Invoke(Lehrer.Player, Lehrer);
            }
        }
    }
}
