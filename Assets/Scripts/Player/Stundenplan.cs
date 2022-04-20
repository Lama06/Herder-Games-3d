using System.Text;
using HerderGames.Lehrer.Goals;
using HerderGames.Time;
using UnityEngine;

namespace HerderGames.Player
{
    [RequireComponent(typeof(Player))]
    public class Stundenplan : MonoBehaviour
    {
        [SerializeField] private TimeManager TimeManager;

        private Unterrichten[] UnterrichtenGoals;

        private void Awake()
        {
            UnterrichtenGoals = FindObjectsOfType<Unterrichten>();
        }

        public Unterrichten GetCurrenttUnterrichtenGoal()
        {
            var eintragCurrent = StundenPlanRaster.GetCurrentStundenPlanEintrag(TimeManager);
            if (eintragCurrent == null || eintragCurrent.Stunde != StundenType.Fach)
            {
                return null;
            }

            foreach (var goal in UnterrichtenGoals)
            {
                foreach (var stunde in goal.GetStunden())
                {
                    if (TimeManager.GetCurrentWochentag() != stunde.Wochentag)
                    {
                        continue;
                    }

                    if (eintragCurrent.FachIndex != stunde.FachIndex)
                    {
                        continue;
                    }

                    return goal;
                }
            }

            return null;
        }

        public Unterrichten GetNextUnterrichtenGoal()
        {
            var eintragNext =
                StundenPlanRaster.GetNaechstenStundenPlanEintragWithType(TimeManager.GetCurrentWochentag(),
                    TimeManager.GetCurrentTime(), StundenType.Fach);
            if (eintragNext == null)
            {
                return null;
            }

            foreach (var goal in UnterrichtenGoals)
            {
                foreach (var stunde in goal.GetStunden())
                {
                    if (TimeManager.GetCurrentWochentag() != stunde.Wochentag)
                    {
                        continue;
                    }

                    if (eintragNext.FachIndex != stunde.FachIndex)
                    {
                        continue;
                    }

                    return goal;
                }
            }
            
            return null; 
        }

        public string GetDisplayText()
        {
            var current = GetCurrenttUnterrichtenGoal();
            var next = GetNextUnterrichtenGoal();
            
            var builder = new StringBuilder();

            void AppendFach(Unterrichten goal)
            {
                if (goal == null)
                {
                    builder.Append("Keine");
                }
                else
                {
                    builder.Append(goal.GetFach());
                    builder.Append(" (in ").Append(goal.GetKlassenraum().GetName()).Append(")");
                    builder.Append(" (bei ").Append(goal.Lehrer.GetName()).Append(")");
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
