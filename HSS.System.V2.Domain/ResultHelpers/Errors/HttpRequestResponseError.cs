using FluentResults;

using System.Text.Json;

namespace HSS.System.V2.Domain.ResultHelpers.Errors
{
    public class HttpRequestResponseError : Error
    {
        public HttpRequestResponseError(string message) : base(message) { }

        public HttpRequestResponseError(object? obj) : base(JsonSerializer.Serialize(obj))
        {

        }
        public HttpRequestResponseError()
        {

        }
    }
}
