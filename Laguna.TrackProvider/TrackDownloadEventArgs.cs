using System;

namespace JhDeStip.Laguna.TrackProvider
{
    /// <summary>
    /// EventArgs for when a track completed downloading.
    /// </summary>
    public class TrackDownloadCompletedEventArgs : EventArgs
    {
        /// <summary>
        /// ID of the track that finished downloading.
        /// </summary>
        public string TrackId { get; private set; }

        /// <summary>
        /// Constructor accepting a track ID of a track that finished downloading.
        /// </summary>
        /// <param name="trackId">ID of the track.</param>
        public TrackDownloadCompletedEventArgs(string trackId)
        {
            TrackId = trackId;
        }
    }

    /// <summary>
    /// EventArgs for when a track failed to download.
    /// </summary>
    public class TrackDownloadFailedEventArgs : EventArgs
    {
        /// <summary>
        /// ID of the track that failed to download.
        /// </summary>
        public string TrackId { get; private set; }
        /// <summary>
        /// Message with more info about the failure.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Constructor accepting a track ID of a track that failed to download and a message.
        /// </summary>
        /// <param name="trackId">ID of the track.</param>
        /// <param name="message">Message with more info about the failure.</param>
        public TrackDownloadFailedEventArgs(string trackId, string message)
        {
            TrackId = trackId;
            Message = message;
        }
    }
}
