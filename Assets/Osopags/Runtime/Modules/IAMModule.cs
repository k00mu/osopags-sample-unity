using System;
using System.Threading.Tasks;
using UnityEngine;
using Osopags.Core;
using Osopags.Models;

namespace Osopags.Modules
{
    public class IAMModule
    {
        private readonly string deviceId;

        public IAMModule()
        {
            deviceId = GetOrCreateDeviceId();
        }


        public void TestConnection(
            OsopagsConfig config,
            SuccessDelegate<bool> onSuccess = null,
            ErrorDelegate<ErrorResponse> onError = null
        )
        {
            _ = TestConnectionInternal(config, onSuccess, onError);
        }

        public void AuthDevice(
            SuccessDelegate<AuthDeviceResponse> onSuccess = null,
            ErrorDelegate<ErrorResponse> onError = null
        )
        {
            _ = AuthDeviceInternal(onSuccess, onError);
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

        // TODO: lets make separate endpoint for this
        private async Task TestConnectionInternal(
            OsopagsConfig config,
            SuccessDelegate<bool> onSuccess = null,
            ErrorDelegate<ErrorResponse> onError = null
        )
        {
            try
            {
                var response = await OsopagsHttpClient
                    .Get<SuccessResponse<GetGameClientResponse>>(
                        config,
                        $"/v1/iam/clients/{config.ClientId}",
                        false
                    );

                onSuccess?.Invoke(response != null && !string.IsNullOrEmpty(response.data.id));
            }
            catch (Exception ex)
            {
                onError?.Invoke(new ErrorResponse { message = ex.Message });
            }
        }

        private async Task AuthDeviceInternal(
            SuccessDelegate<AuthDeviceResponse> onSuccess = null,
            ErrorDelegate<ErrorResponse> onError = null
        )
        {
            try
            {
                var request = new AuthDeviceRequest
                {
                    machine_id = deviceId,
                    client_id = OsopagsSettings.Instance.GetCurrentConfig().ClientId
                };

                var response = await OsopagsHttpClient
                .Post<SuccessResponse<AuthDeviceResponse>>(
                        "/v1/iam/auth/device",
                        request,
                        false
                    );

                OsopagsSDK.Instance.Token = response.data.deviceToken;

                onSuccess?.Invoke(response.data);
            }
            catch (Exception ex)
            {
                onError?.Invoke(new ErrorResponse { message = ex.Message });
            }
        }
    }
}