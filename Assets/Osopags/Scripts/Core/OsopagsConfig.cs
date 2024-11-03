namespace Osopags
{
    public static class OsopagsConfig
    {
        public static string BackendUrl { get; private set; } = "http://localhost:3000";
        public static string ApiKey { get; private set; } = "";

        public static void Initialize(string backendUrl, string apiKey)
        {
            BackendUrl = backendUrl;
            ApiKey = apiKey;
        }
    }
}
