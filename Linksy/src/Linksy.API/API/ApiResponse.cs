using System.Net;

namespace Linksy.API.API
{
    public class ApiResponse<T>
    {
        public HttpStatusCode Code { get; set; }
        public string Status { get; set; } = "success";
        public T? Data { get; set; }
        public DateTime Date { get; set; } = TimeProvider.System.GetUtcNow().UtcDateTime;
        public ApiResponse(HttpStatusCode code, string status, T? data = default) : this(code, data)
        {
            Status = status;
        }
        public ApiResponse(HttpStatusCode code, T? data = default)
        {
            Code = code;
            Data = data;
        }
        public ApiResponse(T? data = default) : this(HttpStatusCode.OK, data) { }
    }
}
