using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Track search service.
    /// This service can be used to search for tracks.
    /// </summary>
    public class TrackSearchService : ITrackSearchService
    {
        #region Instance variables and constants

        private const string QueryParameterName = "query";

        private HttpClient _client;
        private DalConfig _config;

        #endregion

        /// <summary>
        /// Constructor taking the required dependencies.
        /// </summary>
        public TrackSearchService(DalConfig config)
        {
            _config = config;
            _client = new HttpClient { Timeout = _config.Timeout };
        }

        /// <summary>
        /// Returns a list of PlayableItemInfo instances for which the properties match the given search query.
        /// </summary>
        /// <param name="query">Query to get search results for.</param>
        /// <returns>A list of PlayableItemInfo instances.</returns>
        public async Task<IList<PlayableItemInfo>> SearchItemsAsync(string query)
        {
            // Build query
            Query httpQuery = new Query(QueryParameterName, query);

            try
            {
                // Build uri
                var uriBuilder = new UriBuilder(_config.ServerHost);
                uriBuilder.Port = _config.ServerPort;
                uriBuilder.Path = _config.APIBasePath + _config.TrackSearchPath;
                uriBuilder.Query = httpQuery.QueryString;

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
        /// Disposes the service.
        /// </summary>
        public void Dispose()
        {
            // Dispose http client
            _client.Dispose();
        }
    }
}
