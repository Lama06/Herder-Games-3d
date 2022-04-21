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

        private VergiftungsManager Vergiftung;

        protected override void Awake()
        {
            base.Awake();
            Vergiftung = GetComponent<VergiftungsManager>();
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            // Das Goal läuft immer, wenn der Lehrer nicht in der Schule sein sollte,
            // damit in dieser Zeit keine anderen Goals laufen können
            return !ShouldBeInSchool() || InDerSchule && !ShouldBeInSchool() || !InDerSchule && ShouldBeInSchool();
        }

        private bool ShouldBeInSchool()
        {
            return ZeitInDerSchule.IsInside(TimeManager.GetCurrentWochentag(), TimeManager.GetCurrentTime()) &&
                   (Vergiftung == null || !Vergiftung.Syntome);
        }

        public override IEnumerator Execute()
        {
            while (true)
            {
                if (InDerSchule && !ShouldBeInSchool())
                {
                    Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeBeimVerlassen;
                    Lehrer.Agent.destination = Ausgang.position;
                    yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
                    SetInSchule(false);

                    // Damit der Lehrer nicht redet, während er unsichtbar ist
                    Lehrer.Sprache.SaetzeMoeglichkeiten = null;
                }

                if (!InDerSchule && ShouldBeInSchool())
                {
                    Lehrer.Agent.Warp(Eingang.position);
                    SetInSchule(true);
                }

                yield return null;
            }
        }

        private void SetInSchule(bool inSchule)
        {
            InDerSchule = inSchule;

            Lehrer.Renderer.enabled = inSchule;
            // Der NavMeshAgent Component muss deaktiviert werden, weil sonst andere Lehrer nicht zum Ausgang navigieren können,
            // wenn dort bereits ein anderer Lehrer steht, weil die beiden NavMeshAgents nicht kollidiren wollen (obwohl
            // sie unsichtbar sind)
            Lehrer.Agent.enabled = inSchule;
        }

        public bool IsInDerSchule()
        {
            return InDerSchule;
        }
    }
}