using System;
using System.Collections.Generic;

namespace Osopags.Models
{
    // Request Models
    [Serializable]
    public class UpsertUserRequest
    {
        public string username;
        public string email;
        public string password;
    }

    [Serializable]
    public class GetUserRequest
    {
        public string id;
    }

    [Serializable]
    public class UpsertGameClientRequest
    {
        public string gameName;
        public string gameNamespace;
    }

    [Serializable]
    public class GetGameClientRequest
    {
        public string id;
    }

    [Serializable]
    public class AuthUserRequest
    {
        public string username;
        public string email;
        public string password;
    }

    [Serializable]
    public class AuthDeviceRequest
    {
        public string machine_id;
        public string client_id;
    }

    [Serializable]
    public class LinkDeviceToUserRequest
    {
        public string userId;
        public string deviceId;
        public string gameClientId;
        public bool isAnonymous;
    }

    [Serializable]
    public class TrackRequest
    {
        public string eventType;
    }

    [Serializable]
    public class TrackWithDataRequest
    {
        public string eventType;
        public Dictionary<string, object> eventData;
    }

    [Serializable]
    public class TrackEventModel
    {
        public string id;
        public string user_id;
        public string event_type;
        public Dictionary<string, object> event_data;
        public DateTime timestamp;
    }

    [Serializable]
    public class EventStats
    {
        public string eventType;
        public int count;
        public DateTime firstSeen;
        public DateTime lastSeen;
    }

    // Response Models
    [Serializable]
    public class SuccessResponse<T>
    {
        public string status = "success";
        public T data;
        public string message;
    }

    [Serializable]
    public class ErrorResponse
    {
        public string status = "error";
        public string code;
        public string message;
        public Dictionary<string, object> details;
        public string stack;
        public int? retryAfter;
    }

    [Serializable]
    public class UpsertUserResponse
    {
        public string id;
        public string username;
        public string email;
    }

    [Serializable]
    public class GetUserResponse
    {
        public string id;
        public string username;
        public string email;
    }

    [Serializable]
    public class UpsertGameClientResponse
    {
        public string id;
        public string gameName;
        public string gameNamespace;
    }

    [Serializable]
    public class GetGameClientResponse
    {
        public string id;
        public string gameName;
        public string gameNamespace;
    }

    [Serializable]
    public class AuthUserResponse
    {
        public string userToken;
    }

    [Serializable]
    public class AuthDeviceResponse
    {
        public string deviceToken;
    }

    [Serializable]
    public class TrackResponse
    {
        public string id;
        public string gameClientId;
        public string deviceId;
        public string eventType;
        public DateTime timestamp;
    }

    [Serializable]
    public class TrackWithDataResponse
    {
        public string id;
        public string gameClientId;
        public string deviceId;
        public string eventType;
        public Dictionary<string, object> eventData;
        public DateTime timestamp;
    }

    [Serializable]
    public class TrackEventStats
    {
        public string eventType;
        public int count;
        public DateTime firstSeen;
        public DateTime lastSeen;
    }
}