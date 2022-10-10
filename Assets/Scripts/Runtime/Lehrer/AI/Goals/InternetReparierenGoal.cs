using System.Collections;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Animation;
using HerderGames.Lehrer.Sprache;
using HerderGames.Schule;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    public class InternetReparierenGoal : GoalBase
    {
        private readonly TriggerBase Trigger;
        private readonly InternetManager InternetManager;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeWeg;
        private readonly ISaetzeMoeglichkeitenEinmalig SaetzeAngekommenEinmalig;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeAngekommen;
        private readonly AbstractAnimation AnimationWeg;
        private readonly AbstractAnimation AnimationAngekommen;

        private bool Fertig;
        private LanDose LanDose;

        public InternetReparierenGoal(
            Lehrer lehrer,
            TriggerBase trigger,
            InternetManager internet,
            ISaetzeMoeglichkeitenMehrmals saetzeWeg = null,
            ISaetzeMoeglichkeitenEinmalig saetzeAngekommenEinmalig = null,
            ISaetzeMoeglichkeitenMehrmals saetzeAngekommen = null,
            AbstractAnimation animationWeg = null,
            AbstractAnimation animationAngekommen = null
        ) : base(lehrer)
        {
            Trigger = trigger;
            InternetManager = internet;
            SaetzeWeg = saetzeWeg;
            SaetzeAngekommenEinmalig = saetzeAngekommenEinmalig;
            SaetzeAngekommen = saetzeAngekommen;
            AnimationWeg = animationWeg;
            AnimationAngekommen = animationAngekommen;
        }

        private LanDose MicInVision
        {
            get
            {
                foreach (var lanDose in InternetManager.LanDosen)
                {
                    if (lanDose.Mic && Lehrer.Vision.CanSee(lanDose.gameObject))
                    {
                        return lanDose;
                    }
                }

                return null;
            }
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            if (!Trigger.ShouldRun)
            {
                return false;
            }

            if (currentlyRunning)
            {
                return !Fertig;
            }

            LanDose = MicInVision;
            return LanDose != null;
        }

        protected override IEnumerator Execute()
        {
            Fertig = false;
            Lehrer.AnimationManager.CurrentAnimation = AnimationWeg;
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeWeg;
            Lehrer.Agent.destination = LanDose.transform.position;
            yield return NavMeshUtil.Pathfind(Lehrer.Agent);
            Lehrer.AnimationManager.CurrentAnimation = AnimationAngekommen;
            Lehrer.Sprache.Say(SaetzeAngekommenEinmalig);
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAngekommen;
            LanDose.Mic = false;
            Fertig = true;
        }
    }
}
