namespace JhDeStip.Laguna.Server.Dal
{
    /// <summary>
    /// Class containing settings for the Youtube service.
    /// </summary>
    public class YoutubeServiceConfig
    {
        /// <summary>
        /// API key for the Youtube API.
        /// </summary>
        public string YoutubeApiKey { get; set; }

        /// <summary>
        /// URL of the Youtube API's search endpoint.
        /// </summary>
        public string YoutubeSearchEndpoint { get; set; }

        /// <summary>
        /// URL of the Youtube API's playlist items endpoint.
        /// </summary>
        public string YoutubePlaylistItemsEndpoint { get; set; }

        /// <summary>
        /// URL of the Youtube API's video endpoint.
        /// </summary>
        public string YoutubeVideoEndpoint { get; set; }

        /// <summary>
        /// Id of the Music channel. The Music category is really an auto-generated channel.
        /// </summary>
        public string MusicCategoryId { get; set; }

        /// <summary>
        /// Maximum number of search results to get from Youtube.
        /// </summary>
        public string MaxSearchResults { get; set; }

        /// <summary>
        /// Id of the fallback playlist.
        /// </summary>
        public string FallbackPlaylistId { get; set; }

        /// <summary>
        /// Setting for safe search. Possible values are moderate, none and strict.
        /// </summary>
        public string SafeSearch { get; set; }

        /// <summary>
        /// Duration of the video. Possible values are any, long, medium and short.
        /// </summary>
        public string VideoDuration { get; set; }
    }
}
