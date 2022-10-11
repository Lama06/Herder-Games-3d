using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HerderGames.Lehrer.AI
{
    [RequireComponent(typeof(Lehrer))]
    public class AIController : MonoBehaviour
    {
        public List<GoalBase> Goals { get; } = new();
        public GoalBase CurrentGoal => _CurrentGoalData?.Goal;

        private GoalData _CurrentGoalData;
        private readonly List<IEnumerator> GoalUpdateEnumerators = new();
        private Lehrer Lehrer;

        private GoalData CurrentGoalData
        {
            get => _CurrentGoalData;
            set
            {
                if (_CurrentGoalData != null)
                {
                    foreach (var action in _CurrentGoalData.EndedCallback)
                    {
                        action();
                    }
                }

                _CurrentGoalData = value;
            }
        }

        public void AddGoal(GoalBase goal)
        {
            Goals.Add(goal);
            GoalUpdateEnumerators.Add(goal.UpdateGoal().GetEnumerator());
        }

        private void Awake()
        {
            Lehrer = GetComponent<Lehrer>();
        }

        private void Start()
        {
            Lehrer.Agent.enabled = false;
        }

        private void Update()
        {
            UpdateGoals();

            var moreImportantGoal = FindGoalWithHigherPriorityThatCanStart();
            if (moreImportantGoal != null)
            {
                CurrentGoalData = moreImportantGoal;
            }

            if (CurrentGoalData != null)
            {
                var success = CurrentGoalData.Enumerator.MoveNext();
                if (!success)
                {
                    CurrentGoalData = null;
                }
                else
                {
                    if (!CurrentGoalData.Enumerator.Current.ShouldContinue())
                    {
                        CurrentGoalData = null;
                    }
                }
            }
        }

        private void UpdateGoals()
        {
            for (var i = GoalUpdateEnumerators.Count - 1; i >= 0; i--)
            {
                var goalEnumerator = GoalUpdateEnumerators[i];
                var success = goalEnumerator.MoveNext();
                if (!success)
                {
                    GoalUpdateEnumerators.RemoveAt(i);
                }
            }
        }

        private GoalData FindGoalWithHigherPriorityThatCanStart()
        {
            foreach (var goal in Goals)
            {
                if (goal == CurrentGoalData?.Goal)
                {
                    return null;
                }

                var goalEndCallback = new List<Action>()
                {
                    () =>
                    {
                        Lehrer.AnimationManager.CurrentAnimation = null;
                        Lehrer.Sprache.SaetzeMoeglichkeiten = null;
                    }
                };
                var goalEnumerator = goal.ExecuteGoal(goalEndCallback).GetEnumerator();
                goalEnumerator.MoveNext();
                if (((GoalStatus.CanStartIf) goalEnumerator.Current).Value)
                {
                    return new GoalData
                    {
                        Goal = goal,
                        Enumerator = goalEnumerator,
                        EndedCallback = goalEndCallback
                    };
                }
            }

            return null;
        }

        private class GoalData
        {
            public GoalBase Goal;
            public IEnumerator<GoalStatus> Enumerator;
            public List<Action> EndedCallback;
        }
    }
}