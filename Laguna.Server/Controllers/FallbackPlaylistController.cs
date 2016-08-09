using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;

using JhDeStip.Laguna.Server.Dal;
using JhDeStip.Laguna.Server.Services;
using JhDeStip.Laguna.Server.Domain;
using JhDeStip.Laguna.Server.Filters;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace JhDeStip.Laguna.Server.Controllers
{
    [ServiceFilter(typeof(ServiceAvailabilityFilter))]
    [Route("api/v1.0/playlist/[controller]")]
    public class FallbackPlaylistController : Controller
    {
        private IPlaylistService _playListService;
        private IYoutubeService _youtubeRepository;

        public FallbackPlaylistController(IPlaylistService playListService, IYoutubeService youtubeRepository)
        {
            _playListService = playListService;
            _youtubeRepository = youtubeRepository;
        }

        // GET: api/playlist/fallbackPlaylist/items
        [HttpGet]
        [Route("items")]
        public async Task<IEnumerable<PlayableItemInfo>> GetFallbackPlaylistItems()
        {
            return await _playListService.GetAllFromFallbackPlaylistAsync();
        }

        // GET: api/playlist/fallbackPlaylist/refresh
        [HttpGet]
        [Route("refresh")]
        public async Task RefreshFallbackPlaylist()
        {
            List<PlayableItemInfo> playlist = await _youtubeRepository.GetFallbackPlaylistItemsAsync();
            await _playListService.SetFallbackPlaylistAsync(playlist);
        }

        // POST api/playlist/fallbackPlaylist
        [HttpPost]
        public async Task Post([FromBody]List<PlayableItemInfo> fallbackPlaylist)
        {
            if (ModelState.IsValid)
            {
                await _playListService.SetFallbackPlaylistAsync(fallbackPlaylist);
                Response.StatusCode = (int)HttpStatusCode.Created;
            }
            else
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
}
