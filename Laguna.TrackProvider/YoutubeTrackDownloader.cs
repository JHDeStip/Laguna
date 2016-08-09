using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

using YoutubeExtractor;
using Stannieman.CacheTemp;
using Stannieman.HttpQueries;

namespace JhDeStip.Laguna.TrackProvider
{
    /// <summary>
    /// Service to download videos from Youtube and save them as audio file to the application's cache.
    /// </summary>
    public class YoutubeTrackDownloader : ITrackDownloader
    {
        #region Instance variables and constants

        // V parameter for the Youtube video page url query
        private const string V_PARAMETER_NAME = "v";
        private const string FFMPEG_EXE_FILE_NAME = "ffmpeg.exe";
        private const string FFMPEG_COMMAND_ARGUMENTS = "-i \"{0}\" -vn -acodec copy -f {1} {2} -y";

        // Youtube streams ordered from high to low quality audio
        private static readonly int[] PREFERED_QUALITY_ORDER = { 141, 172, 22, 37, 38, 84, 43, 140, 171, 100, 82, 18, 5, 36, 17 };

        private ICacheTempManager _cacheTempManager;
        private YoutubeDownloadConfig _youtubeDownloadConfig;

        private List<string> _downloadQueue;
        private string _highestPriorityVideoId;

        private Process _downloadProcess;
        private bool _isDownloading = false;
        private string _downloadingVideoId = null;
        private bool _downloadKilled = false;

        #endregion

        // Event that fires when a track has completed downloading
        public event EventHandler<TrackDownloadCompletedEventArgs> TrackDownloadCompleted;
        // Event that fires when a track failed to download
        public event EventHandler<TrackDownloadFailedEventArgs> TrackDownloadFailed;

        /// <summary>
        /// Constructor accepting the required dependencies.
        /// </summary>
        /// <param name="youtubeDownloadConfig">Object containing configuration for Youtube.</param>
        /// <param name="cacheTempManager">Service that manages the application's cache.</param>
        public YoutubeTrackDownloader(YoutubeDownloadConfig youtubeDownloadConfig, ICacheTempManager cacheTempManager)
        {
            _youtubeDownloadConfig = youtubeDownloadConfig;
            _cacheTempManager = cacheTempManager;
            _downloadQueue = new List<string>();
            _highestPriorityVideoId = null;
            _downloadingVideoId = null;
            _downloadProcess = new Process();
        }

        /// <summary>
        /// Sets a video as highest priority.
        /// If a download is busy this will be canceled and this track will download first.
        /// </summary>
        /// <param name="videoId">ID of video to give priority.</param>
        public async Task SetHighestPriorityTrack(string videoId)
        {
            await Task.Run(() =>
            {
                lock (_downloadQueue)
                {
                    // Only do something if the same video is not already set as highest priority video and we are not already downloading it
                    if (_highestPriorityVideoId != videoId && !(_isDownloading && _downloadingVideoId == videoId))
                    {
                        _highestPriorityVideoId = videoId;

                        // There is a different track downloading, so we kill that
                        lock (_downloadProcess)
                        {
                            _downloadKilled = true;
                            try
                            {
                                _downloadProcess.Kill();
                            }
                            catch { }
                        }
                    }
                }

                // Start the download process
                Task.Run(() => DownloadQueuedVideos());
            });
        }

        /// <summary>
        /// Adds video IDs to the queue to download.
        /// </summary>
        /// <param name="videoIds">List with IDs of the videos to add to the queue.</param>
        /// <returns>Task instance.</returns>
        public async Task SetDownloadQueueAsync(List<string> videoIds)
        {
            await Task.Run(() =>
            {
                // Lock the queue so we can do our thing safely
                lock (_downloadQueue)
                {
                    _downloadQueue = new List<string>();

                    // We check for every ID if we still need to download it
                    foreach (string videoId in videoIds)
                        // If the track is already in cache or it's set as highest priority we don't need to do anything
                        if (!_cacheTempManager.IsFileWithoutExtensionInCache(videoId) && _highestPriorityVideoId != videoId)
                        {
                            // If we are downloading a track, check if that track is the one we're trying to add.
                            if (_isDownloading)
                            {
                                if (_downloadingVideoId != videoId)
                                    _downloadQueue.Add(videoId);
                            }
                            else
                                _downloadQueue.Add(videoId);
                        }

                    // If no worker is downloading, start a new worker
                    if (!_isDownloading)
                        Task.Run(() => DownloadQueuedVideos());
                }
            });
        }

