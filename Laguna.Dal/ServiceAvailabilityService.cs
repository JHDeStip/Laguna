using JhDeStip.Laguna.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JhDeStip.Laguna.Dal
{
    /// <summary>
    /// Service availability service.
    /// This class can be used set or get the availability of the service.
    /// </summary>
    public class ServiceAvailabilityService : IServiceAvailabilityService
    {
        #region Instance variables and constants

        private HttpClient _client;
        private DalConfig _config;

        #endregion

        /// <summary>
        /// Constructor taking the required dependencies.
        /// </summary>
        public ServiceAvailabilityService(DalConfig config)
        {
            _config = config;
            _client = new HttpClient { Timeout = _config.Timeout };
        }

        /// <summary>
        /// Makes the service available.
        /// </summary>
        public async Task MakeServiceAvailable()
        {
            try
            {
                // Build uri
                var uriBuilder = new UriBuilder(_config.ServerHost);
                uriBuilder.Port = _config.ServerPort;
                uriBuilder.Path = _config.ApiBasePath + _config.SetServiceAvailablePath;

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
        /// Makes the service unavailable.
        /// </summary>
        public async Task MakeServiceUnavailable()
        {
            try
            {
                // Build uri
                var uriBuilder = new UriBuilder(_config.ServerHost);
                uriBuilder.Port = _config.ServerPort;
                uriBuilder.Path = _config.ApiBasePath + _config.SetServiceUnavailablePath;

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
        /// Returns whether the service is available or not.
        /// </summary>
        /// <returns>Whether the service is available.</returns>
        public async Task<bool> IsServiceAvailable()
        {
            try
            {
                // Build uri
                var uriBuilder = new UriBuilder(_config.ServerHost);
                uriBuilder.Port = _config.ServerPort;
                uriBuilder.Path = _config.ApiBasePath + _config.ServiceAvailabilityPath;

                // Get a response from the API
                var response = await _client.GetAsync(uriBuilder.Uri);

                if (response.IsSuccessStatusCode)
                    return await Task.Run(async () => JsonConvert.DeserializeObject<ServiceAvailability>(await response.Content.ReadAsStringAsync()).IsServiceAvailable);
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
