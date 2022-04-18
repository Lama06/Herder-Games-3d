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
            if (player == null || player.GetVerbrechenManager().BegehtGeradeEinVerbrechen)
            {
                return;
            }
            
            player.GetChat().SendChatMessage("Tipp: Du kannst bei dem Alarm Knopf einen Feueralarm auslösen." +
                                             "Die meisten Lehrer werden dann auf den Schulhof gehen");

            InteraktionsMenuId = player.GetInteraktionsMenu().AddEintrag(new InteraktionsMenuEintrag
            {
                Name = InteraktionsMenuName,
                Callback = id =>
                {
                    player.GetVerbrechenManager()
                        .VerbrechenStarten(ZeitZumDruecken, Schwere, () => { AlarmManager.AlarmStarten(); });
                    player.GetInteraktionsMenu().RemoveEintrag(id);
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

            player.GetInteraktionsMenu().RemoveEintrag(InteraktionsMenuId);
        }
    }
}