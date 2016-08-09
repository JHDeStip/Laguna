using System;

namespace JhDeStip.Laguna.Dal
{
    /// <summary>
    /// Exception regarding playlists.
    /// </summary>
    public class PlaylistException : Exception
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public PlaylistException() : base() { }

        /// <summary>
        /// Constructor taking a message that describes the error.
        /// </summary>
        /// <param name="message">Message.</param>
        public PlaylistException(string message) : base(message) { }

        /// <summary>
        /// Constructor taking a message that describes the error and an inner exception.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="innerException">Inner exception.</param>
        public PlaylistException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Exception thrown if a track is added to the queue but is already queued.
    /// </summary>
    public class TrackAlreadyInQueueException : PlaylistException
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public TrackAlreadyInQueueException() : base() { }

        /// <summary>
        /// Constructor taking a message that describes the error.
        /// </summary>
        /// <param name="message">Message.</param>
        public TrackAlreadyInQueueException(string message) : base(message) { }

        /// <summary>
        /// Constructor taking a message that describes the error and an inner exception.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="innerException">Inner exception.</param>
        public TrackAlreadyInQueueException(string message, Exception innerException) : base(message, innerException) { }
    }
}
