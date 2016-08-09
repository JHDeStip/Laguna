using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Mvc;

using JhDeStip.Laguna.Server.Dal;
using JhDeStip.Laguna.Server.Domain;
using JhDeStip.Laguna.Server.Filters;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace JhDeStip.Laguna.Server.Controllers
{
    [ServiceFilter(typeof(ServiceAvailabilityFilter))]
    [Route("api/v1.0/[controller]")]
    public class TrackSearchController : Controller
    {
        private IYoutubeService _youtubeRepository;
        public TrackSearchController(IYoutubeService youtubeRepository)
        {
            _youtubeRepository = youtubeRepository;
        }

        // GET: api/trackSearch
        [HttpGet]
        public async Task<IEnumerable<PlayableItemInfo>> Get()
        {
            var query = Request.Query;
            if (query.Keys.Count == 1 && query.Keys.First() == "query")
                return await _youtubeRepository.GetSearchResultsAsync(query["query"]);
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return null;
            }
        }
    }
}
