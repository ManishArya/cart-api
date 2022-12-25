using System.Net;
using Newtonsoft.Json;

namespace cart_api.Models
{
    public class BaseResponse
    {
        public BaseResponse(string description) : this(description, HttpStatusCode.OK) { }

        public BaseResponse(string description, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : this(statusCode) => ErrorDescription = description;

        public BaseResponse(HttpStatusCode statusCode) => StatusCode = (int)statusCode;

        public bool IsSuccess { get => (StatusCode == (int)HttpStatusCode.OK || StatusCode == (int)HttpStatusCode.NoContent); }

        [JsonIgnore]
        public int StatusCode { get; set; }

        public string ErrorDescription { get; set; }
    }
}