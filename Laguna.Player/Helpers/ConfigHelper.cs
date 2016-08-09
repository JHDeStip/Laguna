using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    Timeout = config.LagunaDal.Timeout,
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
                return new CacheTempConfig
                {
                    CacheDirectory =
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                            config.CacheTemp.CacheDirectory),
                    TempDirectory =
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                            config.CacheTemp.TempDirectory),
                    MaxFileAge = new TimeSpan((int) config.CacheTemp.MaxFileAge, 0, 0, 0)
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
                    DownloadAheadAmount = config.Download.DownloadAheadAmount
                };
            }
            catch (Exception e)
            {
                throw new ConfigException("The TrackDownload configuration has a missing or not allowed value.", e);
            }
        }
    }
}
