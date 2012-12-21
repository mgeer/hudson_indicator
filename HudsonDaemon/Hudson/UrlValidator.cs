namespace HudsonDaemon.Hudson
{
    using System.Text.RegularExpressions;

    internal static class UrlValidator
    {
        private static readonly Regex UrlPattern = new Regex("^http://.+$", RegexOptions.Compiled);

        public static bool Validate(string url)
        {
            return UrlPattern.IsMatch(url);
        }
    }
}

