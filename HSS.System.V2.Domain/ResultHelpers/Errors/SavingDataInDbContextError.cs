using FluentResults;

namespace HSS.System.V2.Domain.ResultHelpers.Errors
{
    public class SavingDataInDbContextError : Error
    {
        public SavingDataInDbContextError(string message) : base(message)
        {
            
        }

        public SavingDataInDbContextError(Exception ex) : base($"Error while saving the data to context : [ERR]-->[{ex.Message}]")
        {
            
        }
    }
}
