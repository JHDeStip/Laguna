using System;
using JhDeStip.Laguna.Dal;

namespace JhDeStip.Laguna.Client.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        public DalConfig BuildDalConfig()
        {
            return new DalConfig
            {
                // Fixed IP of PC behind bar
                ServerHost = @"http://192.168.0.100",
                ServerPort = 5000,
                Timeout = new TimeSpan(0, 0, 15),
                APIBasePath = "api/preview4/",
                TrackSearchPath = "trackSearch/",
                NowPlayingPath = "playlist/nowPlaying/",
                UserQueueItemsPath = "playlist/userQueue/items/",
                NextNItemsPath = "playlist/nextN/",
                GetNextAndSetAsNowPlayingPath = "playlist/getNextAndSetAsNowPlaying/",
                FallbackPlaylistItemsPath = "playlist/fallbackPlaylist/items/"
            };
        }
    }
}
