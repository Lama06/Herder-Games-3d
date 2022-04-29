using HerderGames.Player;
using UnityEngine;

namespace HerderGames.Schule
{
    public class EntfernenVerbrechen : MonoBehaviour
    {
        [SerializeField] private string InteraktionsMenuName;
        [SerializeField] private int TimeRequired;
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
                        player.Score.SchadenFuerDieSchule += SchadenFuerDieSchule;
                        player.GeldManager.Geld += Gewinn;
                        gameObject.SetActive(false);
                    });
                    player.InteraktionsMenu.RemoveEintrag(id);
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