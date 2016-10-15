using Stannieman.CacheTemp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JhDeStip.Laguna.TrackProvider
{
    public class YoutubeTrackDownloader2 : ITrackDownloader
    {
        public event EventHandler<TrackDownloadCompletedEventArgs> TrackDownloadCompleted;
        public event EventHandler<TrackDownloadFailedEventArgs> TrackDownloadFailed;

        #region Instance variables and constants

        // V parameter for the Youtube video page url query
        private const string VParameterName = "v";
        private const string FFmpegExeFileName = "ffmpeg.exe";
        private const string FFmpegCommandArguments = "-i \"{0}\" -vn -acodec copy -f {1} {2} -y";

        // Youtube streams ordered from high to low quality audio
        private static readonly int[] PreferedQuelityOrder = { 141, 172, 22, 37, 38, 84, 43, 140, 171, 100, 82, 18, 5, 36, 17 };

        private ICacheTempManager _cacheTempManager;
        private YoutubeDownloadConfig _youtubeDownloadConfig;

        private List<string> _downloadQueue;
        private string _highestPriorityVideoId;

        private Process _downloadProcess;
        private bool _isDownloading = false;
        private string _downloadingVideoId = null;
        private bool _downloadKilled = false;

        #endregion

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task SetDownloadQueueAsync(List<string> trackIds)
        {
            throw new NotImplementedException();
        }

        public Task SetHighestPriorityTrack(string trackId)
        {
            throw new NotImplementedException();
        }
    }
}