        /// <summary>
        /// Downloads all video's in queue and the high priority video if present.
        /// </summary>
        private void DownloadQueuedVideos()
        {
            do
            {
                // If there is a high priority video or there is a video in queue, set indicaters right.
                lock (_downloadQueue)
                {
                    if (_highestPriorityVideoId != null)
                    {
                        _isDownloading = true;
                        _downloadingVideoId = _highestPriorityVideoId;
                        _highestPriorityVideoId = null;
                    }
                    else if (_downloadQueue.Count > 0)
                    {
                        _isDownloading = true;
                        _downloadingVideoId = _downloadQueue.First();
                        _downloadQueue.RemoveAt(0);
                    }
                };

                // Download the video if there is one
                if (_downloadingVideoId != null)
                {
                    try
                    {
                        // Get info about different video files for a given video
                        IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(new Query(V_PARAMETER_NAME, _downloadingVideoId).QueryString);

                        // Get the best quality video
                        VideoInfo bestQualityAudioVideoInfo = GetVideoWithBestAudioQuality(videoInfos);

                        // bestQualityAudioVideoInfo is null if no suitable video is found
                        if (bestQualityAudioVideoInfo != null)
                        {
                            // Decrypt the URL if necessary
                            if (bestQualityAudioVideoInfo.RequiresDecryption)
                                DownloadUrlResolver.DecryptDownloadUrl(bestQualityAudioVideoInfo);

                            // Download the file and save as audio
                            DownloadAudioForVideoFile(bestQualityAudioVideoInfo.DownloadUrl, GetFFmpegAudioFormatNameForAudioType(bestQualityAudioVideoInfo.AudioType), _downloadingVideoId + "." + GetFileExtensionForAudioType(bestQualityAudioVideoInfo.AudioType));

                            // Report the download has finished, even if something went wrong
                            TrackDownloadCompleted?.Invoke(this, new TrackDownloadCompletedEventArgs(_downloadingVideoId));
                        }
                        else
                            // Report that the download failed, there is no suitable file to download
                            TrackDownloadFailed?.Invoke(this, new TrackDownloadFailedEventArgs(_downloadingVideoId, "No suitable file was found to download."));
                    }
                    catch
                    {
                        // Report that the download failed
                        TrackDownloadFailed?.Invoke(this, new TrackDownloadFailedEventArgs(_downloadingVideoId, "The track was not found or an error occured doring download or demuxing."));
                    }

                    lock (_downloadQueue)
                    {
                        // If there are no more videos to download we end the loop by setting the _downloadingVideoId to null. Also reset the downloading indicator.
                        if (_highestPriorityVideoId == null && _downloadQueue.Count <= 0)
                        {
                            _downloadingVideoId = null;
                            _isDownloading = false;
                        }

                    }
                }
            } while (_downloadingVideoId != null);
        }

        /// <summary>
        /// Returns the video with the best audio quality from a list of VideoInfo instances.
        /// </summary>
        /// <param name="videoInfos">List of VideoInfo instances to find the best from.</param>
        /// <returns>Best quality video or null if no suitable video is found.</returns>
        private VideoInfo GetVideoWithBestAudioQuality(IEnumerable<VideoInfo> videoInfos)
        {
            // For every quality in best to low quality order, retun the video of this quality if found
            foreach (int quality in PREFERED_QUALITY_ORDER)
            {
                var bestQualityVideoInfos = from videoInfo in videoInfos
                                            where videoInfo.FormatCode == quality
                                            select videoInfo;

                // Linq query returns a list, so take the first one if there are videos in it
                if (bestQualityVideoInfos.Count() > 0)
                    return bestQualityVideoInfos.First();
            }

            return null;
        }

