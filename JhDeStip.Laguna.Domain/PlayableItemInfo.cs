using System;

namespace JhDeStip.Laguna.Domain
{
    /// <summary>
    /// Class containing info about an item that can be played back.
    /// </summary>
    public class PlayableItemInfo : IEquatable<object>
    {
        /// <summary>
        /// Id of the item.
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// Title of the item.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Duration of the item.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// URL of a thumbnail image for the item.
        /// </summary>
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
            return base.Equals(other) && ItemId == checkObj.ItemId;
        }
    }
}