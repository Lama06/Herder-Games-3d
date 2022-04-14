using System;
using System.Collections.Generic;
using HerderGames.Time.Stundenplan;

namespace HerderGames.Time.Data
{
    public enum WochentagAuswahlArt
    {
        Manuell,
        Alle,
        Wochenende,
        Schultage,
        LangtageLang,
        LangtageNormal,
        Kurztage
    }

    public static class WochentagAuswahlArtExtensions
    {
        public static IEnumerable<Wochentag> ResolveWochentage(this WochentagAuswahlArt auswahlArt,
            IEnumerable<Wochentag> manuelleWochentage)
        {
            var result = new List<Wochentag>();

            switch (auswahlArt)
            {
                case WochentagAuswahlArt.Alle:
                    foreach (var value in Enum.GetValues(typeof(Wochentag)))
                    {
                        result.Add((Wochentag) value);
                    }

                    break;
                case WochentagAuswahlArt.Schultage:
                    foreach (var value in Enum.GetValues(typeof(Wochentag)))
                    {
                        if (((Wochentag) value).IsSchultag())
                        {
                            result.Add((Wochentag) value);
                        }
                    }

                    break;
                case WochentagAuswahlArt.Wochenende:
                    foreach (var value in Enum.GetValues(typeof(Wochentag)))
                    {
                        if (((Wochentag) value).IsWochenende())
                        {
                            result.Add((Wochentag) value);
                        }
                    }

                    break;
                case WochentagAuswahlArt.Manuell:
                    result.AddRange(manuelleWochentage);
                    break;
                case WochentagAuswahlArt.LangtageLang:
                    result.AddRange(WochentagLaenge.LangtagLang.GetWochentage());
                    break;
                case WochentagAuswahlArt.LangtageNormal:
                    result.AddRange(WochentagLaenge.LangtagNormal.GetWochentage());
                    break;
                case WochentagAuswahlArt.Kurztage:
                    result.AddRange(WochentagLaenge.Kurztag.GetWochentage());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(auswahlArt), auswahlArt, null);
            }

            return result;
        }
    }
}