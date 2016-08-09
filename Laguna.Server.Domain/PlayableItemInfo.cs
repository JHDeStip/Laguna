using System;
using System.ComponentModel.DataAnnotations;

namespace JhDeStip.Laguna.Server.Domain
{
    /// <summary>
    /// Class containing info about a playable item.
    /// </summary>
    public class PlayableItemInfo : IEquatable<PlayableItemInfo>
    {
        /// <summary>
        /// Id of the item.
        /// </summary>
        [Required]
        public string ItemId { get; set; }

        /// <summary>
        /// Title of the item.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Diration of the item.
        /// </summary>
        [Required]
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Url where the thumbnail for the item can be found.
        /// </summary>
        [Required]
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// Equals implementation.
        /// Returns whether the given object is the same as this one.
        /// </summary>
        /// <param name="other">Object to check for equality.</param>
        /// <returns>Whether the object is the same as this one.</returns>
        public override bool Equals(object other)
        {
            // PlayableItemInfo instances are considered equal if their ItemId is the same
            return base.Equals(other) && ItemId == ((PlayableItemInfo)other).ItemId;
        }

        /// <summary>
        /// Equals implementation.
        /// Returns whether the given object is the same as this one.
        /// </summary>
        /// <param name="other">Object to check for equality.</param>
        /// <returns>Whether the object is the same as this one.</returns>
        public bool Equals(PlayableItemInfo other)
        {
            // PlayableItemInfo instances are considered equal if their ItemId is the same
            return ItemId == other.ItemId;
        }

        /// <summary>
        /// GetHashCode implementation.
        /// Returns the hash code of the instance.
        /// </summary>
        /// <returns>Hash code of the instance.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
