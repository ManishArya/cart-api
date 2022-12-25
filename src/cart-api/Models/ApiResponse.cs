using System.Net;

namespace cart_api.Models
{
    public class ApiResponse<T> : BaseResponse
    {
        public ApiResponse(T content) : this(content, HttpStatusCode.OK) { }

        public ApiResponse(T content, HttpStatusCode statusCode) : base(statusCode) => Content = content;

        public T Content { get; set; }
    }
}