using FluentResults;

using System.Text.Json;

namespace HSS.System.V2.Domain.ResultHelpers.Errors
{
    public class UnKnownError : Error
    {
        public UnKnownError(string message) : base(message) { }

        public UnKnownError(Exception exception) : base("We got this exception: " + exception.ToString())
        {

        }
        public UnKnownError()
        {

        }
    }
}
