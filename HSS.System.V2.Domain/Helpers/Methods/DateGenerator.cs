using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSS.System.V2.Domain.Helpers.Methods
{
    public class DateGenerator
    {
        public static DateTime GetCurrentDateTimeByRegion(Region region)
        {
            switch (region)
            {
                case Region.Egypt:
                    var egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
                    return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);
                default:
                    throw new NotSupportedException($"The region {region} is not supported.");
            }
        }
    }

        public enum Region
    {
        Egypt,
    }
}
