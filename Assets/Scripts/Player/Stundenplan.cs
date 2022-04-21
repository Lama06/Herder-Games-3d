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

        private Lehrer.Lehrer[] AlleLehrer;
        private UnterrichtenGoal[] UnterrichtenGoals;

        private void Awake()
        {
            AlleLehrer = FindObjectsOfType<Lehrer.Lehrer>();
            UnterrichtenGoals = FindObjectsOfType<UnterrichtenGoal>();
        }

        public UnterrichtenGoal GetCurrenttUnterrichtenGoal()
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

        public UnterrichtenGoal GetNextUnterrichtenGoal()
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
                var stunde = goal.GetStunde();
                
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

            return null;
        }

        public string GetDisplayText()
        {
            var current = GetCurrenttUnterrichtenGoal();
            var next = GetNextUnterrichtenGoal();

            var builder = new StringBuilder();

            void AppendFach(UnterrichtenGoal goal)
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