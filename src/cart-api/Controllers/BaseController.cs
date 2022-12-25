using Microsoft.AspNetCore.Mvc;
using cart_api.Models;
using Microsoft.Extensions.Logging;
using System;

namespace cart_api.Controllers
{
    public class BaseController : ControllerBase, IBaseController
    {
        protected readonly ILogger<BaseController> _logger;

        protected BaseController(ILogger<BaseController> logger) => _logger = logger;

        void IBaseController.LogException(string message, Exception ex) => _logger.LogError(ex, $"{ControllerContext.ActionDescriptor.ControllerName}.{ControllerContext.ActionDescriptor.ActionName} {message}");

        void IBaseController.LogInformation(string message) => _logger.LogInformation($"{ControllerContext.ActionDescriptor.ControllerName}.{ControllerContext.ActionDescriptor.ActionName} {message}");

        protected IActionResult ToSendResponse()
        {
            var baseResponse = new BaseResponse(System.Net.HttpStatusCode.OK);
            return ToSendResponse(baseResponse);
        }

        protected IActionResult ToSendResponse(BaseResponse baseResponse) => StatusCode(baseResponse.StatusCode, baseResponse);
    }
}