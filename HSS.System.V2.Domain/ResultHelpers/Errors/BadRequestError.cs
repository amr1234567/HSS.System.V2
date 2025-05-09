using FluentResults;

namespace HSS.System.V2.Domain.ResultHelpers.Errors
{
    public class BadRequestError : Error
    {
        public BadRequestError(string message) : base(message) { }

        public BadRequestError()
            : base() { }
    }

}
