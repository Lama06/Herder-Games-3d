using System.Collections;
using HerderGames.Lehrer.Sprache;
using HerderGames.Player;
using HerderGames.Util;
using UnityEngine;

namespace HerderGames.Lehrer
{
    [RequireComponent(typeof(Lehrer))]
    public abstract class InteraktionsMenuFrage : MonoBehaviour
    {
        [SerializeField] private Player.Player Player;
        [SerializeField] private string InteraktionsMenuName;
        [SerializeField] private int Kosten;
        [SerializeField] private float AnnahmeWahrscheinlichkeit;

        [Header("Annahme")] [SerializeField] private float ReputationsAenderungBeiAnnahme;
        [SerializeField] private SaetzeMoeglichkeitenEinmalig AnnahmeAntworten;

        [Header("Ablehnen")] [SerializeField] private float ReputationsAenderungBeiAblehnen;
        [SerializeField] private SaetzeMoeglichkeitenEinmalig AblehnenAntworten;

        protected Lehrer Lehrer;

        protected abstract bool ShouldShowInInteraktionsMenu();

        protected virtual void OnAnnahme()
        {
        }

        private void Awake()
        {
            Lehrer = GetComponent<Lehrer>();
        }

        private void Start()
        {
            StartCoroutine(ManageInteraktionsMenu());
        }

        private IEnumerator ManageInteraktionsMenu()
        {
            var interaktionsMenuId = 0;
            var hasEintrag = false;

            while (true)
            {
                if (ShouldShowInInteraktionsMenu() && !hasEintrag)
                {
                    interaktionsMenuId = Player.InteraktionsMenu.AddEintrag(new InteraktionsMenuEintrag
                    {
                        Name = InteraktionsMenuName,
                        Callback = id =>
                        {
                            if (!Player.GeldManager.Pay(Kosten))
                            {
                                Player.Chat.SendChatMessage($"Du benötigst {Kosten}€ um diese Aktion durchzuführen");
                                return;
                            }
                            
                            if (Utility.TrueWithPercent(AnnahmeWahrscheinlichkeit))
                            {
                                OnAnnahme();
                                Lehrer.Sprache.Say(AnnahmeAntworten);
                                Lehrer.Reputation.AddReputation(ReputationsAenderungBeiAnnahme);
                            }
                            else
                            {
                                Lehrer.Sprache.Say(AblehnenAntworten);
                                Lehrer.Reputation.AddReputation(ReputationsAenderungBeiAblehnen);
                            }
                        }
                    });

                    hasEintrag = true;
                }

                if (!ShouldShowInInteraktionsMenu() && hasEintrag)
                {
                    Player.InteraktionsMenu.RemoveEintrag(interaktionsMenuId);
                    hasEintrag = false;
                }

                yield return null;
            }
        }
    }
}