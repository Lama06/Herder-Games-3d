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
        public GoalBase CurrentGoal => CurrentGoalData?.Goal;

        private GoalData CurrentGoalData;
        private readonly List<IEnumerator> GoalUpdateEnumerators = new();
        private Lehrer Lehrer;

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
                if (CurrentGoalData != null)
                {
                    foreach (var action in CurrentGoalData.UnexpectedGoalEndCallback)
                    {
                        action();
                    }
                }
                
                CurrentGoalData = moreImportantGoal;
            }

            if (CurrentGoalData != null)
            {
                var success = CurrentGoalData.Enumerator.MoveNext();
                if (!success)
                {
                    // Hier nicht UnexpectedGoalEndCallback ausführen, weil das Goal ja selber benndet wurde
                    CurrentGoalData = null;
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
                if (CurrentGoalData != null && goal == CurrentGoalData.Goal)
                {
                    return null;
                }

                var unexpectedGoalEndCallback = new Stack<Action>();
                var goalEnumerator = goal.ExecuteGoal(unexpectedGoalEndCallback).GetEnumerator();
                var success = goalEnumerator.MoveNext();
                if (success)
                {
                    return new GoalData
                    {
                        Goal = goal,
                        Enumerator = goalEnumerator,
                        UnexpectedGoalEndCallback = unexpectedGoalEndCallback
                    };
                }
            }

            return null;
        }

        private class GoalData
        {
            public GoalBase Goal;
            public IEnumerator Enumerator;
            public Stack<Action> UnexpectedGoalEndCallback;
        }
    }
}