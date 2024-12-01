using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Osopags.Models;
using Osopags.Utils;

namespace Osopags.Core
{
    public static class OsopagsHttpClient
    {
        private static UnityWebRequest CreateRequest(string endpoint, string method, bool requiresAuth)
        {
            var url = $"{OsopagsSettings.Instance.GetCurrentConfig().BaseUrl}{endpoint}";
            Debug.Log($"Request URL: {url}");
            var request = new UnityWebRequest(url, method);

            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            if (requiresAuth)
            {
                var token = OsopagsSDK.Instance.Token;
                if (token != null)
                {
                    request.SetRequestHeader("Authorization", $"Bearer {token}");
                }
            }

            return request;
        }

        public static async Task<T> Get<T>(string endpoint, bool requiresAuth = true)
        {
            using (var request = CreateRequest(endpoint, "GET", requiresAuth))
            {
                return await SendRequest<T>(request);
            }
        }

        public static async Task<T> Post<T>(string endpoint, object data, bool requiresAuth = true)
        {
            using (var request = CreateRequest(endpoint, "POST", requiresAuth))
            {
                if (data != null)
                {
                    string jsonData = JsonUtility.ToJson(data);
                    byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
                    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                }

                return await SendRequest<T>(request);
            }
        }

        private static UnityWebRequest CreateRequest(OsopagsConfig config, string endpoint, string method, bool requiresAuth)
        {
            var url = $"{config.BaseUrl}{endpoint}";
            Debug.Log($"Request URL: {url}");
            var request = new UnityWebRequest(url, method);

            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            if (requiresAuth)
            {
                var token = OsopagsSDK.Instance.Token;
                if (token != null)
                {
                    request.SetRequestHeader("Authorization", $"Bearer {token}");
                }
            }

            return request;
        }

        public static async Task<T> Get<T>(OsopagsConfig config, string endpoint, bool requiresAuth = true)
        {
            using (var request = CreateRequest(config, endpoint, "GET", requiresAuth))
            {
                return await SendRequest<T>(request);
            }
        }

        public static async Task<T> Post<T>(OsopagsConfig config, string endpoint, object data, bool requiresAuth = true)
        {
            using (var request = CreateRequest(config, endpoint, "POST", requiresAuth))
            {
                if (data != null)
                {
                    string jsonData = JsonUtility.ToJson(data);
                    byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
                    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                }

                return await SendRequest<T>(request);
            }
        }

        private static async Task<T> SendRequest<T>(UnityWebRequest request)
        {
            try
            {
                await request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    if (OsopagsSettings.Instance.GetCurrentConfig().EnableDebugLog)
                    {
                        Debug.LogError($"HTTP Error: {request.error}");
                        Debug.LogError($"Response: {request.downloadHandler.text}");
                    }
                    throw new Exception(HttpResponseHandler.GetReadableError(
                        request.error,
                        request.downloadHandler.text
                    ));
                }

                var response = JsonUtility.FromJson<T>(request.downloadHandler.text);
                return response;
            }
            catch (Exception e)
            {
                if (OsopagsSettings.Instance.GetCurrentConfig().EnableDebugLog)
                {
                    Debug.LogException(e);
                }
                throw;
            }
        }

        public static bool IsOnline()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
    }
}