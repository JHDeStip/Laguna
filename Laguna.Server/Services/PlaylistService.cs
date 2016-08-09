using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JhDeStip.Laguna.Server.Domain;

namespace JhDeStip.Laguna.Server.Services
{
    public class PlaylistService : IPlaylistService
    {
        private List<PlayableItemInfo> _userQueue;
        private List<PlayableItemInfo> _fallbackPlaylist;
        private PlayableItemInfo _nowPlaying;
        private object _lock = new object();

        private string _lastVideoIdFromFallbackPlaylist;

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlaylistService()
        {
            _userQueue = new List<PlayableItemInfo>();
            _fallbackPlaylist = new List<PlayableItemInfo>();
        }

        /// <summary>
        /// Adds a given item to the user queue.
        /// </summary>
        /// <param name="newPlayableItemInfo">Item to add to the queue.</param>
        /// <returns>Task</returns>
        public async Task AddToUserQueueAsync(PlayableItemInfo newPlayableItemInfo)
        {
            await Task.Run(() =>
            {
                // If the item that's added equels the one that's now playing of it's already in the list: throw exception, otherwise add to user queue
                lock (_lock)
                {
                    if (_userQueue.Contains(newPlayableItemInfo) || (_nowPlaying != null && newPlayableItemInfo.Equals(_nowPlaying)))
                        throw new TrackAlreadyInQueueException();

                    _userQueue.Add(newPlayableItemInfo);
                }
            });
        }

        /// <summary>
        /// Returns a shallow copy of the user queue.
        /// </summary>
        /// <returns>Task with shallow copy of user queue.</returns>
        public async Task<IEnumerable<PlayableItemInfo>> GetAllFromUserQueueAsync()
        {
            return await Task.Run(() =>
            {
                lock (_lock)
                    return new List<PlayableItemInfo>(_userQueue);
            });
        }

        /// <summary>
        /// Returns a shallow copy of the fallback playlist.
        /// </summary>
        /// <returns>Task with shallow copy of fallback list.</returns>
        public async Task<IEnumerable<PlayableItemInfo>> GetAllFromFallbackPlaylistAsync()
        {
            return await Task.Run(() =>
            {
                lock (_lock)
                    return new List<PlayableItemInfo>(_fallbackPlaylist);
            });
        }

        /// <summary>
        /// Sets the given list as fallback playlist.
        /// </summary>
        /// <param name="fallbackPlaylist">List to set as new fallback playlist.</param>
        /// <returns>Task</returns>
        public async Task SetFallbackPlaylistAsync(List<PlayableItemInfo> fallbackPlaylist)
        {
            await Task.Run(() =>
            {
                // Set new list as fallback list
                _fallbackPlaylist = fallbackPlaylist;
                // Fast forward new list to where we were
                FastForwardFallbackPlaylist();
            });
        }

        /// <summary>
        /// Gets the next n items to play.
        /// </summary>
        /// <param name="n">Amount of next items to return.</param>
        /// <returns>Task with next n items to play.</returns>
        public async Task<IEnumerable<PlayableItemInfo>> GetNextNAsync(int n)
        {
            return await Task.Run(() =>
            {
                List<PlayableItemInfo> returnList = new List<PlayableItemInfo>();

                lock (_lock)
                {
                    // First take items from the user queue if there are any
                    if (_userQueue.Count > 0)
                    {
                        // If the user queue contains contains enough items, take these
                        if (_userQueue.Count >= n)
                            returnList.AddRange(_userQueue.GetRange(0, n));
                        else
                            // If the user queue doesn't contain enough items take all there is
                            returnList.AddRange(_userQueue.GetRange(0, _userQueue.Count));
                    }

                    // If there are items in the fallback list we can start taking the remaining items from there
                    if (_fallbackPlaylist.Count > 0)
                    {
                        // Calculate how much we still need
                        int nRemain = n - returnList.Count;

                        while (nRemain > 0)
                        {
                            // If the fallback list contains enough items, take these
                            if (_fallbackPlaylist.Count >= nRemain)
                                returnList.AddRange(_fallbackPlaylist.GetRange(0, nRemain));
                            else
                                // If there aren't enough items take all there is
                                returnList.AddRange(_fallbackPlaylist.GetRange(0, _fallbackPlaylist.Count));

                            // Update amount remaining
                            nRemain = n - returnList.Count;
                        }
                    }
                }

                return returnList;
            });
        }

        /// <summary>
        /// Removes the item with the given id from the user queue.
        /// </summary>
        /// <param name="playableItemId">Id of item to remove from the queue.</param>
        /// <returns>Task</returns>
        public async Task RemoveFromUserQueueAsync(string playableItemId)
        {
            await Task.Run(() =>
            {
                lock (_lock)
                {
                    var itemToRemove = _userQueue.FirstOrDefault(s => s.ItemId == playableItemId);
                    if (itemToRemove != null)
                        _userQueue.Remove(itemToRemove);
                    else
                        throw new TrackNotInQueueException($"There is no track with ID {playableItemId} in the use queue.");
                }
            });
        }

        /// <summary>
        /// Gets the next item to play and sets it as now playing.
        /// </summary>
        /// <returns>Task with the next item to play.</returns>
        public async Task<PlayableItemInfo> GetNextAndSetAsNowPlayingAsync()
        {
            return await Task.Run(() =>
            {
                lock (_lock)
                {
                    // If there is an item in the user queue: set this as now playing and remove it from the queue
                    if (_userQueue.Count > 0)
                    {
                        _nowPlaying = _userQueue[0];
                        _userQueue.RemoveAt(0);
                    }
                    // If there is nothing in the user queue but there is in the fallback list: set the first as now playing and rotate the list
                    else if (_fallbackPlaylist.Count > 0)
                    {
                        _nowPlaying = _fallbackPlaylist.First();
                        _lastVideoIdFromFallbackPlaylist = _nowPlaying.ItemId;
                        RotateFallbackPlaylist();
                    }
                    // If there are no tracks, set nowplaying to null
                    else
                        _nowPlaying = null;

                    // Finally return the track that should play now
                    return _nowPlaying;
                }
            });
        }

        /// <summary>
        /// Returns the now playing item.
        /// </summary>
        /// <returns>Task with item that's set as now playing.</returns>
        public async Task<PlayableItemInfo> GetNowPlayingAsync()
        {
            return await Task.Run(() =>
            {
                lock (_lock)
                    return _nowPlaying;
            });
        }

        /// <summary>
        /// Fast forwards the fallback list to after the last played item.
        /// </summary>
        private void FastForwardFallbackPlaylist()
        {
            lock (_lock)
            {
                // Check if there is a last played id (otherwise we have not yet played an item from the fallback list).
                if (_lastVideoIdFromFallbackPlaylist != null)
                    // Search for the index of the last played video, and if found rotate the list 1 more
                    for (int i = 0; i < _fallbackPlaylist.Count; i++)
                        if (_fallbackPlaylist[i].ItemId == _lastVideoIdFromFallbackPlaylist)
                            for (int j = 0; j <= i; j++)
                                RotateFallbackPlaylist();
            }
        }

        /// <summary>
        /// Rotates the fallback playlist so the first item is set as last item and the next item comes on top if the list.
        /// </summary>
        private void RotateFallbackPlaylist()
        {
            lock (_lock)
            {
                // If there are elements in the fallback list, add the first item to the end and remove the first item
                if (_fallbackPlaylist.Count > 0)
                {
                    _fallbackPlaylist.Add(_fallbackPlaylist.First());
                    _fallbackPlaylist.RemoveAt(0);
                }
            }
        }
    }
}
