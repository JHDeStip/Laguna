using System;
using System.Threading.Tasks;

namespace JhDeStip.Laguna.Dal
{
    /// <summary>
    /// Interface describing a service availability service.
    /// The service can be used to set or retreive the availability of the service.
    /// </summary>
    public interface IServiceAvailabilityService : IDisposable
    {
        /// <summary>
        /// Makes the service available.
        /// </summary>
        Task MakeServiceAvailable();
        /// <summary>
        /// Makes the service unavailable.
        /// </summary>
        Task MakeServiceUnavailable();
        /// <summary>
        /// Returns whether the service is available or not.
        /// </summary>
        /// <returns>Whether the service is available.</returns>
        Task<bool> IsServiceAvailable();
    }
}