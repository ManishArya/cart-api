using System;

namespace cart_api.Controllers
{
    public interface IBaseController
    {
        void LogInformation(string message);
        void LogException(string message, Exception ex);
    }
}