using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using JhDeStip.Laguna.Server.Domain;

namespace JhDeStip.Laguna.Server.Dal
{
    /// <summary>
    /// Interface describing a service to access the Youtube API.
    /// </summary>
    public interface IYoutubeService
    {
        /// <summary>
        /// Get search results from Youtube for a given query.
        /// </summary>
        /// <param name="query">Search query.</param>
        /// <returns>List of PlayableItemInfo instances for the search results.</returns>
        Task<IList<PlayableItemInfo>> GetSearchResultsAsync(string query);

        /// <summary>
        /// Get the tracks from in the fallback playlist.
        /// </summary>
        /// <returns>List of PlayableItemInfo instances for the tracks in the fallback playlist.</returns>
        Task<List<PlayableItemInfo>> GetFallbackPlaylistItemsAsync();
    }

    public class YoutubeException : Exception { }
    public class TrackNotAvailableException : YoutubeException { }
}
