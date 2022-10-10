using System;
using HerderGames.Zeit;
using UnityEditor;
using Object = UnityEngine.Object;

namespace HerderGames.Editor
{
    public static class SkipStundeMenu
    {
        [MenuItem("Herder Games/Nächste Unterrichtsstunde")]
        public static void SkipStunde()
        {
            var timeManager = Object.FindObjectOfType<TimeManager>();
            var naechstesFach = StundenPlanRaster.GetNaechstenStundenPlanEintrag(timeManager.CurrentWochentag, timeManager.CurrentTime,
                eintrag => eintrag.Stunde == StundenType.Fach);
            timeManager.CurrentWochentag = naechstesFach.Tag;
            timeManager.CurrentTime = naechstesFach.Beginn;
        }
    }
}