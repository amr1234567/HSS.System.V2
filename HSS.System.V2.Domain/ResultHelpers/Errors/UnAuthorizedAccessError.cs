using FluentResults;

namespace HSS.System.V2.Domain.ResultHelpers.Errors
{
    public class UnAuthorizedAccessError : Error
    {
        public UnAuthorizedAccessError(string message) : base(message)
        { }

        public UnAuthorizedAccessError() : base("UnAuth Access") { }

    }
}
