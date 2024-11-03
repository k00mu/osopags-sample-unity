namespace Osopags
{
    public static class Osopags
    {
        public static OsopagsAnalytics Analytics { get; private set; }

        public static void Initialize(string backendUrl, string apiKey)
        {
            OsopagsConfig.Initialize(backendUrl, apiKey);
            HttpHelper.SetApiKey(apiKey);
            Analytics = new OsopagsAnalytics();
        }
    }
}
