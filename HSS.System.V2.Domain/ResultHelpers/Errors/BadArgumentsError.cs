using FluentResults;

namespace HSS.System.V2.Domain.ResultHelpers.Errors
{
    public class BadArgumentsError : Error
    {
        public BadArgumentsError(string message) : base(message) { }

        public BadArgumentsError()
            : base("The parameters passed to the function are not as expected") { }

        public static BadArgumentsError Happen(string functionName = "", params object[] parameters)
        {
            string paramTypes = parameters.Any()
                ? string.Join(", ", parameters.Select(o => o.GetType().Name))
                : "No parameters";

            string functionPart = string.IsNullOrWhiteSpace(functionName)
                ? "in this format"
                : $"to function '{functionName}' in this format";

            return new BadArgumentsError($"[{paramTypes}] passed {functionPart} is not allowed.");
        }
    }

}
