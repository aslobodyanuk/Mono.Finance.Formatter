namespace Finance.Formatter.Models.Config
{
    public class AppConfig
    {
        public double UahToUsdMultiplier { get; set; }

        public double EurToUsdMultiplier { get; set; }

        public string TemplatePath { get; set; }

        public KeywordConfig[] Keywords { get; set; }

        public string[] IgnoredKeywords { get; set; }
    }
}
