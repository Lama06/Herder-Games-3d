using HerderGames.Player;
using UnityEngine;

namespace HerderGames.Schule
{
    public class VergiftbaresEssen : MonoBehaviour
    {
        [SerializeField] private Transform Standpunkt;
        [SerializeField] private InteraktionsMenu InteraktionsMenu;
        [SerializeField] private string InteraktionsMenuName;

        public VergiftungsStatus Status { get; set; }

        public Vector3 GetStandpunkt()
        {
            return Standpunkt.position;
        }

        private int InteraktionsmenuEintragId;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Player.Player>() == null)
            {
                return;
            }

            if (Status.IsVergiftet())
            {
                return;
            }

            InteraktionsmenuEintragId = InteraktionsMenu.AddEintrag(new InteractionsMenuEintrag
            {
                Name = InteraktionsMenuName,
                Callback = (id) =>
                {
                    Status = VergiftungsStatus.VergiftetNichtBemerkt;
                    InteraktionsMenu.RemoveEintrag(id);
                },
            });
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Player.Player>() == null)
            {
                return;
            }

            InteraktionsMenu.RemoveEintrag(InteraktionsmenuEintragId);
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