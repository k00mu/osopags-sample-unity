using UnityEngine;
using Osopags.Models;
using Osopags.Modules;

namespace Osopags.Core
{
    public class OsopagsSDK
    {
        private static OsopagsSDK instance;
        public static OsopagsSDK Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OsopagsSDK();
                }
                return instance;
            }
        }

        private IAMModule iamModule;
        private AnalyticsModule analyticsModule;
        private OsopagsConfig currentConfig;
        private AuthToken currentToken;

        public IAMModule IAM => iamModule;
        public AnalyticsModule Analytics => analyticsModule;
        public OsopagsConfig Config => currentConfig;

        private OsopagsSDK()
        {
            LoadConfig();
            InitializeServices();
        }

        private void LoadConfig()
        {
            var settings = Resources.Load<OsopagsSettings>("OsopagsSettings");
            if (settings == null)
            {
                Debug.LogError("Osopags settings not found!");
                return;
            }

            // Load config based on current environment
#if DEVELOPMENT
            currentConfig = settings.Config.Development;
#elif STAGING
            currentConfig = settings.Config.Staging;
#else
            currentConfig = settings.Config.Production;
#endif

            if (currentConfig.EnableDebugLog)
            {
                Debug.Log($"Osopags SDK initialized with environment: {currentConfig.GameNamespace}");
            }
        }

        private void InitializeServices()
        {
            var httpClient = new HttpClient(currentConfig);
            iamModule = new IAMModule(httpClient);
            analyticsModule = new AnalyticsModule(httpClient);
        }

        public void SetToken(AuthToken token)
        {
            currentToken = token;
            if (currentConfig.EnableDebugLog)
            {
                Debug.Log("Auth token updated");
            }
        }

        public AuthToken GetToken()
        {
            return currentToken;
        }
    }
}