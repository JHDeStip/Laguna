using System.Collections.Generic;

namespace JhDeStip.Laguna.Server.Models
{
    /// <summary>
    /// Class representing a user that signs in using an API key.
    /// </summary>
    public class ApiKeyUser
    {
        /// <summary>
        /// Name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// API key of the user.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Whether the user is active. This means he is allowed to use the API.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// The permissions the user has.
        /// </summary>
        public HashSet<Permissions> Permissions { get; set; }
    }
}
