using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

using Newtonsoft.Json;
using Stannieman.HttpQueries;
using JhDeStip.Laguna.Domain;

namespace JhDeStip.Laguna.Dal
{
    /// <summary>
    /// Playlist service.
    /// This service can be used to add/remove/... items to/from a playlist etc.
    /// </summary>
    public class PlaylistService : IPlaylistService
    {
        #region Instance variables and constants

        private const string AmountParameterName = "amount";

        private HttpClient _client;
        private DalConfig _config;

        #endregion

        /// <summary>
        /// Constructor taking the required dependencies.
        /// </summary>
        public PlaylistService(DalConfig config)
        {
            _config = config;
            _client = new HttpClient { Timeout = _config.Timeout };
        }

        /// <summary>
        /// Gets all items in the user queue.
        /// </summary>
        /// <returns>A list of items in the user queue.</returns>
        public async Task<IList<PlayableItemInfo>> GetUserQueueItemsAsync()
        {
            try
            {
                // Build uri
                var uriBuilder = new UriBuilder(_config.ServerHost);
                uriBuilder.Port = _config.ServerPort;
                uriBuilder.Path = _config.ApiBasePath + _config.UserQueueItemsPath;

                // Get a response from the API
                var response = await _client.GetAsync(uriBuilder.Uri);

                if (response.IsSuccessStatusCode)
                    // If the request was successful we create a list of playable items from the response
                    return await Task.Run(async () => JsonConvert.DeserializeObject<IList<PlayableItemInfo>>(await response.Content.ReadAsStringAsync()));
                else
                    throw new ServerCommunicationException($"We got response code {response.StatusCode} from the server.");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Adds an item to the user queue.
        /// </summary>
        /// <param name="newItem">PlayableItemInfo instance to add to the user queue.</param>
        public async Task AddItemToQueueAsync(PlayableItemInfo newItem)
        {
            try
            {
                // Build uri
                var uriBuilder = new UriBuilder(_config.ServerHost);
                uriBuilder.Port = _config.ServerPort;
                uriBuilder.Path = _config.ApiBasePath + _config.UserQueueItemsPath;

                // Create a new HTTP body from the CheckInRequest instance
                string body = await Task.Run(() => JsonConvert.SerializeObject(newItem));

                // Get a response from the API
                var response = await _client.PostAsync(uriBuilder.Uri, new StringContent(body, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.Conflict)
                        throw new TrackAlreadyInQueueException();
                    else
                        throw new ServerCommunicationException($"We got response code {response.StatusCode} from the server.");
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Removes the given item from the user queue.
        /// </summary>
        /// <param name="playableItemInfo">PlayableItemInfo instance to remove.</param>
        public async Task RemoveUserQueueItemAsync(PlayableItemInfo playableItemInfo)
        {
            try
            {
                // Build uri
                var uriBuilder = new UriBuilder(_config.ServerHost);
                uriBuilder.Port = _config.ServerPort;
                uriBuilder.Path = _config.ApiBasePath + _config.UserQueueItemsPath + playableItemInfo.ItemId;

                // Do API call to delete item
                await _client.DeleteAsync(uriBuilder.Uri);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns the item that is currently playing, null if no item is playing.
        /// </summary>
        /// <returns>The item that is currently playing.</returns>
        public async Task<PlayableItemInfo> GetNowPlayingItemAsync()
        {
            try
            {
                // Build uri
                var uriBuilder = new UriBuilder(_config.ServerHost);
                uriBuilder.Port = _config.ServerPort;
                uriBuilder.Path = _config.ApiBasePath + _config.NowPlayingPath;

                // Get a response from the API
                var response = await _client.GetAsync(uriBuilder.Uri);

                if (response.IsSuccessStatusCode)
                    // If the request was successful we create a playable item from the response
                    return await Task.Run(async () => JsonConvert.DeserializeObject<PlayableItemInfo>(await response.Content.ReadAsStringAsync()));
                else if (response.StatusCode == HttpStatusCode.NoContent)
                    return null;
                else
                    throw new ServerCommunicationException($"We got response code {response.StatusCode} from the server.");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns all items in the fallback playlist.
        /// </summary>
        /// <returns>All items in the fallback playlist.</returns>
        public async Task<IList<PlayableItemInfo>> GetFallbackPlaylistItemsAsync()
        {
            try
            {
                // Build uri
                var uriBuilder = new UriBuilder(_config.ServerHost);
                uriBuilder.Port = _config.ServerPort;
                uriBuilder.Path = _config.ApiBasePath + _config.FallbackPlaylistItemsPath;

                // Get a response from the API
                var response = await _client.GetAsync(uriBuilder.Uri);

                if (response.IsSuccessStatusCode)
                    // If the request was successful we create a list of playable items from the response
                    return await Task.Run(async () => JsonConvert.DeserializeObject<IList<PlayableItemInfo>>(await response.Content.ReadAsStringAsync()));
                else
                    throw new ServerCommunicationException($"We got response code {response.StatusCode} from the server.");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns the next items to play.
        /// </summary>
        /// <param name="amount">Amount of items to return.</param>
        /// <returns>A list of next items to play.</returns>
        public async Task<IList<PlayableItemInfo>> GetNextNItemsAsync(int amount)
        {
            // Build query
            Query query = new Query(AmountParameterName, amount);

            try
            {
                // Build uri
                var uriBuilder = new UriBuilder(_config.ServerHost);
                uriBuilder.Port = _config.ServerPort;
                uriBuilder.Path = _config.ApiBasePath + _config.NextNItemsPath;
                uriBuilder.Query = query.QueryString;

                // Get a response from the API
                var response = await _client.GetAsync(uriBuilder.Uri);

                if (response.IsSuccessStatusCode)
                    // If the request was successful we create a list of playable items from the response
                    return await Task.Run(async () => JsonConvert.DeserializeObject<IList<PlayableItemInfo>>(await response.Content.ReadAsStringAsync()));
                else
                    throw new ServerCommunicationException($"We got response code {response.StatusCode} from the server.");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns the next item to play and sets it as now playing.
        /// Returns null if there is no next item.
        /// </summary>
        /// <returns>The next item to play.</returns>
        public async Task<PlayableItemInfo> GetNextAndSetAsNowPlayingAsync()
        {
            try
            {
                // Build uri
                var uriBuilder = new UriBuilder(_config.ServerHost);
                uriBuilder.Port = _config.ServerPort;
                uriBuilder.Path = _config.ApiBasePath + _config.GetNextAndSetAsNowPlayingPath;

                // Get a response from the API
                var response = await _client.GetAsync(uriBuilder.Uri);

                if (response.IsSuccessStatusCode)
                    // If the request was successful we create a playable item from the response
                    return await Task.Run(async () => JsonConvert.DeserializeObject<PlayableItemInfo>(await response.Content.ReadAsStringAsync()));
                else if (response.StatusCode == HttpStatusCode.NoContent)
                    return null;
                else
                    throw new ServerCommunicationException($"We got response code {response.StatusCode} from the server.");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Refreshes the fallback playlist on the server.
        /// </summary>
        public async Task RefreshFallbackPlaylistOnServerAsync()
        {
            try
            {
                // Build uri
                var uriBuilder = new UriBuilder(_config.ServerHost);
                uriBuilder.Port = _config.ServerPort;
                uriBuilder.Path = _config.ApiBasePath + _config.RefreshFallbackPlaylistOnServerPath;

                // Get a response from the API
                var response = await _client.GetAsync(uriBuilder.Uri);

                if (!response.IsSuccessStatusCode)
                    throw new ServerCommunicationException($"We got response code {response.StatusCode} from the server.");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Disposes the service.
        /// </summary>
        public void Dispose()
        {
            // Dispose http client
            _client.Dispose();
        }
    }
}