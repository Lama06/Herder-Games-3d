using System.Collections;
using HerderGames.AI;
using HerderGames.Time;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    [RequireComponent(typeof(Lehrer))]
    public class SchuleBetretenVerlassenGoal : GoalBase
    {
        [SerializeField] private Transform Eingang;
        [SerializeField] private Transform Ausgang;
        [SerializeField] private WoechentlicheZeitspannen ZeitInDerSchule;
        [SerializeField] private bool InDerSchule = true;

        private Lehrer Lehrer;

        private void Awake()
        {
            Lehrer = GetComponent<Lehrer>();
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return !ShouldBeInSchool() || InDerSchule && !ShouldBeInSchool() || !InDerSchule && ShouldBeInSchool();
        }

        private bool ShouldBeInSchool()
        {
            return ZeitInDerSchule.IsInside(Lehrer.GetTimeManager().GetCurrentWochentag(),
                Lehrer.GetTimeManager().GetCurrentTime()) && !Lehrer.GetKrankheit().IsKrankMitSymtomen();
        }

        public override IEnumerator Execute()
        {
            while (true)
            {
                if (InDerSchule && !ShouldBeInSchool())
                {
                    Lehrer.GetAgent().destination = Ausgang.position;
                    yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.GetAgent());
                    SetVisible(false);
                    InDerSchule = false;
                }

                if (!InDerSchule && ShouldBeInSchool())
                {
                    Lehrer.GetAgent().Warp(Eingang.position);
                    SetVisible(true);
                    InDerSchule = true;
                }

                yield return null;
            }
        }

        private void SetVisible(bool visible)
        {
            Lehrer.GetRenderer().enabled = visible;
        }

        public bool IsInDerSchule()
        {
            return InDerSchule;
        }
    }
}