        /// <summary>
        /// Downloads a file from the given URL and save it to cache in the given format with the given filename.
        /// </summary>
        /// <param name="url">URL to download.</param>
        /// <param name="audioFormat">Audio format as FFmpeg expects it.</param>
        /// <param name="fileName">File name for the downloaded file.</param>
        private void DownloadAudioForVideoFile(string url, string audioFormat, string fileName)
        {
            // Create ProcessStartInfo. We use Process.Start to invoke the external FFmpeg executable.
            ProcessStartInfo startInfo = new ProcessStartInfo(FFMPEG_EXE_FILE_NAME);
            startInfo.Arguments = String.Format(FFMPEG_COMMAND_ARGUMENTS, url, audioFormat, Path.Combine(_cacheTempManager.TempDirectory, fileName));
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;

            try
            {
                // Start the process and check if it ran without errors after it exits.
                // The process will download the file to the application's temp directory.
                lock (_downloadProcess)
                {
                    _downloadKilled = false;
                    _downloadProcess = new Process();
                    _downloadProcess.StartInfo = startInfo;
                    // Check if the download was supposed to be killed. In that case we don't continue.
                    if (!_downloadKilled)
                        _downloadProcess.Start();
                }

                _downloadProcess.WaitForExit();

                bool downloadSuccessful = false;
                // Check if the download process finished successfully. Throw an exception if it didn't and if that was not intended.
                lock (_downloadProcess)
                {
                    if (_downloadProcess.ExitCode == 0)
                        downloadSuccessful = true;
                    // Only throw an exception if the download was not intentionally killed
                    else if (!_downloadKilled)
                        throw new TrackDownloadException($"An error occured while downloading the file using {FFMPEG_EXE_FILE_NAME}.");
                }

                // Move the file from the Temp to the Cache directory.
                if (downloadSuccessful)
                    File.Move(Path.Combine(_cacheTempManager.TempDirectory, fileName), Path.Combine(_cacheTempManager.CacheDirectory, fileName));
            }
            catch (TrackDownloadException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                new TrackDownloadException($"An error occured while downloading the file using {FFMPEG_EXE_FILE_NAME}.", e);
            }
        }

        /// <summary>
        /// Returns the name of the audio format as FFmpeg expects it for to a given AudioType.
        /// </summary>
        /// <param name="audioType">Type of audio to get the FFmpeg format for.</param>
        /// <returns>FFmpeg audio format string.</returns>
        private string GetFFmpegAudioFormatNameForAudioType(AudioType audioType)
        {
            switch (audioType)
            {
                case AudioType.Aac:
                    return "adts";
                case AudioType.Vorbis:
                    return "ogg";
                case AudioType.Mp3:
                    return "mp3";
                default:
                    throw new Exception();
            }
        }

        /// <summary>
        /// Returns the file extension for a given AudioType.
        /// </summary>
        /// <param name="audioType">Type of audio to get the file extension for.</param>
        /// <returns>File extension.</returns>
        private string GetFileExtensionForAudioType(AudioType audioType)
        {
            switch (audioType)
            {
                case AudioType.Aac:
                    return "aac";
                case AudioType.Vorbis:
                    return "ogg";
                case AudioType.Mp3:
                    return "mp3";
                default:
                    throw new Exception();
            }
        }

        /// <summary>
        /// Disposes the service.
        /// </summary>
        public void Dispose()
        {
            // Kill the download process
            lock (_downloadProcess)
            {
                _downloadKilled = true;
                try
                {
                    // If there's no process running this throws an exception
                    _downloadProcess.Kill();
                }
                catch { }
                _downloadProcess.Dispose();
            }
        }
    }
}