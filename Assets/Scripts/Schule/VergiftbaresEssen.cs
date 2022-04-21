using HerderGames.Player;
using UnityEngine;

namespace HerderGames.Schule
{
    public class VergiftbaresEssen : MonoBehaviour
    {
        [SerializeField] private Transform Standpunkt;
        [SerializeField] private string InteraktionsMenuName;
        [SerializeField] private int ZeitZumVergiften;
        [SerializeField] private float SchwereDesVerbrechens;
        [SerializeField] private int SchadenFuerDieSchule;

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

            if (Status.IsVergiftet())
            {
                return;
            }

            InteraktionsmenuEintragId = player.InteraktionsMenu.AddEintrag(new InteraktionsMenuEintrag
            {
                Name = InteraktionsMenuName,
                Callback = id =>
                {
                    player.VerbrechenManager.VerbrechenStarten(ZeitZumVergiften, SchwereDesVerbrechens,
                        () =>
                        {
                            Status = VergiftungsStatus.VergiftetNichtBemerkt;
                            player.Score.SchadenFuerDieSchule += SchadenFuerDieSchule;
                        });
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