using HerderGames.Player;
using UnityEngine;

namespace HerderGames.Schule
{
    public class FeueralarmKnopf : MonoBehaviour
    {
        [SerializeField] private AlarmManager AlarmManager;
        [SerializeField] private string InteraktionsMenuName;
        [SerializeField] private float ZeitZumDruecken;
        [SerializeField] private float Schwere;

        private int InteraktionsMenuId;

        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<Player.Player>();
            if (player == null || player.VerbrechenManager.BegehtGeradeEinVerbrechen)
            {
                return;
            }
            
            player.Chat.SendChatMessage("Tipp: Du kannst bei dem Alarm Knopf einen Feueralarm auslösen." +
                                        "Die meisten Lehrer werden dann auf den Schulhof gehen");

            InteraktionsMenuId = player.InteraktionsMenu.AddEintrag(new InteraktionsMenuEintrag
            {
                Name = InteraktionsMenuName,
                Callback = id =>
                {
                    player.VerbrechenManager
                        .VerbrechenStarten(ZeitZumDruecken, Schwere, () => { AlarmManager.AlarmStarten(); });
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