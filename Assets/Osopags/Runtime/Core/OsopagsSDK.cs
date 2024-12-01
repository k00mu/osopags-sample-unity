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
        private AnalyticModule analyticModule;
        private string currentToken;
        public string Token
        {
            get { return currentToken; }
            set
            {
                currentToken = value;
                if (OsopagsSettings.Instance.GetCurrentConfig().EnableDebugLog)
                {
                    Debug.Log("Token updated");
                }
            }
        }

        public IAMModule IAM => iamModule;
        public AnalyticModule Analytic => analyticModule;

        private OsopagsSDK()
        {
            InitializeServices();
        }

        private void InitializeServices()
        {
            iamModule = new IAMModule();
            analyticModule = new AnalyticModule();
        }
    }
}