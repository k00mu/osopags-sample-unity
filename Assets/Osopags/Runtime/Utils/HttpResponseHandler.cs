using UnityEngine;
using System;

namespace Osopags.Utils
{
    public static class HttpResponseHandler
    {
        public static string GetReadableError(string errorMessage, string responseText)
        {
            try
            {
                // Try to parse error response
                var errorResponse = JsonUtility.FromJson<ErrorResponse>(responseText);
                if (errorResponse != null && !string.IsNullOrEmpty(errorResponse.message))
                {
                    return $"{errorMessage}\n\nServer message: {errorResponse.message}";
                }
            }
            catch
            {
                // If parsing fails, return original error
            }

            return errorMessage;
        }
    }

    [Serializable]
    public class ErrorResponse
    {
        public string status;
        public string code;
        public string message;
    }
}