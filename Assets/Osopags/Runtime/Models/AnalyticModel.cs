using System;
using System.Collections.Generic;

namespace Osopags.Models
{
    public class OfflineTrackRequest
    {
        public string EventType { get; set; }
        public Dictionary<string, object> EventData { get; set; }
        public DateTime Timestamp { get; set; }
    }
}