using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MediaPortal.Plugins
{
    internal static class DayNightBackgrounds
    {
        private static readonly Dictionary<string, string> Pairs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Modern/abstract.jpg", "Modern/Day/abstract.png" },
            { "Modern/cinema.jpg", "Modern/Day/cinema.png" },
            { "Modern/gaming.jpg", "Modern/Day/gaming.png" },
            { "Modern/home-lounge.png", "Modern/Day/home-lounge.png" },
            { "Modern/home.jpg", "Modern/Day/cinema.png" },
            { "Modern/photos.jpg", "Modern/Day/photos.png" },
            { "Modern/plugins-browser.png", "Modern/Day/plugins-browser.png" },
            { "Modern/plugins.jpg", "Modern/Day/plugins.png" },
            { "Modern/settings-console.png", "Modern/Day/settings-console.png" },
            { "Modern/studio.jpg", "Modern/Day/studio.png" },
            { "Modern/tv.jpg", "Modern/Day/tv.png" },
            { "Animations/games/default.jpg", "Modern/Day/abstract.png" },
            { "Animations/shutdownbg/default.jpg", "Modern/Day/abstract.png" },
        };

        internal static string Apply(string xml)
        {
            return Regex.Replace(xml, @"(<texture(?:\s[^>]*)?>)([^<]*)(</texture>)", delegate(Match match)
            {
                string night = match.Groups[2].Value.Replace('\\', '/');
                while (night.Contains("//")) night = night.Replace("//", "/");
                string day;
                if (!Pairs.TryGetValue(night, out day)) return match.Value;
                string morning = day.Replace("Modern/Day/", "Modern/Morning/");
                string noon = day.Replace("Modern/Day/", "Modern/Noon/");
                return match.Groups[1].Value + "#(iif(string.equals(#DayNight.Period,'morning'),'" + morning
                    + "',iif(string.equals(#DayNight.Period,'noon'),'" + noon
                    + "',iif(string.equals(#DayNight.Period,'day'),'" + day + "','" + night + "'))))" + match.Groups[3].Value;
            }, RegexOptions.IgnoreCase);
        }
    }
}
