using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;
using JhDeStip.Laguna.Server.Dal.Converters;
using JhDeStip.Laguna.Server.Domain;


namespace JhDeStip.Laguna.Server.Dal
{
    /// <summary>
    /// IYoutubeService implementation.
    /// </summary>
    public class YoutubeService : IYoutubeService
    {
        private const string YoutubeApiKeySettingKey = "youtubeAPIKey";

        private const string KeyParameterName = "key";
        private const string TypeParameterName = "type";
        private const string PartParameterName = "part";
        private const string SafeSearchParameterName = "safeSearch";
        private const string VideoDurationParameterName = "videoDuration";
        private const string MaxResultsParameterName = "maxResults";
        private const string QueryParameterName = "q";
        private const string IdParameterName = "id";
        private const string FieldsParameterName = "fields";
        private const string PlaylistIdParameterName = "playlistId";
        private const string PageTokenParameterName = "pageToken";
        private const string CategoryIdParameterName = "videoCategoryId";

        private const string TypeParameterValue = "video";

        private const string SearchPartParameterValue = "snippet";
        private const string SearchFieldsParameterValue = "items(id(videoId),snippet(title,thumbnails(default(url))))";

        private const string PlaylistPartParameterValue = "snippet,contentDetails";
        private const string PlaylistFieldsParameterValue = "nextPageToken,items(contentDetails(videoId),snippet(title,thumbnails(default(url))))";
        private const string PlaylistMaxResults = "50";

        private const string VideoDetailsPartParameterValue = "contentDetails,status";
        private const string VideoDetailsFieldsParameterValue = "items(contentDetails(duration),status(uploadStatus))";
        private const string VideoAvailableUploadStatus = "processed";

        private HttpClient _client;
        private IOptions<YoutubeServiceConfig> _config;

        /// <summary>
        /// Constructor accepting the require dependencies.
        /// </summary>
        /// <param name="config">Configuration.</param>
        public YoutubeService(IOptions<YoutubeServiceConfig> config)
        {
            _config = config;
            _client = new HttpClient();
        }

