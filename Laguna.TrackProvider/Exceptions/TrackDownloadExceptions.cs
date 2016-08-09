using System;

namespace JhDeStip.Laguna.TrackProvider
{
    /// <summary>
    /// Exception for if something goes wrong with downloading tracks.
    /// </summary>
    public class TrackDownloadException : Exception
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public TrackDownloadException() : base() { }

        /// <summary>
        /// Constructor taking a message that describes the error.
        /// </summary>
        /// <param name="message">Message.</param>
        public TrackDownloadException(string message) : base(message) { }

        /// <summary>
        /// Constructor taking a message that describes the error and an inner exception.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="innerException">Inner exception.</param>
        public TrackDownloadException(string message, Exception innerException) : base(message, innerException) { }
    }
}
