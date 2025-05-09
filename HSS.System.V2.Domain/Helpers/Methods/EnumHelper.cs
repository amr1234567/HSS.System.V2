using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HSS.System.V2.Domain.Helpers.Methods
{
    public static class EnumHelper
    {
        public static string GetEnumMemberValue(this Enum? enumValue)
        {
            if (enumValue == null)
                return "";
            var enumType = enumValue.GetType();
            var memberInfo = enumType.GetMember(enumValue.ToString());

            if (memberInfo.Length > 0)
            {
                var attr = memberInfo[0].GetCustomAttribute<EnumMemberAttribute>();
                if (attr != null)
                {
                    return attr.Value; // Return the EnumMember value if found
                }
            }

            return enumValue.ToString(); // Fallback to default enum name
        }

    }
}
