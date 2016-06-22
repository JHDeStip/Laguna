using System;

namespace JhDeStip.Laguna.Dal
{
    /// <summary>
    /// Class containing properties with configuration values.
    /// All properties have a default value for in case they are not set explicitly in the application's config.
    /// </summary>
    public class DalConfig
    {
        private string _serverHost;
        /// <summary>
        /// Host of the server that provides the REST API.
        /// </summary>
        public string ServerHost
        {
            get { return _serverHost; }
            set
            {
                // Set default value if no value is given
                if (value == null)
                    value = "http://localhost";

                _serverHost = value;
            }
        }

        /// <summary>
        /// Port of the server where the REST API is provided.
        /// </summary>
        public int ServerPort { get; set; }

        /// <summary>
        /// Time in milliseconds after which the connection times out.
        /// </summary>
        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 0, 100);

        private string _APIBasePath;
        /// <summary>
        /// Base path of the API.
        /// </summary>
        public string APIBasePath
        {
            get { return _APIBasePath; }
            set
            {
                // Set default value if no value is given
                if (value == null)
                    value = "api/";

                _APIBasePath = value;
            }
        }

        private string _trackSearchPath;
        /// <summary>
        /// API path for the track search call.
        /// </summary>
        public string TrackSearchPath
        {
            get { return _trackSearchPath; }
            set
            {
                // Set default value if no value is given
                if (value == null)
                    value = "trackSearch/";

                _trackSearchPath = value;
            }
        }

        private string _nowPlayingPath;
        /// <summary>
        /// API path for the now playing call.
        /// </summary>
        public string NowPlayingPath
        {
            get { return _nowPlayingPath; }
            set
            {
                // Set default value if no value is given
                if (value == null)
                    value = "playlist/nowPlaying/";

                _nowPlayingPath = value;
            }
        }

        private string _userQueueItemsPath;
        /// <summary>
        /// API path for the user queue items call.
        /// </summary>
        public string UserQueueItemsPath
        {
            get { return _userQueueItemsPath; }
            set
            {
                // Set default value if no value is given
                if (value == null)
                    value = "playlist/userQueue/items/";

                _userQueueItemsPath = value;
            }
        }

        private string _nextNItemsPath;
        /// <summary>
        /// API path for the next n items call.
        /// </summary>
        public string NextNItemsPath
        {
            get { return _nextNItemsPath; }
            set
            {
                // Set default value if no value is given
                if (value == null)
                    value = "playlist/nextN/";

                _nextNItemsPath = value;
            }
        }

        private string _getNextAndSetAsNowPlayingPath;
        /// <summary>
        /// API path for the get next and set as now playing call.
        /// </summary>
        public string GetNextAndSetAsNowPlayingPath
        {
            get { return _getNextAndSetAsNowPlayingPath; }
            set
            {
                // Set default value if no value is given
                if (value == null)
                    value = "playlist/getNextAndSetAsNowPlaying/";

                _getNextAndSetAsNowPlayingPath = value;
            }
        }

        private string _fallbackPlaylistItemsPath;
        /// <summary>
        /// API path for the fallback playlist items call.
        /// </summary>
        public string FallbackPlaylistItemsPath
        {
            get { return _fallbackPlaylistItemsPath; }
            set
            {
                // Set default value if no value is given
                if (value == null)
                    value = "playlist/fallbackPlaylist/items/";

                _fallbackPlaylistItemsPath = value;
            }
        }

        private string _setServiceAvailablePath;
        /// <summary>
        /// API path for the call to make the service available.
        /// </summary>
        public string SetServiceAvailablePath
        {
            get { return _setServiceAvailablePath; }
            set
            {
                // Set default value if no value is given
                if (value == null)
                    value = "serviceAvailability/setAvailable/";

                _setServiceAvailablePath = value;
            }
        }

        private string _setServiceUnavailablePath;
        /// <summary>
        /// API path for the call to make the service unavailable.
        /// </summary>
        public string SetServiceUnavailablePath
        {
            get { return _setServiceUnavailablePath; }
            set
            {
                // Set default value if no value is given
                if (value == null)
                    value = "serviceAvailability/setUnavailable/";

                _setServiceUnavailablePath = value;
            }
        }

        private string _serviceAvailabilityPath;
        /// <summary>
        /// API path for the call to get the availability of the service.
        /// </summary>
        public string ServiceAvailabilityPath
        {
            get { return _serviceAvailabilityPath; }
            set
            {
                // Set default value if no value is given
                if (value == null)
                    value = "serviceAvailability/";

                _serviceAvailabilityPath = value;
            }
        }

        private string _refreshFallbackPlaylistOnServerPath;
        /// <summary>
        /// API path for the call to refresh the fallback playlist on the server.
        /// </summary>
        public string RefreshFallbackPlaylistOnServerPath
        {
            get { return _refreshFallbackPlaylistOnServerPath; }
            set
            {
                // Set default value if no value is given
                if (value == null)
                    value = "playlist/fallbackPlaylist/refresh/";

                _refreshFallbackPlaylistOnServerPath = value;
            }
        }
    }
}
