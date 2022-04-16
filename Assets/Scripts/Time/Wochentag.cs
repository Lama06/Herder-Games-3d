using System;

namespace HerderGames.Time
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
        public static WochentagLaenge GetWochentagType(this Wochentag tag)
        {
            return tag switch
            {
                Wochentag.Montag => WochentagLaenge.LangtagLang,
                Wochentag.Mittwoch or Wochentag.Donnernstag => WochentagLaenge.LangtagNormal,
                Wochentag.Dienstag or Wochentag.Freitag => WochentagLaenge.Kurztag,
                Wochentag.Samstag or Wochentag.Sonntag => WochentagLaenge.Wochenende,
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

        public static bool IsSchultag(this Wochentag tag)
        {
            return tag != Wochentag.Samstag && tag != Wochentag.Sonntag;
        }

        public static bool IsWochenende(this Wochentag tag)
        {
            return !tag.IsSchultag();
        }
    }
}