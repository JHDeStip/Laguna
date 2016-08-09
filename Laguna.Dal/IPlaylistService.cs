using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using JhDeStip.Laguna.Domain;

namespace JhDeStip.Laguna.Dal
{
    /// <summary>
    /// Interface describing a service to work with playlists.
    /// The playlist service can be used to add/remove/... items to/from a playlist etc.
    /// </summary>
    public interface IPlaylistService : IDisposable
    {
        /// <summary>
        /// Gets all items in the user queue.
        /// </summary>
        /// <returns>A list of items in the user queue.</returns>
        Task<IList<PlayableItemInfo>> GetUserQueueItemsAsync();

        /// <summary>
        /// Adds an item to the user queue.
        /// </summary>
        /// <param name="newItem">PlayableItemInfo instance to add to the user queue.</param>
        Task AddItemToQueueAsync(PlayableItemInfo newItem);

        /// <summary>
        /// Removes the given item from the user queue.
        /// </summary>
        /// <param name="playableItemInfo">PlayableItemInfo instance to remove.</param>
        Task RemoveUserQueueItemAsync(PlayableItemInfo playableItemInfo);

        /// <summary>
        /// Returns the item that is currently playing.
        /// </summary>
        /// <returns>The item that is currently playing.</returns>
        Task<PlayableItemInfo> GetNowPlayingItemAsync();

        /// <summary>
        /// Returns all items in the fallback playlist.
        /// </summary>
        /// <returns>All items in the fallback playlist.</returns>
        Task<IList<PlayableItemInfo>> GetFallbackPlaylistItemsAsync();

        /// <summary>
        /// Returns the next items to play.
        /// </summary>
        /// <param name="amount">Amount of items to return.</param>
        /// <returns>A list of next items to play.</returns>
        Task<IList<PlayableItemInfo>> GetNextNItemsAsync(int amount);

        /// <summary>
        /// Returns the next item to play and sets it as now playing.
        /// </summary>
        /// <returns>The next item to play.</returns>
        Task<PlayableItemInfo> GetNextAndSetAsNowPlayingAsync();

        /// <summary>
        /// Refreshes the fallback playlist on the server.
        /// </summary>
        Task RefreshFallbackPlaylistOnServerAsync();
    }
}
