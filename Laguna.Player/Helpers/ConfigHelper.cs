using System;
using System.IO;
using JhDeStip.Laguna.Dal;
using JhDeStip.Laguna.Player.Config;
using JhDeStip.Laguna.Player.Exceptions;
using JhDeStip.Laguna.TrackProvider;
using Stannieman.CacheTemp;

namespace JhDeStip.Laguna.Player.Helpers
{
    public class ConfigHelper
    {
        public DalConfig BuildDalConfig(dynamic config)
        {
            try
            {
                return new DalConfig
                {
                    ServerHost = config.LagunaDal.ServerHost,
                    ServerPort = config.LagunaDal.ServerPort,
                    Timeout = new TimeSpan(0, 0, 0, Int32.Parse(config.LagunaDal.Timeout.ToString())),
                    ApiBasePath = config.LagunaDal.APIBasePath,
                    TrackSearchPath = config.LagunaDal.TrackSearchPath,
                    NowPlayingPath = config.LagunaDal.NowPlayingPath,
                    UserQueueItemsPath = config.LagunaDal.UserQueueItemsPath,
                    NextNItemsPath = config.LagunaDal.NextNItemsPath,
                    GetNextAndSetAsNowPlayingPath = config.LagunaDal.GetNextAndSetAsNowPlayingPath,
                    FallbackPlaylistItemsPath = config.LagunaDal.FallbackPlaylistItemsPath,
                    SetServiceAvailablePath = config.LagunaDal.SetServiceAvailablePath,
                    SetServiceUnavailablePath = config.LagunaDal.SetServiceUnavailablePath,
                    ServiceAvailabilityPath = config.LagunaDal.ServiceAvailabilityPath,
                    RefreshFallbackPlaylistOnServerPath = config.LagunaDal.RefreshFallbackPlaylistOnServerPath
                };
            }
            catch (Exception e)
            {
                throw new ConfigException("The DalConfig configuration has a missing or not allowed value.", e);
            }
        }

        public CacheTempConfig BuildCacheTempConfig(dynamic config)
        {
            try
            {
                var programDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

                return new CacheTempConfig
                {
                    CacheDirectory = Path.Combine(programDataPath, config.CacheTemp.CacheDirectory.ToString()),
                    TempDirectory = Path.Combine(programDataPath, config.CacheTemp.TempDirectory.ToString()),
                    MaxFileAge = new TimeSpan(Int32.Parse(config.CacheTemp.MaxFileAge.ToString()), 0, 0, 0)
                };
            }
            catch (Exception e)
            {
                throw new ConfigException("The CacheTemp configuration has a missing or not allowed value.", e);
            }
        }

        public YoutubeDownloadConfig BuildYoutubeDownloadConfig(dynamic config)
        {
            try
            {
                return new YoutubeDownloadConfig
                {
                    VideoPageUrl = config.Youtube.VideoPageUrl
                };
            }
            catch (Exception e)
            {
                throw new ConfigException("The YoutubeDownload configuration has a missing or not allowed value.", e);
            }
        }

        public TrackDownloadConfig BuildTrackDownloadConfig(dynamic config)
        {
            try
            {
                return new TrackDownloadConfig
                {
                    DownloadAheadAmount = config.TrackDownload.DownloadAheadAmount
                };
            }
            catch (Exception e)
            {
                throw new ConfigException("The TrackDownload configuration has a missing or not allowed value.", e);
            }
        }
    }
}
