using System;

namespace JhDeStip.Laguna.Dal
{
    /// <summary>
    /// Class containing properties with configuration values.
    /// All properties have a default value for in case they are not set explicitly in the application's config.
    /// </summary>
    public class DalConfig
    {
        /// <summary>
        /// Host of the server that provides the REST API.
        /// </summary>
        public string ServerHost { get; set; } = String.Empty;

        /// <summary>
        /// Port of the server where the REST API is provided.
        /// </summary>
        public int ServerPort { get; set; }

        /// <summary>
        /// Time in milliseconds after which the connection times out.
        /// </summary>
        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 0, 100);

        /// <summary>
        /// Base path of the API.
        /// </summary>
        public string ApiBasePath { get; set; } = String.Empty;

        /// <summary>
        /// API path for the track search call.
        /// </summary>
        public string TrackSearchPath { get; set; } = String.Empty;

        /// <summary>
        /// API path for the now playing call.
        /// </summary>
        public string NowPlayingPath { get; set; } = String.Empty;

        /// <summary>
        /// API path for the user queue items call.
        /// </summary>
        public string UserQueueItemsPath { get; set; } = String.Empty;

        /// <summary>
        /// API path for the next n items call.
        /// </summary>
        public string NextNItemsPath { get; set; } = String.Empty;

        /// <summary>
        /// API path for the get next and set as now playing call.
        /// </summary>
        public string GetNextAndSetAsNowPlayingPath { get; set; } = String.Empty;

        /// <summary>
        /// API path for the fallback playlist items call.
        /// </summary>
        public string FallbackPlaylistItemsPath { get; set; } = String.Empty;

        /// <summary>
        /// API path for the call to make the service available.
        /// </summary>
        public string SetServiceAvailablePath { get; set; } = String.Empty;

        /// <summary>
        /// API path for the call to make the service unavailable.
        /// </summary>
        public string SetServiceUnavailablePath { get; set; } = String.Empty;

        /// <summary>
        /// API path for the call to get the availability of the service.
        /// </summary>
        public string ServiceAvailabilityPath { get; set; } = String.Empty;

        /// <summary>
        /// API path for the call to refresh the fallback playlist on the server.
        /// </summary>
        public string RefreshFallbackPlaylistOnServerPath { get; set; } = String.Empty;
    }
}
