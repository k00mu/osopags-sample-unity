using System.Threading.Tasks;
using System.Net.Http;
using UnityEngine;

namespace Osopags
{
    public class OsopagsAnalytics
    {
        public async Task SendEventAsync(string eventName, string eventData)
        {
            var url = $"{OsopagsConfig.BackendUrl}/events";
            var content = new StringContent($"{{\"eventName\":\"{eventName}\",\"eventData\":{eventData}}}", System.Text.Encoding.UTF8, "application/json");

            var response = await HttpHelper.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                Debug.Log("Event sent successfully.");
            }
            else
            {
                Debug.LogError("Failed to send event.");
            }
        }
    }
}
