using JhDeStip.Laguna.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace JhDeStip.Laguna.Server.Filters
{
    public class ServiceAvailabilityFilter : IActionFilter
    {
        IServiceAvailabilityService _serviceAvailabilityService;

        public ServiceAvailabilityFilter(IServiceAvailabilityService serviceAvailabilityService)
        {
            _serviceAvailabilityService = serviceAvailabilityService;
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!_serviceAvailabilityService.IsServiceAvailable)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                context.HttpContext.Response.Headers.Clear();
                context.Result = new EmptyResult();
            }  
        }
    }
}
