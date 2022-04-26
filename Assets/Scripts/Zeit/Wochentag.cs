using System;
using System.Collections.Generic;

namespace HerderGames.Zeit
{
    public enum Wochentag
    {
        Montag,
        Dienstag,
        Mittwoch,
        Donnernstag,
        Freitag,
        Samstag,
        Sonntag
    }

    public static class WochentagExtensions
    {
        public static IList<StundenType> GetAblauf(this Wochentag tag)
        {
            return tag switch
            {
                Wochentag.Montag => new List<StundenType>
                {
                    StundenType.Fach, StundenType.Kurzpause, StundenType.Fach, StundenType.Kurzpause, StundenType.Fach,
                    StundenType.Mittagspause, StundenType.Fach
                },
                Wochentag.Mittwoch or Wochentag.Donnernstag => new List<StundenType>
                {
                    StundenType.Fach, StundenType.Kurzpause, StundenType.Fach, StundenType.Kurzpause,
                    StundenType.Lernzeit, StundenType.Mittagspause, StundenType.Fach
                },
                Wochentag.Dienstag or Wochentag.Freitag => new List<StundenType>
                {
                    StundenType.Fach, StundenType.Kurzpause, StundenType.Fach, StundenType.Kurzpause, StundenType.Fach
                },
                Wochentag.Samstag or Wochentag.Sonntag => new List<StundenType>(),
                _ => throw new ArgumentOutOfRangeException(nameof(tag), tag, null)
            };
        }

        public static ISet<WochentagEigenschaft> GetEigenschaften(this Wochentag tag)
        {
            return tag switch
            {
                Wochentag.Montag => new HashSet<WochentagEigenschaft> {WochentagEigenschaft.Schultag, WochentagEigenschaft.LangtagLang},
                Wochentag.Dienstag => new HashSet<WochentagEigenschaft> {WochentagEigenschaft.Schultag, WochentagEigenschaft.Kurztag},
                Wochentag.Mittwoch => new HashSet<WochentagEigenschaft> {WochentagEigenschaft.Schultag, WochentagEigenschaft.LangtagNormal},
                Wochentag.Donnernstag => new HashSet<WochentagEigenschaft> {WochentagEigenschaft.Schultag, WochentagEigenschaft.LangtagNormal},
                Wochentag.Freitag => new HashSet<WochentagEigenschaft> {WochentagEigenschaft.Schultag, WochentagEigenschaft.Kurztag},
                Wochentag.Samstag => new HashSet<WochentagEigenschaft> {WochentagEigenschaft.Wochenende},
                Wochentag.Sonntag => new HashSet<WochentagEigenschaft> {WochentagEigenschaft.Wochenende},
                _ => throw new ArgumentOutOfRangeException(nameof(tag), tag, null)
            };
        }

        public static Wochentag GetNextWochentag(this Wochentag tag)
        {
            return (Wochentag) ((int) (tag + 1) % 7);
        }

        public static bool IsValid(this Wochentag tag)
        {
            return (int) tag <= 6 && (int) tag >= 0;
        }
    }
}