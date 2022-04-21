using HerderGames.Lehrer.Goals;
using UnityEngine;

namespace HerderGames.Lehrer
{
    [RequireComponent(typeof(Unterrichten))]
    public class InteraktionsMenuFrageSchwaenzen : InteraktionsMenuFrage
    {
        protected override bool ShouldShowInInteraktionsMenu()
        {
            return Lehrer.AI.CurrentGoal is Unterrichten {LehrerArrived: true} unterrichten && unterrichten.GetKlassenraum().PlayerInside;
        }

        protected override void OnSucces()
        {
            if (Lehrer.AI.CurrentGoal is not Unterrichten unterrichten)
            {
                return;
            }

            unterrichten.SchuelerFreigestelltDieseStunde = true;
        }
    }
}