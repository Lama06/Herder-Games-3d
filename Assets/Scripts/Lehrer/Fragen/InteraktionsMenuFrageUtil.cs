using System;
using HerderGames.Lehrer.AI.Goals;
using Vector3 = UnityEngine.Vector3;

namespace HerderGames.Lehrer.Fragen
{
    public static class InteraktionsMenuFrageUtil
    {
        public static Func<Player.Player, Lehrer, bool> ShowWhileKannSchwaenzen()
        {
            return (_, lehrer) => lehrer.AI.CurrentGoal is UnterrichtenGoal {LehrerArrived: true, SchuelerFreigestelltDieseStunde: false} unterrichten && unterrichten.GetKlassenraum().PlayerInside;
        }

        public static Func<Player.Player, Lehrer, bool> ShowNearby()
        {
            return (player, lehrer) => Vector3.Distance(player.transform.position, lehrer.transform.position) <= 10f;
        }

        public static Action<Player.Player, Lehrer> AusUnterrichtFreistellen()
        {
            return (_, lehrer) =>
            {
                if (lehrer.AI.CurrentGoal is not UnterrichtenGoal unterrichten)
                {
                    return;
                }

                unterrichten.SchuelerFreigestelltDieseStunde = true;
            };
        }
    }
}
