using System.Collections.Generic;
using System.Text;
using HerderGames.Lehrer.AI.Goals;
using HerderGames.Zeit;
using UnityEngine;

namespace HerderGames.Player
{
    /*
     * Stundenplan:
     *
     * Montag:
     *
     * - Schulten
     * - Kammerath
     * - Schwehmer
     * -
     *
     * Dienstag:
     *
     * -
     * -
     * -
     * 
     * Mittwoch:
     * 
     * -
     * -
     * -
     * 
     * Donnerstag:
     * 
     * -
     * -
     * -
     * 
     * Freitag:
     *
     * -
     * -
     * -
     * 
     */
    
    [RequireComponent(typeof(Player))]
    public class Stundenplan : MonoBehaviour
    {
        [SerializeField] private TimeManager TimeManager;

        private Lehrer.Lehrer[] AlleLehrer;

        private void Awake()
        {
            AlleLehrer = FindObjectsOfType<Lehrer.Lehrer>();
        }

        private IEnumerable<UnterrichtenGoal> UnterrichtenGoals
        {
            get
            {
                var result = new List<UnterrichtenGoal>();

                foreach (var lehrer in AlleLehrer)
                {
                    foreach (var goal in lehrer.AI.Goals)
                    {
                        if (goal is UnterrichtenGoal unterrichten)
                        {
                            result.Add(unterrichten);
                        }
                    }
                }

                return result;
            }
        }

        public UnterrichtenGoal CurrenttUnterrichtenGoal
        {
            get
            {
                foreach (var lehrer in AlleLehrer)
                {
                    if (lehrer.AI.CurrentGoal is UnterrichtenGoal {SchuelerFreigestelltDieseStunde: false} unterrichten)
                    {
                        return unterrichten;
                    }
                }

                return null;
            }
        }

        public UnterrichtenGoal NextUnterrichtenGoal
        {
            get
            {
                var nextFach = StundenPlanRaster.GetNaechstenStundenPlanEintrag(TimeManager.CurrentWochentag, TimeManager.CurrentTime, eintrag => eintrag.Stunde == StundenType.Fach);
                if (nextFach == null)
                {
                    return null;
                }

                foreach (var goal in UnterrichtenGoals)
                {
                    var stunde = goal.StundeImStundenplan;

                    if (stunde == null)
                    {
                        continue;
                    }

                    if (TimeManager.CurrentWochentag != stunde.Wochentag)
                    {
                        continue;
                    }

                    if (nextFach.FachIndex != stunde.FachIndex)
                    {
                        continue;
                    }

                    return goal;
                }

                return null;
            }
        }

        public string DisplayText
        {
            get
            {
                var current = CurrenttUnterrichtenGoal;
                var next = NextUnterrichtenGoal;

                var builder = new StringBuilder();

                void AppendFach(UnterrichtenGoal goal)
                {
                    if (goal == null)
                    {
                        builder.Append("Keine");
                    }
                    else
                    {
                        builder.Append(goal.StundeImStundenplan.Fach);
                        builder.Append(" (in ").Append(goal.UnterrichtsRaum.Name).Append(")");
                        builder.Append(" (bei ").Append(goal.Lehrer.Configuration.Name).Append(")");
                    }
                }

                builder.Append("Aktuelle Stunde: ");
                AppendFach(current);

                builder.Append("\n");

                builder.Append("Nächste Stunde: ");
                AppendFach(next);

                return builder.ToString();
            }
        }
    }
}
