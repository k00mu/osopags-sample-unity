// Utilities/Logger.cs
using UnityEngine;

namespace Osopags.Utilities
{
    public static class Logger
    {
        private const string prefix = "[Osopags]";

        public static void Log(string message)
        {
            Debug.Log($"{prefix} {message}");
        }

        public static void LogWarning(string message)
        {
            Debug.LogWarning($"{prefix} {message}");
        }

        public static void LogError(string message)
        {
            Debug.LogError($"{prefix} {message}");
        }
    }
}
