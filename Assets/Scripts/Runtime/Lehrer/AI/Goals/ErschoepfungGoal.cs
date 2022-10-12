using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Animation;
using HerderGames.Lehrer.Sprache;
using HerderGames.Util;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    public class ErschoepfungGoal : GoalBase
    {
        private readonly TriggerBase Trigger;
        private readonly float MaximaleDistanzProMinute;
        private readonly float MaxiamleHoeheProMinute;
        private readonly float LaengeDerPause;
        private readonly ISaetzeMoeglichkeitenMehrmals Saetze;
        private readonly AbstractAnimation Animation;

        private readonly List<float> DistanzLetzeMinute = new();
        private readonly List<float> HoeheLetzteMinute = new();

        public ErschoepfungGoal(
            TriggerBase trigger,
            float maximaleDistanzProMinute,
            float maxiamleHoeheProMinute,
            float laengeDerPause,
            ISaetzeMoeglichkeitenMehrmals saetze = null,
            AbstractAnimation animation = null
        )
        {
            Trigger = trigger;
            MaximaleDistanzProMinute = maximaleDistanzProMinute;
            MaxiamleHoeheProMinute = maxiamleHoeheProMinute;
            LaengeDerPause = laengeDerPause;
            Saetze = saetze;
            Animation = animation;
        }

        public override IEnumerable ExecuteGoal(Stack<Action> unexpectedGoalEndCallback)
        {
            if (!Trigger.ShouldRun)
            {
                yield break;
            }

            if (!(DistanzLetzeMinute.Sum() >= MaximaleDistanzProMinute || HoeheLetzteMinute.Sum() >= MaxiamleHoeheProMinute))
            {
                yield break;
            }

            yield return null;

            DistanzLetzeMinute.Clear();
            HoeheLetzteMinute.Clear();
            Lehrer.Sprache.SaetzeMoeglichkeiten = Saetze;
            Lehrer.AnimationManager.CurrentAnimation = Animation;

            foreach (var _ in IteratorUtil.WaitForSeconds(LaengeDerPause))
            {
                if (!Trigger.ShouldRun)
                {
                    yield break;
                }

                yield return null;
            }
        }

        public override IEnumerable UpdateGoal()
        {
            float GetYDistance(Vector3 pos1, Vector3 pos2)
            {
                return Mathf.Abs(pos1.y - pos2.y);
            }

            float GetXAndZDistance(Vector3 pos1, Vector3 pos2)
            {
                return Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.z - pos2.z);
            }

            void AddEintrag(IList<float> liste, float eintrag)
            {
                if (liste.Count >= 60)
                {
                    liste.RemoveAt(0);
                }

                liste.Add(eintrag);
            }

            var lastPosition = Lehrer.transform.position;

            yield return null;

            while (true)
            {
                var currentPosition = Lehrer.transform.position;

                var distance = GetXAndZDistance(lastPosition, currentPosition);
                AddEintrag(DistanzLetzeMinute, distance);

                var hoehe = GetYDistance(lastPosition, currentPosition);
                AddEintrag(HoeheLetzteMinute, hoehe);

                lastPosition = currentPosition;

                foreach (var _ in IteratorUtil.WaitForSeconds(1))
                {
                    yield return null;
                }
            }
        }
    }
}