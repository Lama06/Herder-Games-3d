using System.Collections;
using HerderGames.Lehrer.Sprache;
using HerderGames.Player;
using HerderGames.Util;
using UnityEngine;

namespace HerderGames.Lehrer
{
    [RequireComponent(typeof(Lehrer))]
    public abstract class ZufaelligeInteraktionsMenuAktion : MonoBehaviour
    {
        [SerializeField] private Player.Player Player;
        [SerializeField] private string InteraktionsMenuName;
        [SerializeField] private float AnnahmeWahrscheinlichkeit;
        [SerializeField] private float ReputationsGewinn;
        [SerializeField] private SaetzeMoeglichkeitenEinmalig AnnahmeAntworten;
        [SerializeField] private float ReputationsVerlust;
        [SerializeField] private SaetzeMoeglichkeitenEinmalig AblehnenAntworten;

        protected Lehrer Lehrer;

        protected abstract bool ShouldShowInInteraktionsMenu();

        protected virtual void OnSucces()
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
                            if (Utility.TrueWithPercent(AnnahmeWahrscheinlichkeit))
                            {
                                OnSucces();
                                Lehrer.Sprache.SayRandomNow(AnnahmeAntworten);
                                Lehrer.Reputation.AddReputation(ReputationsGewinn);
                            }
                            else
                            {
                                Lehrer.Sprache.SayRandomNow(AblehnenAntworten);
                                Lehrer.Reputation.AddReputation(ReputationsVerlust);
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