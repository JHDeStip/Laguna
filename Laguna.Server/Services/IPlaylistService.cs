using System.Collections.Generic;
using System.Threading.Tasks;

using JhDeStip.Laguna.Server.Domain;

namespace JhDeStip.Laguna.Server.Services
{
    public interface IPlaylistService
    {
        Task SetFallbackPlaylistAsync(List<PlayableItemInfo> fallbackPlaylist);
        Task AddToUserQueueAsync(PlayableItemInfo newPlayableItemInfo);
        Task<IEnumerable<PlayableItemInfo>> GetAllFromUserQueueAsync();
        Task<IEnumerable<PlayableItemInfo>> GetAllFromFallbackPlaylistAsync();
        Task<PlayableItemInfo> GetNextAndSetAsNowPlayingAsync();
        Task<IEnumerable<PlayableItemInfo>> GetNextNAsync(int amount);
        Task RemoveFromUserQueueAsync(string playableItemId);
        Task<PlayableItemInfo> GetNowPlayingAsync();
    }
}
