using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace HSS.System.V2.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthorizeByEnumAttribute : AuthorizeAttribute
    {
        public AuthorizeByEnumAttribute(object enumValue)
        {
            if (enumValue == null)
                throw new ArgumentNullException(nameof(enumValue));

            if (!enumValue.GetType().IsEnum)
                throw new ArgumentException("القيمة المقدمة يجب أن تكون من نوع تعداد (Enum)");

            // تمرير قيمة التعداد كسلسلة نصية إلى السمة Authorize
            Roles = enumValue.ToString();
        }
    }
} 