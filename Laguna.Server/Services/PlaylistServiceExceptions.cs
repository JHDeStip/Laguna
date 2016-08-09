using System;

namespace JhDeStip.Laguna.Server.Services
{
    public class PlayListException : Exception
    {
        public PlayListException() : base() { }

        public PlayListException(string message) : base(message) { }

        public PlayListException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class TrackAlreadyInQueueException : PlayListException
    {
        public TrackAlreadyInQueueException() : base() { }

        public TrackAlreadyInQueueException(string message) : base(message) { }

        public TrackAlreadyInQueueException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class TrackNotInQueueException : PlayListException
    {
        public TrackNotInQueueException() : base() { }

        public TrackNotInQueueException(string message) : base(message) { }

        public TrackNotInQueueException(string message, Exception innerException) : base(message, innerException) { }
    }
}
