using FluentResults;

namespace HSS.System.V2.Domain.ResultHelpers.Errors
{
    public class UnSupportedTypeError : Error
    {
        public UnSupportedTypeError(string message) : base(message) { }

        public UnSupportedTypeError()
        {

        }

        public static UnSupportedTypeError Happen<T>() => new UnSupportedTypeError($"Unsupported {typeof(T).Name} type");
    }
}
