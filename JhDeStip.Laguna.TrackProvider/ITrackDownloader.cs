using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JhDeStip.Laguna.TrackProvider
{
    public interface ITrackDownloader : IDisposable
    {
        event EventHandler<TrackDownloadCompletedEventArgs> TrackDownloadCompleted;
        event EventHandler<TrackDownloadFailedEventArgs> TrackDownloadFailed;

        Task SetHighestPriorityTrack(string trackId);
        Task SetDownloadQueueAsync(List<string> trackIds);
    }
}