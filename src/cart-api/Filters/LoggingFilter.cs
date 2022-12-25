using Microsoft.AspNetCore.Mvc.Filters;
using cart_api.Controllers;
using System.Net;
using System.Security;
using Microsoft.AspNetCore.Mvc;
using cart_api.Models;
using cart_api.Constants;
using cart_api.Exceptions;
using System;

namespace cart_api.Filters
{
    public class LoggingFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Controller is IBaseController baseController)
            {
                if (context.Exception != null)
                {
                    baseController.LogException(context.Exception.Message, context.Exception);
                    var message = string.Empty;

                    if (context.Exception is InvalidOperationException exception)
                    {
                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        message = exception.Message;
                    }
                    else if (context.Exception is SecurityException securityException)
                    {
                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        message = securityException.Message;
                    }
                    else if (context.Exception is NotFoundException notFoundException)
                    {
                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        message = notFoundException.Message;
                    }
                    else
                    {
                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        message = MessageConst.ERROR_MESSAGE;
                    }

                    context.Result = new JsonResult(new BaseResponse(message, (HttpStatusCode)context.HttpContext.Response.StatusCode));
                    context.ExceptionHandled = true;
                }

                baseController.LogInformation("returning");
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Controller is IBaseController baseController)
            {
                baseController.LogInformation($"beginning {context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString}");
            }
        }
    }
}