using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Mvc;

using JhDeStip.Laguna.Server.Services;
using JhDeStip.Laguna.Server.Domain;
using Microsoft.AspNetCore.Authorization;
using JhDeStip.Laguna.Server.Filters;

namespace JhDeStip.Laguna.Server.Controllers
{
    [ServiceFilter(typeof(ServiceAvailabilityFilter))]
    [Route("api/v1.0/[controller]")]
    public class PlaylistController : Controller
    {
        private IPlaylistService _playListService;

        public PlaylistController(IPlaylistService playListService)
        {
            _playListService = playListService;
        }

        // GET: api/playlist/getNextAndSetAsNowplaying
        [HttpGet]
        [Route("getNextAndSetAsNowPlaying")]
        public async Task<PlayableItemInfo> GetNextndSetAsNowPlaying()
        {
            PlayableItemInfo nextItem = await _playListService.GetNextAndSetAsNowPlayingAsync();

            if (nextItem == null)
                Response.StatusCode = (int)HttpStatusCode.NoContent;

            return nextItem;
        }

        // GET: api/playlist/nowPlaying
        [HttpGet]
        [Route("nowPlaying")]
        public async Task<PlayableItemInfo> GetNowPlaying()
        {
            PlayableItemInfo nowPlaying = await _playListService.GetNowPlayingAsync();

            if (nowPlaying == null)
                Response.StatusCode = (int)HttpStatusCode.NoContent;

            return nowPlaying;
        }

        // GET: api/playlist/nextN
        [HttpGet]
        [Route("nextN")]
        public async Task<IEnumerable<PlayableItemInfo>> GetNextNAsync()
        {
            var query = Request.Query;
            int amount;
            if (query.Keys.Count == 1 && query.Keys.First() == "amount" && int.TryParse(query["amount"], out amount))
                return await _playListService.GetNextNAsync(amount);
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return null;
            }
        }
    }
}