        /// <summary>
        /// Get search results from Youtube for a given query.
        /// </summary>
        /// <param name="query">Search query.</param>
        /// <returns>List of PlayableItemInfo instances for the search results.</returns>
        public async Task<IList<PlayableItemInfo>> GetSearchResultsAsync(string query)
        {
            return await Task.Run(async () =>
            {
                IList<PlayableItemInfo> searchResultItems = await GetSearchSnippetPartsAsync(query);

                int count = searchResultItems.Count;
                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        await GetAdditionalVideoDetailsAsync(searchResultItems[i]);
                    }
                    catch
                    {
                        searchResultItems.RemoveAt(i);
                        count--;
                        i--;
                    }
                }

                return searchResultItems;
            });
        }

        /// <summary>
        /// Get the tracks from in the fallback playlist.
        /// </summary>
        /// <returns>List of PlayableItemInfo instances for the tracks in the fallback playlist.</returns>
        public async Task<List<PlayableItemInfo>> GetFallbackPlaylistItemsAsync()
        {
            return await Task.Run(async () =>
            {
                List<PlayableItemInfo> fallbackPlaylistItems = await GetPlaylistItemsSnippetPartsAsync(_config.Value.FallbackPlaylistId);

                int count = fallbackPlaylistItems.Count;
                for (int i=0; i<count; i++)
                {
                    try
                    {
                        await GetAdditionalVideoDetailsAsync(fallbackPlaylistItems[i]);
                    }
                    catch
                    {
                        fallbackPlaylistItems.RemoveAt(i);
                        count--;
                        i--;
                    }
                }

                return fallbackPlaylistItems;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        private async Task<IList<PlayableItemInfo>> GetSearchSnippetPartsAsync(string keyword)
        {
            var searchResultItems = new List<PlayableItemInfo>();

            // Create a list with all names and values of the get parameters
            var paramList = new List<KeyValuePair<string, string>>();
            paramList.Add(new KeyValuePair<string, string>(KeyParameterName, _config.Value.YoutubeApiKey));
            paramList.Add(new KeyValuePair<string, string>(PartParameterName, SearchPartParameterValue));
            paramList.Add(new KeyValuePair<string, string>(TypeParameterName, TypeParameterValue));
            //paramList.Add(new KeyValuePair<string, string>(CATEGORY_ID_PARAM_NAME, _config.Value.MusicCatetoryId));
            paramList.Add(new KeyValuePair<string, string>(MaxResultsParameterName, _config.Value.MaxSearchResults));
            paramList.Add(new KeyValuePair<string, string>(SafeSearchParameterName, _config.Value.SafeSearch));
            paramList.Add(new KeyValuePair<string, string>(VideoDurationParameterName, _config.Value.VideoDuration));
            paramList.Add(new KeyValuePair<string, string>(QueryParameterName, keyword));
            paramList.Add(new KeyValuePair<string, string>(FieldsParameterName, SearchFieldsParameterValue));
            // Get a string of all get parameters
            string paramString = BuildGetParamatersString(paramList);

            try
            {
                var uriBuilder = new UriBuilder(_config.Value.YoutubeSearchEndpoint);
                uriBuilder.Query = paramString;

                // Get a response from the API
                var response = await _client.GetAsync(uriBuilder.Uri);

                if (response.IsSuccessStatusCode)
                {
                    // If the request was successful we create a CheckInResult from the response
                    var contentStr = await response.Content.ReadAsStringAsync();
                    dynamic contentObj = await Task.Run(() => JsonConvert.DeserializeObject(contentStr));
                    foreach (dynamic video in contentObj.items)
                        searchResultItems.Add(new PlayableItemInfo { ItemId = video.id.videoId, Title = video.snippet.title, ThumbnailUrl = video.snippet.thumbnails.@default.url });
                }
            }
            catch { }

            return searchResultItems;
        }

        private async Task<List<PlayableItemInfo>> GetPlaylistItemsSnippetPartsAsync(string playlistId)
        {
            var playlistItems = new List<PlayableItemInfo>();

            // Create a list with all names and values of the get parameters
            var paramList = new List<KeyValuePair<string, string>>();
            paramList.Add(new KeyValuePair<string, string>(KeyParameterName, _config.Value.YoutubeApiKey));
            paramList.Add(new KeyValuePair<string, string>(PlaylistIdParameterName, _config.Value.FallbackPlaylistId));
            paramList.Add(new KeyValuePair<string, string>(TypeParameterName, TypeParameterValue));
            paramList.Add(new KeyValuePair<string, string>(PartParameterName, PlaylistPartParameterValue));
            paramList.Add(new KeyValuePair<string, string>(FieldsParameterName, PlaylistFieldsParameterValue));
            paramList.Add(new KeyValuePair<string, string>(MaxResultsParameterName, PlaylistMaxResults));

            string nextPageToken = null;

            try
            {
                do
                {
                    if (nextPageToken != null)
                        paramList.Add(new KeyValuePair<string, string>(PageTokenParameterName, nextPageToken));

                    // Get a string of all get parameters
                    string paramString = BuildGetParamatersString(paramList);

                    var uriBuilder = new UriBuilder(_config.Value.YoutubePlaylistItemsEndpoint);
                    uriBuilder.Query = paramString;

                    // Get a response from the API
                    var response = await _client.GetAsync(uriBuilder.Uri);

                    if (response.IsSuccessStatusCode)
                    {
                        // If the request was successful we create a CheckInResult from the response
                        var contentStr = await response.Content.ReadAsStringAsync();
                        dynamic contentObj = await Task.Run(() => JsonConvert.DeserializeObject(contentStr));

                        foreach (dynamic video in contentObj.items)
                        {
                            try
                            {
                                playlistItems.Add(new PlayableItemInfo { ItemId = (string)video.contentDetails.videoId, Title = (string)video.snippet.title, ThumbnailUrl = (string)video.snippet.thumbnails.@default.url });
                            }
                            catch { }
                        }

                        try
                        {
                            nextPageToken = contentObj.nextPageToken;
                        }
                        catch
                        {
                            nextPageToken = null;
                        }

                    }
                } while (nextPageToken != null);

            }
            catch { }

            return playlistItems;
        }

        private async Task GetAdditionalVideoDetailsAsync(PlayableItemInfo video)
        {
            // Create a list with all names and values of the get parameters
            var paramList = new List<KeyValuePair<string, string>>();
            paramList.Add(new KeyValuePair<string, string>(KeyParameterName, _config.Value.YoutubeApiKey));
            paramList.Add(new KeyValuePair<string, string>(IdParameterName, video.ItemId));
            paramList.Add(new KeyValuePair<string, string>(PartParameterName, VideoDetailsPartParameterValue));
            paramList.Add(new KeyValuePair<string, string>(FieldsParameterName, VideoDetailsFieldsParameterValue));

            // Get a string of all get parameters
            string paramString = BuildGetParamatersString(paramList);

            try
            {
                var uriBuilder = new UriBuilder(_config.Value.YoutubeVideoEndpoint);
                uriBuilder.Query = paramString;

                // Get a response from the API
                var response = await _client.GetAsync(uriBuilder.Uri);

                if (response.IsSuccessStatusCode)
                {
                    // If the request was successful take the required info from the response
                    var contentStr = await response.Content.ReadAsStringAsync();
                    dynamic contentObj = await Task.Run(() => JsonConvert.DeserializeObject(contentStr));
                    if ((string)contentObj.items[0].status.uploadStatus != VideoAvailableUploadStatus)
                        throw new TrackNotAvailableException();
                    video.Duration = ISO8601TimeSpanConverter.ConvertToTimeSpan((string)contentObj.items[0].contentDetails.duration);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary> 
        /// Creates a get parameter string from a list of key-value pairs. 
        /// </summary> 
        /// <param name="parameters">List of key-value pairs consisting of parameter names and values.</param> 
        /// <returns>String of get parameters.</returns> 
        private string BuildGetParamatersString(List<KeyValuePair<string, string>> parameters)
        {
            StringBuilder builder = new StringBuilder();

            // For all but the last param add them to the string, followed by an '&' 
            for (int i = 0; i < parameters.Count - 1; i++)
                builder.Append(parameters[i].Key).Append("=").Append(parameters[i].Value).Append("&");

            // Add the last param without trailing '&'. First check if there is at least 1 param. 
            if (parameters.Count > 0)
                builder.Append(parameters.Last().Key).Append("=").Append(parameters.Last().Value);

            return builder.ToString();
        }
    }
}
