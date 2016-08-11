using Microsoft.AspNetCore.Mvc;
using JhDeStip.Laguna.Server.Services;
using JhDeStip.Laguna.Server.Domain;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace JhDeStip.Laguna.Server.Controllers
{
    [Route("api/v1.0/[controller]")]
    public class ServiceAvailabilityController : Controller
    {
        private IServiceAvailabilityService _serviceAvailabilityService;

        public ServiceAvailabilityController(IServiceAvailabilityService serviceAvailabilityService)
        {
            _serviceAvailabilityService = serviceAvailabilityService;
        }

        // GET: api/serviceAvailability/setAvailable
        [HttpGet]
        [Route("setAvailable")]
        public void SetAvailable()
        {
            _serviceAvailabilityService.IsServiceAvailable = true;
        }

        // GET: api/serviceAvailability/setUnvailable
        [HttpGet]
        [Route("setUnavailable")]
        public void SetUnavailable()
        {
            _serviceAvailabilityService.IsServiceAvailable = false;
        }

        // GET: api/serviceAvailability
        [HttpGet]
        public ServiceAvailability IsServiceAvailable()
        {
            return new ServiceAvailability { IsServiceAvailable = _serviceAvailabilityService.IsServiceAvailable };
        }
    }
}
