using System;
using System.Collections;
using HerderGames.Schule;
using HerderGames.Zeit;
using UnityEngine;

namespace HerderGames.Lehrer
{
    [RequireComponent(typeof(Lehrer))]
    public class VergiftungsManager : MonoBehaviour
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private Player.Player Player;
        [SerializeField] private int LaengeDerVergiftung;
        [SerializeField] private int KostenFuerDieSchuleProTagVergiftet;

        public LehrerVergiftungsStatus Status { get; private set; }
        private VergiftbaresEssen GrundDerVergiftung;
        private int TageRemaining;

        private void Start()
        {
            StartCoroutine(ManageVergiftung());
        }

        private IEnumerator ManageVergiftung()
        {
            var currentWochentag = TimeManager.GetCurrentWochentag();
            while (true)
            {
                yield return new WaitUntil(() => currentWochentag != TimeManager.GetCurrentWochentag());
                currentWochentag = TimeManager.GetCurrentWochentag();
                // Wird immer am Anfang eines neuen Wochentages ausgeführt

                if (!Status.IsVergiftet())
                {
                    continue;
                }

                if (TageRemaining <= 0)
                {
                    Status = LehrerVergiftungsStatus.NichtVergiftet;
                    GrundDerVergiftung = null;
                    TageRemaining = 0;
                    continue;
                }

                TageRemaining--;

                if (!Status.HasSyntome())
                {
                    Status = Status.MitSyntomen();
                    GrundDerVergiftung.Status = GrundDerVergiftung.Status.ToBemerkt();
                }

                Player.Score.SchadenFuerDieSchule += KostenFuerDieSchuleProTagVergiftet;
            }
        }

        public void Vergiften(VergiftbaresEssen grund)
        {
            Status = grund.Status switch
            {
                _ when grund.Status.IsVergiftetNormal() => LehrerVergiftungsStatus.VergiftetNormalKeineSyntome,
                _ when grund.Status.IsVergiftetOrthamol() => LehrerVergiftungsStatus.VergiftetOrthamolKeineSyntome,
                _ => throw new ArgumentOutOfRangeException()
            };
            GrundDerVergiftung = grund;
            TageRemaining = LaengeDerVergiftung;
        }
    }

    public enum LehrerVergiftungsStatus
    {
        NichtVergiftet,
        VergiftetNormalKeineSyntome,
        VergiftungNormalSyntome,
        VergiftetOrthamolKeineSyntome,
        VergiftungOrthamolSyntome
    }

    public static class LehrerVergiftungsStatusExtensions
    {
        public static bool IsVergiftet(this LehrerVergiftungsStatus status)
        {
            return status != LehrerVergiftungsStatus.NichtVergiftet;
        }

        public static bool HasSyntome(this LehrerVergiftungsStatus status)
        {
            return status is LehrerVergiftungsStatus.VergiftungNormalSyntome or LehrerVergiftungsStatus.VergiftungOrthamolSyntome;
        }
        
        public static LehrerVergiftungsStatus MitSyntomen(this LehrerVergiftungsStatus status)
        {
            return status switch
            {
                LehrerVergiftungsStatus.VergiftetNormalKeineSyntome => LehrerVergiftungsStatus.VergiftungNormalSyntome,
                LehrerVergiftungsStatus.VergiftetOrthamolKeineSyntome => LehrerVergiftungsStatus.VergiftungOrthamolSyntome,
                _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
            };
        }
    }
}
