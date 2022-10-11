using System;
using HerderGames.Zeit;
using UnityEditor;
using Object = UnityEngine.Object;

namespace HerderGames.Editor
{
    public static class SkipStundeMenu
    {
        [MenuItem("Herder Games/Nächste Stunde")]
        public static void SkipStunde()
        {
            var timeManager = Object.FindObjectOfType<TimeManager>();
            var naechstesFach = StundenPlanRaster.GetNaechstenStundenPlanEintrag(timeManager.CurrentWochentag, timeManager.CurrentTime, _ => true);
            timeManager.CurrentWochentag = naechstesFach.Tag;
            timeManager.CurrentTime = naechstesFach.Beginn;
        }
    }
}