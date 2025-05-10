using System.Text.Json.Serialization;

namespace HSS.System.V2.Domain.Helpers.Models
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }

        public static ApiResponse<T> Success(T data, string message = "Mission accomplished")
        {
            return new ApiResponse<T>
            {
                StatusCode = 200,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> Error(string message, int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                Message = message,
                Data = default
            };
        }

        public static ApiResponse<T> Error(string message, T data, int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                Message = message,
                Data = data
            };
        }
    }
}