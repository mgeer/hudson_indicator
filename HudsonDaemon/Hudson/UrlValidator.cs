using System.Text.RegularExpressions;

namespace HudsonIndicator.HudsonDaemon.Hudson
{
    internal static class UrlValidator
    {
        private static readonly Regex UrlPattern = new Regex("^http://.+$", RegexOptions.Compiled);

        public static bool Validate(string url)
        {
            return UrlPattern.IsMatch(url);
        }
    }
}

