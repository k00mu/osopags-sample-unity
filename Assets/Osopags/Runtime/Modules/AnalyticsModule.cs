using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Osopags.Models;
using Osopags.Core;

namespace Osopags.Modules
{
    public class AnalyticsModule
    {
        private readonly HttpClient httpClient;
        private readonly Queue<GameEvent> eventQueue;
        private readonly int maxQueueSize = 100;
        private bool isSending = false;

        public AnalyticsModule(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.eventQueue = new Queue<GameEvent>();
        }

        public void TrackEvent(string eventType, Dictionary<string, object> eventData)
        {
            var gameEvent = new GameEvent
            {
                EventType = eventType,
                EventData = eventData,
                Timestamp = DateTime.UtcNow
            };

            eventQueue.Enqueue(gameEvent);

            if (eventQueue.Count >= maxQueueSize)
            {
                _ = SendEvents();
            }
        }

        private async Task SendEvents()
        {
            if (isSending || eventQueue.Count == 0)
                return;

            isSending = true;

            try
            {
                var events = new List<GameEvent>();
                while (eventQueue.Count > 0 && events.Count < maxQueueSize)
                {
                    events.Add(eventQueue.Dequeue());
                }

                await httpClient.Post<object>("/v1/analytics/events", new { events });
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to send analytics events: {e.Message}");
                // Re-queue failed events
                foreach (var evt in eventQueue)
                {
                    eventQueue.Enqueue(evt);
                }
            }
            finally
            {
                isSending = false;
            }
        }
    }
}