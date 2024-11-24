using System;
using System.Threading.Tasks;
using UnityEngine;
using Osopags.Models;
using Osopags.Core;

namespace Osopags.Modules
{
    public class IAMModule
    {
        private readonly HttpClient httpClient;
        private readonly string deviceId;

        public IAMModule(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.deviceId = GetOrCreateDeviceId();
        }

        private string GetOrCreateDeviceId()
        {
            string key = "Osopags_DeviceId";
            string deviceId = PlayerPrefs.GetString(key);

            if (string.IsNullOrEmpty(deviceId))
            {
                deviceId = Guid.NewGuid().ToString();
                PlayerPrefs.SetString(key, deviceId);
                PlayerPrefs.Save();
            }

            return deviceId;
        }

        public async Task<bool> TestConnection(OsopagsConfig config)
        {
            try
            {
                var response = await httpClient.Get<SuccessResponse<GetGameClientResponse>>(config, $"/v1/iam/clients/{config.ClientId}", false);
                Debug.Log($"Connection test response: {response.data.gameName}");
                return response != null && !string.IsNullOrEmpty(response.data.id);
            }
            catch (Exception e)
            {
                Debug.LogError($"Connection test failed: {e.Message}");
                return false;
            }
        }

        public async Task<AuthResponse> AuthenticateDevice()
        {
            var request = new
            {
                device_id = deviceId,
                client_id = OsopagsSDK.Instance.Config.ClientId,
            };

            try
            {
                var response = await httpClient.Post<AuthResponse>("/v1/iam/device/auth", request, false);
                if (response.Token != null)
                {
                    OsopagsSDK.Instance.SetToken(response.Token);
                }
                return response;
            }
            catch (Exception e)
            {
                return new AuthResponse { Error = e.Message };
            }
        }

        public async Task<AuthResponse> Register(string username, string email, string password)
        {
            var request = new
            {
                username,
                email,
                password
            };

            try
            {
                var response = await httpClient.Post<AuthResponse>("/v1/iam/register", request, false);
                if (response.Token != null)
                {
                    OsopagsSDK.Instance.SetToken(response.Token);
                }
                return response;
            }
            catch (Exception e)
            {
                return new AuthResponse { Error = e.Message };
            }
        }

        public async Task<AuthResponse> Login(string username, string password)
        {
            var request = new
            {
                username,
                password
            };

            try
            {
                var response = await httpClient.Post<AuthResponse>("/v1/iam/login", request, false);
                if (response.Token != null)
                {
                    OsopagsSDK.Instance.SetToken(response.Token);
                }
                return response;
            }
            catch (Exception e)
            {
                return new AuthResponse { Error = e.Message };
            }
        }
    }
}