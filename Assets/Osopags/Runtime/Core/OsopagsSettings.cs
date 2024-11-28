using UnityEngine;
using Osopags.Models;

namespace Osopags.Core
{
    /// <summary>
    /// ScriptableObject to store Osopags configuration settings.
    /// This will be created and managed through the Unity Editor.
    /// </summary>
    public class OsopagsSettings : ScriptableObject
    {
        private static OsopagsSettings instance;
        public static OsopagsSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<OsopagsSettings>("OsopagsSettings");
                    if (instance == null)
                    {
                        Debug.LogError("OsopagsSettings not found in Resources folder! Please create settings through Osopags/Settings");
                    }
                }
                return instance;
            }
        }

        [SerializeField]
        private EnvironmentConfig config;
        public EnvironmentConfig Config
        {
            get => config;
            set => config = value;
        }

        [SerializeField]
        private string currentEnvironment = "Development";
        public string CurrentEnvironment
        {
            get => currentEnvironment;
            set => currentEnvironment = value;
        }

        /// <summary>
        /// Get the current environment's configuration
        /// </summary>
        public OsopagsConfig GetCurrentConfig()
        {
            if (config == null)
            {
                Debug.LogError("Osopags configuration is not set!");
                return null;
            }

            switch (currentEnvironment.ToLower())
            {
                case "development":
                    return config.Development;
                case "production":
                    return config.Production;
                default:
                    Debug.LogError($"Unknown environment: {currentEnvironment}");
                    return config.Development; // Fallback to development
            }
        }

        /// <summary>
        /// Validate the current configuration
        /// </summary>
        public bool ValidateConfig()
        {
            var currentConfig = GetCurrentConfig();
            if (currentConfig == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(currentConfig.BaseUrl))
            {
                Debug.LogError("Base URL is not set!");
                return false;
            }

            if (string.IsNullOrEmpty(currentConfig.ClientId))
            {
                Debug.LogError("Client ID is not set!");
                return false;
            }

            if (string.IsNullOrEmpty(currentConfig.GameNamespace))
            {
                Debug.LogError("Game Namespace is not set!");
                return false;
            }

            return true;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // Ensure config exists
            if (config == null)
            {
                config = new EnvironmentConfig
                {
                    Development = new OsopagsConfig(),
                    Production = new OsopagsConfig()
                };
            }

            // Ensure all environment configs exist
            if (config.Development == null)
                config.Development = new OsopagsConfig();
            if (config.Production == null)
                config.Production = new OsopagsConfig();

            // Validate URLs format
            ValidateUrl(config.Development.BaseUrl);
            ValidateUrl(config.Production.BaseUrl);
        }

        private void ValidateUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                {
                    Debug.LogWarning($"URL should start with 'http://' or 'https://': {url}");
                }
            }
        }
#endif

        /// <summary>
        /// Create default settings asset
        /// </summary>
        public static OsopagsSettings CreateDefaultSettings()
        {
            var settings = CreateInstance<OsopagsSettings>();
            settings.Config = new EnvironmentConfig
            {
                Development = new OsopagsConfig
                {
                    BaseUrl = "http://localhost:3000",
                    EnableDebugLog = true
                },
                Production = new OsopagsConfig
                {
                    BaseUrl = "https://api.yourgame.com",
                    EnableDebugLog = false
                }
            };
            return settings;
        }
    }
}