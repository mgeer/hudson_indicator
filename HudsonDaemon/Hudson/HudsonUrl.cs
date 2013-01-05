namespace HudsonIndicator.HudsonDaemon.Hudson
{
    public class HudsonUrl
    {
        private readonly string url;

        public HudsonUrl(string url)
        {
            this.url = url.TrimEnd("/".ToCharArray());
        }

        public string GetJSONAPI()
        {
            return string.Format("{0}/{1}", this.url, "api/json");
        }
    }
}

