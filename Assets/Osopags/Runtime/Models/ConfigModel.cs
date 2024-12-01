using System;
using UnityEngine;

namespace Osopags.Models
{
    [Serializable]
    public class OsopagsConfig
    {
        public string BaseUrl;
        public string ClientId;
        public string GameNamespace;
        public bool EnableDebugLog;
    }

    [Serializable]
    public class EnvironmentConfig
    {
        public OsopagsConfig Development;
        public OsopagsConfig Production;
    }
}