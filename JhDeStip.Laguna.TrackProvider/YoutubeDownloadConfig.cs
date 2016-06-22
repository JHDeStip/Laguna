namespace JhDeStip.Laguna.TrackProvider
{
    /// <summary>
    /// Class containing configuration for Youtube downloads.
    /// </summary>
    public class YoutubeDownloadConfig
    {
        private string _videoPageUrl;
        /// <summary>
        /// Base URL of web pages for videos.
        /// </summary>
        public string VideoPageUrl
        {
            get { return _videoPageUrl; }
            set
            {
                // Set default value if no value is given
                if (value == null)
                    value = @"http://www.youtube.com/watch";

                // Append value to ProgramData directory
                _videoPageUrl = value;
            }
        }
    }
}
