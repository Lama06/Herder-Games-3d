using HerderGames.Player;
using UnityEngine;

namespace HerderGames.Schule
{
    public class VergiftbaresEssen : MonoBehaviour
    {
        [SerializeField] private Transform Standpunkt;
        [SerializeField] private string InteraktionsMenuName;

        public VergiftungsStatus Status { get; set; }

        public Vector3 GetStandpunkt()
        {
            return Standpunkt.position;
        }

        private int InteraktionsmenuEintragId;

        private void OnTriggerEnter(Collider other)
        {
            var chat = other.GetComponent<Chat>();
            if (chat != null)
            {
                chat.SendChatMessage("Tipp: Du bist in der Nähe eines Essens, das du vergiften kannst. Öffne dazu das Interaktionsmenu");
            }
            
            var interaktionsMenu = other.GetComponent<InteraktionsMenu>();
            if (interaktionsMenu == null)
            {
                return;
            }

            if (Status.IsVergiftet())
            {
                return;
            }

            InteraktionsmenuEintragId = interaktionsMenu.AddEintrag(new InteraktionsMenuEintrag
            {
                Name = InteraktionsMenuName,
                Callback = (id) =>
                {
                    Status = VergiftungsStatus.VergiftetNichtBemerkt;
                    interaktionsMenu.RemoveEintrag(id);
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