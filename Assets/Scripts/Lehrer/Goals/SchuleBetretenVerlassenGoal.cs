using System.Collections;
using HerderGames.AI;
using HerderGames.Lehrer.Sprache;
using HerderGames.Time;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    public class SchuleBetretenVerlassenGoal : LehrerGoalBase
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private Transform Eingang;
        [SerializeField] private Transform Ausgang;
        [SerializeField] private WoechentlicheZeitspannen ZeitInDerSchule;
        [SerializeField] private bool InDerSchule = true;
        [SerializeField] private SaetzeMoeglichkeitenMehrmals SaetzeBeimVerlassen;

        public override bool ShouldRun(bool currentlyRunning)
        {
            return !ShouldBeInSchool() || InDerSchule && !ShouldBeInSchool() || !InDerSchule && ShouldBeInSchool();
        }

        private bool ShouldBeInSchool()
        {
            return ZeitInDerSchule.IsInside(TimeManager.GetCurrentWochentag(), TimeManager.GetCurrentTime()) &&
                   !Lehrer.Krankheit.IsKrankMitSymtomen();
        }

        public override IEnumerator Execute()
        {
            while (true)
            {
                if (InDerSchule && !ShouldBeInSchool())
                {
                    Lehrer.Sprache.SetSatzSource(SaetzeBeimVerlassen);
                    Lehrer.Agent.destination = Ausgang.position;
                    yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
                    SetVisible(false);
                    InDerSchule = false;
                    Lehrer.Sprache.SetSatzSource(null);
                }

                if (!InDerSchule && ShouldBeInSchool())
                {
                    Lehrer.Agent.Warp(Eingang.position);
                    SetVisible(true);
                    InDerSchule = true;
                    Lehrer.Sprache.SetSatzSource(null);
                }

                yield return null;
            }
        }

        private void SetVisible(bool visible)
        {
            Lehrer.Renderer.enabled = visible;
        }

        public bool IsInDerSchule()
        {
            return InDerSchule;
        }
    }
}