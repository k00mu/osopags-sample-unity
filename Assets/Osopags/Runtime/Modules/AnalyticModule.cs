using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Osopags.Models;
using Osopags.Core;

namespace Osopags.Modules
{
    public class AnalyticModule
    {
        private readonly Queue<OfflineTrackRequest> offlineTrackQueue;

        public AnalyticModule()
        {
            offlineTrackQueue = new Queue<OfflineTrackRequest>();
        }

        public void Track(
            string eventType,
            SuccessDelegate<TrackResponse> onSuccess = null,
            ErrorDelegate<ErrorResponse> onError = null
        )
        {
            _ = TrackInternal(new TrackRequest { eventType = eventType }, onSuccess, onError);
        }

        public void TrackWithData(
            TrackWithDataRequest request,
            SuccessDelegate<TrackWithDataResponse> onSuccess = null,
            ErrorDelegate<ErrorResponse> onError = null
        )
        {
            _ = TrackWithDataInternal(request, onSuccess, onError);
        }

        private async Task TrackInternal(
            TrackRequest request,
            SuccessDelegate<TrackResponse> onSuccess = null,
            ErrorDelegate<ErrorResponse> onError = null
        )
        {
            if (OsopagsHttpClient.IsOnline())
            {
                try
                {
                    var response = await OsopagsHttpClient
                    .Post<SuccessResponse<TrackResponse>>(
                        "/v1/analytic/tracks",
                        request
                    );

                    onSuccess?.Invoke(response.data);
                }
                catch (Exception ex)
                {
                    onError?.Invoke(new ErrorResponse { message = ex.Message });
                }
            }

            // TODO: if trackWhileOffline is true, we will store the event in the offline queue
            offlineTrackQueue.Enqueue(new OfflineTrackRequest
            {
                EventType = request.eventType,
                Timestamp = DateTime.UtcNow
            });
        }

        private async Task TrackWithDataInternal(
            TrackWithDataRequest request,
            SuccessDelegate<TrackWithDataResponse> onSuccess = null,
            ErrorDelegate<ErrorResponse> onError = null
        )
        {
            if (OsopagsHttpClient.IsOnline())
            {
                try
                {
                    var response = await OsopagsHttpClient
                    .Post<SuccessResponse<TrackWithDataResponse>>(
                        "/v1/analytic/tracks",
                        request
                    );

                    onSuccess?.Invoke(response.data);
                }
                catch (Exception ex)
                {
                    onError?.Invoke(new ErrorResponse { message = ex.Message });
                }
            }

            // TODO: if trackWhileOffline is true, we will store the event in the offline queue
            offlineTrackQueue.Enqueue(new OfflineTrackRequest
            {
                EventType = request.eventType,
                EventData = request.eventData,
                Timestamp = DateTime.UtcNow
            });
        }
    }
}