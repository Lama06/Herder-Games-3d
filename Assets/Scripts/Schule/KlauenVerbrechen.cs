using HerderGames.Player;
using UnityEngine;

namespace HerderGames.Schule
{
    public class KlauenVerbrechen : MonoBehaviour
    {
        [SerializeField] private string InteraktionsMenuName;
        [SerializeField] private float TimeRequired;
        [SerializeField] private float Schwere;
        [SerializeField] private int SchadenFuerDieSchule;
        [SerializeField] private int Gewinn;

        private int InteraktionsMenuId;

        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<Player.Player>();
            if (player == null)
            {
                return;
            }

            InteraktionsMenuId = player.InteraktionsMenu.AddEintrag(new InteraktionsMenuEintrag
            {
                Name = InteraktionsMenuName,
                Callback = id =>
                {
                    player.VerbrechenManager.VerbrechenStarten(TimeRequired, Schwere, () =>
                    {
                        gameObject.SetActive(false);
                    });
                }
            });
        }

        private void OnTriggerExit(Collider other)
        {
            var player = other.GetComponent<Player.Player>();
            if (player == null)
            {
                return;
            }
            
            player.InteraktionsMenu.RemoveEintrag(InteraktionsMenuId);
        }
    }
}