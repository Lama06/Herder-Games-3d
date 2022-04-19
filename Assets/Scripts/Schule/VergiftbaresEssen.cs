using HerderGames.Player;
using UnityEngine;

namespace HerderGames.Schule
{
    public class VergiftbaresEssen : MonoBehaviour
    {
        [SerializeField] private Transform Standpunkt;
        [SerializeField] private string InteraktionsMenuName;
        [SerializeField] private float ZeitZumVergiften;
        [SerializeField] private float SchwereDesVerbrechens;

        public VergiftungsStatus Status { get; set; }
        private int InteraktionsmenuEintragId;

        public Vector3 GetStandpunkt()
        {
            return Standpunkt.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<Player.Player>();
            if (player == null)
            {
                return;
            }

            if (Status.IsVergiftet() || player.VerbrechenManager.BegehtGeradeEinVerbrechen)
            {
                return;
            }

            player.Chat
                .SendChatMessage(
                    "Tipp: Du bist in der Nähe eines Essens, das du vergiften kannst. " +
                    "Lehrer, die davon regelmäßig essen, werden dann für ein paar Tage nicht mehr in die Schule kommen. " +
                    "Öffne dazu das Interaktionsmenu");

            InteraktionsmenuEintragId = player.InteraktionsMenu.AddEintrag(new InteraktionsMenuEintrag
            {
                Name = InteraktionsMenuName,
                Callback = id =>
                {
                    player.VerbrechenManager.VerbrechenStarten(ZeitZumVergiften, SchwereDesVerbrechens,
                        () => { Status = VergiftungsStatus.VergiftetNichtBemerkt; });
                    player.InteraktionsMenu.RemoveEintrag(id);
                },
            });
        }

        private void OnTriggerExit(Collider other)
        {
            var interaktionsMenu = other.GetComponent<InteraktionsMenu>();
            if (interaktionsMenu == null)
            {
                return;
            }

            interaktionsMenu.RemoveEintrag(InteraktionsmenuEintragId);
        }
    }

    public enum VergiftungsStatus
    {
        NichtVergiftet,
        VergiftetNichtBemerkt,
        VergiftetBemerkt
    }

    public static class VergiftungsStatusExtensions
    {
        public static bool IsVergiftet(this VergiftungsStatus status)
        {
            return status is VergiftungsStatus.VergiftetBemerkt or VergiftungsStatus.VergiftetNichtBemerkt;
        }
    }
}