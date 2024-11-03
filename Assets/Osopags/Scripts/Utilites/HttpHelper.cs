using System.Net.Http;
using System.Threading.Tasks;

namespace Osopags
{
    public static class HttpHelper
    {
        private static readonly HttpClient client = new HttpClient();

        public static void SetApiKey(string apiKey)
        {
            client.DefaultRequestHeaders.Remove("Authorization");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        }

        public static async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            return await client.PostAsync(url, content);
        }

        public static async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await client.GetAsync(url);
        }

        public static async Task<HttpResponseMessage> PutAsync(string url, HttpContent content)
        {
            return await client.PutAsync(url, content);
        }

        public static async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            return await client.DeleteAsync(url);
        }
    }
}