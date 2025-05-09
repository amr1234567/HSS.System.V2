using FluentResults;

namespace HSS.System.V2.Domain.ResultHelpers.Errors
{
    public class UnSupportedBehaviorError : Error
    {
        public UnSupportedBehaviorError(string message) : base(message)
        { }

        public UnSupportedBehaviorError() : base("UnSupported Behavior") { }

    }
}
