using System;
using System.Collections.Generic;
using HerderGames.Time.Stundenplan;

namespace HerderGames.Time.Data
{
    public enum ZeitRelativitaet
    {
        Absolut,
        SchuleBeginn,
        SchuleEnde,
        FachAnfangAlle,
        FachAnfangN,
        FachEndeAlle,
        FachEndeN,
        KurzpauseAnfangAlle,
        KurzpauseAnfangN,
        KurzpauseEndeAlle,
        KurzpauseEndeN,
        LernzeitAnfang,
        LernzeitEnde,
        MittagspauseAnfang,
        MittagspauseEnde
    }

    public static class ZeitRelaitivitaetExtensions
    {
        public static IList<float> ResolveToAbsoluteTime(this ZeitRelativitaet relativitaet, Wochentag wochentag, float relativ,
            int n)
        {
            var results = new List<float>();

            var stundenPlan = StundenPlanRaster.GetTagesAblauf(wochentag);
            var faecher = StundenPlanRaster.GetAlleStundenPlanEintraegeWithType(wochentag, StundenType.Fach);
            var fachN = StundenPlanRaster.GetStundenPlanEintragForFach(wochentag, n);
            var kurzpausen = StundenPlanRaster.GetAlleStundenPlanEintraegeWithType(wochentag, StundenType.Kurzpause);
            var kurzpauseN = StundenPlanRaster.GetStundenPlanEintragForKurzpause(wochentag, n);
            var lernzeit = StundenPlanRaster.GetAlleStundenPlanEintraegeWithType(wochentag, StundenType.Lernzeit);
            var mittagspause =
                StundenPlanRaster.GetAlleStundenPlanEintraegeWithType(wochentag, StundenType.Mittagspause);
            
            switch (relativitaet)
            {
                case ZeitRelativitaet.Absolut:
                    results.Add(relativ);
                    break;
                case ZeitRelativitaet.SchuleBeginn:
                    if (wochentag.IsSchultag())
                    {
                        results.Add(StundenPlanRaster.SchuleBeginn + relativ);
                    }
                    break;
                case ZeitRelativitaet.SchuleEnde:
                    if (stundenPlan.Count > 0)
                    {
                        var letzteStunde = stundenPlan[^1];
                        results.Add(letzteStunde.Ende + relativ);
                    }
                    break;
                case ZeitRelativitaet.FachAnfangAlle:
                    foreach (var fach in faecher)
                    {
                        results.Add(fach.Beginn + relativ);
                    }

                    break;
                case ZeitRelativitaet.FachAnfangN:
                    if (fachN != null)
                    {
                        results.Add(fachN.Beginn + relativ);   
                    }
                    break;
                case ZeitRelativitaet.FachEndeAlle:
                    foreach (var fach in faecher)
                    {
                        results.Add(fach.Ende + relativ);
                    }

                    break;
                case ZeitRelativitaet.FachEndeN:
                    if (fachN != null)
                    {
                        results.Add(fachN.Ende + relativ);
                    }

                    break;
                case ZeitRelativitaet.KurzpauseAnfangAlle:
                    foreach (var kurzpause in kurzpausen)
                    {
                        results.Add(kurzpause.Beginn + relativ);
                    }

                    break;
                case ZeitRelativitaet.KurzpauseAnfangN:
                    if (kurzpauseN != null)
                    {
                        results.Add(kurzpauseN.Beginn + relativ);
                    }
                    break;
                case ZeitRelativitaet.KurzpauseEndeAlle:
                    foreach (var kurzpause in kurzpausen)
                    {
                        results.Add(kurzpause.Ende + relativ);
                    }
                    break;
                case ZeitRelativitaet.KurzpauseEndeN:
                    if (kurzpauseN != null)
                    {
                        results.Add(kurzpauseN.Ende + relativ);
                    }
                    break;
                case ZeitRelativitaet.LernzeitAnfang:
                    if (lernzeit.Count == 1)
                    {
                        results.Add(lernzeit[0].Beginn + relativ);
                    }
                    break;
                case ZeitRelativitaet.LernzeitEnde:
                    if (lernzeit.Count == 1)
                    {
                        results.Add(lernzeit[0].Ende + relativ);
                    }
                    break;
                case ZeitRelativitaet.MittagspauseAnfang:
                    if (mittagspause.Count == 1)
                    {
                        results.Add(mittagspause[0].Beginn + relativ);
                    }
                    break;
                case ZeitRelativitaet.MittagspauseEnde:
                    if (mittagspause.Count == 1)
                    {
                        results.Add(mittagspause[0].Ende + relativ);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(relativitaet), relativitaet, null);
            }

            return results;
        }
    }
}
