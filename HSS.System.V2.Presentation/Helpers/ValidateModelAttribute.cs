using HSS.System.V2.Domain.Helpers.Models;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HSS.System.V2.Presentation.Helpers
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errorDict = context.ModelState
                    .Where(ms => ms.Value.Errors.Any())
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                var response = ApiResponse<object>.Error(
                    message: "Validation Failed",
                    errors: errorDict,
                    statusCode: 400
                );

                context.Result = new JsonResult(response)
                {
                    StatusCode = 400
                };
            }
        }
    }

}
