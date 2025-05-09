using FluentResults;

namespace HSS.System.V2.Domain.ResultHelpers.Errors
{
    public class RetrievingDataFromDbContextError : Error
    {
        public RetrievingDataFromDbContextError(string message) : base(message)
        {

        }

        public RetrievingDataFromDbContextError(Exception ex) : base($"Error while retrieving the data to context : [ERR]-->[{ex.Message}]")
        {

        }
    }
}
