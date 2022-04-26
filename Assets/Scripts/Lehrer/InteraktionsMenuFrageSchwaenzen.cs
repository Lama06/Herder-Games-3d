using HerderGames.Lehrer.AI.Goals;

namespace HerderGames.Lehrer
{
    public class InteraktionsMenuFrageSchwaenzen : InteraktionsMenuFrage
    {
        protected override bool ShouldShowInInteraktionsMenu()
        {
            return Lehrer.AI.CurrentGoal is UnterrichtenGoal {LehrerArrived: true} unterrichten && unterrichten.GetKlassenraum().PlayerInside;
        }

        protected override void OnAnnahme()
        {
            if (Lehrer.AI.CurrentGoal is not UnterrichtenGoal unterrichten)
            {
                return;
            }

            unterrichten.SchuelerFreigestelltDieseStunde = true;
        }
    }
}