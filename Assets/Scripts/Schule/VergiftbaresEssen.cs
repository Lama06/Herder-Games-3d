using System;
using System.Collections;
using System.Collections.Generic;
using HerderGames.Player;
using UnityEngine;

namespace HerderGames.Schule
{
    public class VergiftbaresEssen : MonoBehaviour
    {
        [SerializeField] private Transform Standpunkt;
        [SerializeField] private string InteraktionsMenuNameNormal;
        [SerializeField] private string InteraktionsMenuNameOrthamol;
        [SerializeField] private int ZeitZumVergiften;
        [SerializeField] private float SchwereDesVerbrechens;
        [SerializeField] private int SchadenFuerDieSchule;

        public EssenVergiftungsStatus Status { get; set; }
        private Player.Player PlayerInTrigger;

        public Vector3 GetStandpunkt()
        {
            return Standpunkt.position;
        }

        private void Start()
        {
            StartCoroutine(ManageInterationsMenu());
        }

        private IEnumerator ManageInterationsMenu()
        {
            bool ShouldShow() => PlayerInTrigger != null && !Status.IsVergiftetNormal();

            void AddEintrag(string name, EssenVergiftungsStatus newStatus, List<int> ids)
            {
                ids.Add(PlayerInTrigger.InteraktionsMenu.AddEintrag(new InteraktionsMenuEintrag
                {
                    Name = name,
                    Callback = _ =>
                    {
                        PlayerInTrigger.VerbrechenManager.VerbrechenStarten(ZeitZumVergiften, SchwereDesVerbrechens, () => Status = newStatus);
                        PlayerInTrigger.Score.SchadenFuerDieSchule += SchadenFuerDieSchule;
                    }
                }));
            }

            while (true)
            {
                yield return new WaitUntil(ShouldShow);
                var player = PlayerInTrigger;
                var ids = new List<int>();
                AddEintrag(InteraktionsMenuNameNormal, EssenVergiftungsStatus.VergiftungNormalNichtBemerkt, ids);
                AddEintrag(InteraktionsMenuNameOrthamol, EssenVergiftungsStatus.VergiftungOrthamolNichtBemerkt, ids);

                yield return new WaitUntil(() => !ShouldShow());
                ids.ForEach(id => player.InteraktionsMenu.RemoveEintrag(id)); // Hier kann nicht PlayerInTrigger benutzt werden, weil er null ist
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Player.Player>(out var player))
            {
                PlayerInTrigger = player;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Player.Player>() != null)
            {
                PlayerInTrigger = null;
            }
        }
    }

    public enum EssenVergiftungsStatus
    {
        NichtVergiftet,
        VergiftungNormalNichtBemerkt,
        VergiftungOrthamolNichtBemerkt,
        VergiftetNormalBemerkt,
        VergiftetOrthamolBemerkt
    }

    public static class EssenVergiftungsStatusExtensions
    {
        public static bool IsBemerkt(this EssenVergiftungsStatus status)
        {
            return status is EssenVergiftungsStatus.VergiftetNormalBemerkt or EssenVergiftungsStatus.VergiftetOrthamolBemerkt;
        }

        public static bool IsVergiftet(this EssenVergiftungsStatus status)
        {
            return status.IsVergiftetNormal() || status.IsVergiftetOrthamol();
        }

        public static bool IsVergiftetNormal(this EssenVergiftungsStatus status)
        {
            return status is EssenVergiftungsStatus.VergiftungNormalNichtBemerkt or EssenVergiftungsStatus.VergiftetNormalBemerkt;
        }

        public static bool IsVergiftetOrthamol(this EssenVergiftungsStatus status)
        {
            return status is EssenVergiftungsStatus.VergiftungOrthamolNichtBemerkt or EssenVergiftungsStatus.VergiftetOrthamolBemerkt;
        }

        public static EssenVergiftungsStatus ToBemerkt(this EssenVergiftungsStatus status)
        {
            return status switch
            {
                EssenVergiftungsStatus.VergiftungNormalNichtBemerkt => EssenVergiftungsStatus.VergiftungNormalNichtBemerkt,
                EssenVergiftungsStatus.VergiftungOrthamolNichtBemerkt => EssenVergiftungsStatus.VergiftetOrthamolBemerkt,
                _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
            };
        }
    }
}
