using FluentResults;

using System.Text.Json;

namespace HSS.System.V2.Domain.ResultHelpers.Errors
{
    public class UnExpectedResponseError : Error
    {
        public UnExpectedResponseError(string message) : base(message) { }

        public UnExpectedResponseError()
        {

        }

        public static UnExpectedResponseError Happen(Type expectedType, Type actualType) =>
            new($"We expect to get response of type '{expectedType.Name}' but we got response of type {actualType.Name}");

        public static UnExpectedResponseError Happen<T>(Type actualType) =>
            new($"We expect to get response of type '{typeof(T).Name}' but we got response of type {actualType.Name}");

        public static UnExpectedResponseError Happen<T>(object? response) =>
            new($"We expect to get response of type '{typeof(T).Name}' but we got this response : [{JsonSerializer.Serialize(response)}]");
    }
}
