using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Osopags.Models;
using Osopags.Core;

namespace Osopags.Modules
{
    public class AnalyticsModule
    {
        private readonly Queue<GameEvent> offlineEventQueue;

        public AnalyticsModule()
        {
            offlineEventQueue = new Queue<GameEvent>();
        }

        public async Task<bool> TrackEvent(string eventType, Dictionary<string, object> eventData)
        {
            if (OsopagsHttpClient.IsOnline())
            {
                var eventRequest = new EventRequest
                {
                    eventType = eventType,
                    eventData = eventData
                };

                var response = await OsopagsHttpClient.Post<SuccessResponse<TelemetryEventResponse>>(
                    "/v1/analytics/events",
                    eventRequest,
                    true
                );

                return response.data.id != null;
            }
            else
            {
                offlineEventQueue.Enqueue(new GameEvent
                {
                    EventType = eventType,
                    EventData = eventData,
                    Timestamp = DateTime.UtcNow
                });

                return false;
            }
        }
    }
}