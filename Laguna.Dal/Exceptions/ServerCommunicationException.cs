using System;

namespace JhDeStip.Laguna.Dal
{
    /// <summary>
    /// Exception for if something goes wrong while doing an API call.
    /// </summary>
    public class ServerCommunicationException : Exception
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ServerCommunicationException() : base() { }

        /// <summary>
        /// Constructor taking a message that describes the error.
        /// </summary>
        /// <param name="message">Message.</param>
        public ServerCommunicationException(string message) : base(message) { }

        /// <summary>
        /// Constructor taking a message that describes the error and an inner exception.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="innerException">Inner exception.</param>
        public ServerCommunicationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
