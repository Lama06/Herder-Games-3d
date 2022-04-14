using System;
using System.Collections.Generic;

namespace HerderGames.Time.Stundenplan
{
    public enum WochentagLaenge
    {
        LangtagLang,
        LangtagNormal,
        Kurztag,
        Wochenende
    }

    public static class WochentagLaengeExtensions
    {
        public static IEnumerable<StundenType> GetAblauf(this WochentagLaenge laenge)
        {
            return laenge switch
            {
                WochentagLaenge.LangtagLang => new[]
                {
                    StundenType.Fach, StundenType.Kurzpause, StundenType.Fach, StundenType.Kurzpause, StundenType.Fach,
                    StundenType.Mittagspause, StundenType.Fach
                },
                WochentagLaenge.LangtagNormal => new[]
                {
                    StundenType.Fach, StundenType.Kurzpause, StundenType.Fach, StundenType.Kurzpause,
                    StundenType.Lernzeit,
                    StundenType.Mittagspause, StundenType.Fach
                },
                WochentagLaenge.Kurztag => new[]
                {
                    StundenType.Fach, StundenType.Kurzpause, StundenType.Fach, StundenType.Kurzpause, StundenType.Fach
                },
                WochentagLaenge.Wochenende => Array.Empty<StundenType>(),
                _ => throw new ArgumentOutOfRangeException(nameof(laenge), laenge, null)
            };
        }

        public static IEnumerable<Wochentag> GetWochentage(this WochentagLaenge laenge)
        {
            return laenge switch
            {
                WochentagLaenge.LangtagLang => new[] {Wochentag.Montag},
                WochentagLaenge.LangtagNormal => new[] {Wochentag.Mittwoch, Wochentag.Donnernstag},
                WochentagLaenge.Kurztag => new[] {Wochentag.Dienstag, Wochentag.Freitag},
                WochentagLaenge.Wochenende => new[] {Wochentag.Samstag, Wochentag.Sonntag},
                _ => throw new ArgumentOutOfRangeException(nameof(laenge), laenge, null)
            };
        }
    }
}