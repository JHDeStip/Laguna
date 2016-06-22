using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using JhDeStip.Laguna.Domain;

/// <summary>
/// Interface describing a track search service.
/// The track search service can be used to search for tracks.
/// </summary>
namespace JhDeStip.Laguna.Dal
{
    public interface ITrackSearchService : IDisposable
    {
        /// <summary>
        /// Returns a list of PlayableItemInfo instances for which the properties match the given search query.
        /// </summary>
        /// <param name="query">Query to get search results for.</param>
        /// <returns>A list of PlayableItemInfo instances.</returns>
        Task<IList<PlayableItemInfo>> SearchItemsAsync(string query);
    }
}
