using System.Collections;
using HerderGames.AI;
using HerderGames.Lehrer.Sprache;
using HerderGames.Schule;
using HerderGames.Time;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    public class VergiftbaresEssenEssenGehen : LehrerGoalBase
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private VergiftbaresEssen VergiftbaresEssen;
        [SerializeField] private WoechentlicheZeitspannen Wann;
        [SerializeField] private Saetze SaetzeWeg;
        [SerializeField] private Saetze SaetzeAngekommen;

        public override bool ShouldRun(bool currentlyRunning)
        {
            return VergiftbaresEssen.Status != VergiftungsStatus.VergiftetBemerkt && Wann.IsInside(TimeManager.GetCurrentWochentag(), TimeManager.GetCurrentTime());
        }

        public override IEnumerator Execute()
        {
            Lehrer.Sprache.SetSatzSource(SaetzeWeg);
            Lehrer.Agent.destination = VergiftbaresEssen.GetStandpunkt();
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            if (VergiftbaresEssen.Status.IsVergiftet())
            {
                Lehrer.Krankheit.Erkranken(VergiftbaresEssen);
            }
            Lehrer.Sprache.SetSatzSource(SaetzeAngekommen);
        }
    }
}
