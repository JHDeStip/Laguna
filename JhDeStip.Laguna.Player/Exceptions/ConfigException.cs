using System;

namespace JhDeStip.Laguna.Player.Exceptions
{
    /// <summary>
    /// Exception regarding configuration.
    /// </summary>
    public class ConfigException : Exception
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ConfigException() : base() { }

        /// <summary>
        /// Constructor taking a message that describes the error.
        /// </summary>
        /// <param name="message">Message.</param>
        public ConfigException(string message) : base(message) { }

        /// <summary>
        /// Constructor taking a message that describes the error and an inner exception.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="innerException">Inner exception.</param>
        public ConfigException(string message, Exception innerException) : base(message, innerException) { }
    }
}
