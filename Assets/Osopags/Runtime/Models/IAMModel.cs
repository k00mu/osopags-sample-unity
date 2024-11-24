using System;

namespace Osopags.Models
{
    public class AuthToken
    {
        public string AccessToken { get; set; }
        public string SessionId { get; set; }
        public string DeviceId { get; set; }
        public bool IsAnonymous { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    public class AuthResponse
    {
        public AuthToken Token { get; set; }
        public string Error { get; set; }
    }
}