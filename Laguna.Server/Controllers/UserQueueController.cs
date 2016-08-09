using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Mvc;

using JhDeStip.Laguna.Server.Services;
using JhDeStip.Laguna.Server.Domain;
using JhDeStip.Laguna.Server.Filters;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace JhDeStip.Laguna.Server.Controllers
{
    [ServiceFilter(typeof(ServiceAvailabilityFilter))]
    [Route("api/v1.0/playlist/[controller]/items")]
    public class UserQueueController : Controller
    {
        private IPlaylistService _playListService;

        public UserQueueController(IPlaylistService playListService)
        {
            _playListService = playListService;
        }

        // GET: api/playlist/userQueue/items
        [HttpGet]
        public async Task<IEnumerable<PlayableItemInfo>> Get()
        {
            return await _playListService.GetAllFromUserQueueAsync();
        }

        // POST api/playlist/userQueue/items
        [HttpPost]
        //[Authorize(Policy = nameof(ApplicationAvailableRequirement))]
        public async Task Post([FromBody]PlayableItemInfo newPlayableItemInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _playListService.AddToUserQueueAsync(newPlayableItemInfo);
                    Response.StatusCode = (int)HttpStatusCode.Created;
                }
                else
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            catch
            {
                Response.StatusCode = (int)HttpStatusCode.Conflict;
            }
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            try
            {
                await _playListService.RemoveFromUserQueueAsync(id);
                Response.StatusCode = (int)HttpStatusCode.OK;
            }
            catch (TrackNotInQueueException)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
        }
    }
}
