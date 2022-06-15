using System.Collections;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Sprache;
using HerderGames.Schule;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    public class InternetReparierenGoal : GoalBase
    {
        private readonly TriggerBase Trigger;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeWeg;
        private readonly ISaetzeMoeglichkeitenEinmalig SaetzeAngekommenEinmalig;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeAngekommen;
        private readonly InternetManager InternetManager;
        private readonly VisionSensor Vision;

        private bool GehtGeradeHin;

        public InternetReparierenGoal(
            Lehrer lehrer,
            TriggerBase trigger,
            ISaetzeMoeglichkeitenMehrmals saetzeWeg,
            ISaetzeMoeglichkeitenEinmalig saetzeAngekommenEinmalig,
            ISaetzeMoeglichkeitenMehrmals saetzeAngekommen,
            InternetManager internet,
            VisionSensor vision
        ) : base(lehrer)
        {
            Trigger = trigger;
            SaetzeWeg = saetzeWeg;
            SaetzeAngekommenEinmalig = saetzeAngekommenEinmalig;
            SaetzeAngekommen = saetzeAngekommen;
            InternetManager = internet;
            Vision = vision;
        }

        private LanDose GetMicInVision()
        {
            foreach (var lanDose in InternetManager.LanDosen)
            {
                if (lanDose.Mic && Vision.CanSee(lanDose.gameObject))
                {
                    return lanDose;
                }
            }

            return null;
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            if (!Trigger.Resolve())
            {
                return false;
            }

            return currentlyRunning ? GehtGeradeHin : GetMicInVision() != null;
        }

        public override void OnGoalEnd()
        {
            GehtGeradeHin = false;
        }

        public override IEnumerator Execute()
        {
            var lanDose = GetMicInVision();

            GehtGeradeHin = true;
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeWeg;
            Lehrer.Agent.destination = lanDose.transform.position;
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            Lehrer.Sprache.Say(SaetzeAngekommenEinmalig);
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAngekommen;
            yield return new WaitForSeconds(3f);
            lanDose.Mic = false;
            GehtGeradeHin = false;
        }
    }
}
