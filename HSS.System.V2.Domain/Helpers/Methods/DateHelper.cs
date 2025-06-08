using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Appointments;

using System.Globalization;

namespace HSS.System.V2.Domain.Helpers.Methods;

public static class DateHelper
{
    public static IQueryable<TApp> FilterByDate<TApp>
        (this IQueryable<TApp> query, DateTime? dateFrom = null, DateTime? dateTo = null) where TApp : Appointment
    {
        if (dateFrom.HasValue)
        {
            if (dateTo.HasValue)
            {
                return query.Where(a => a.SchaudleStartAt >= dateFrom.Value && a.SchaudleStartAt <= dateTo);
            }
            else
                return query.Where(a => a.SchaudleStartAt >= dateFrom.Value);
        }
        return query;
    }

    public static IQueryable<TApp> FilterByDate<TApp>
       (this IQueryable<TApp> query, DateFilterationRequest filters) where TApp : Appointment
    {
        var dateFrom = filters.DateFrom;
        var dateTo = filters.DateTo;
        if (dateFrom.HasValue)
        {
            if (dateTo.HasValue)
            {
                return query.Where(a => a.SchaudleStartAt.Date >= dateFrom.Value.Date && a.SchaudleStartAt.Date <= dateTo.Value.Date);
            }
            else
                return query.Where(a => a.SchaudleStartAt.Date >= dateFrom.Value.Date);
        }
        return query;
    }
}

public static class HelperDate
{
    /// <summary>
    /// Summary:
    ///  you will get the current datetime based on timezone the default timezone is EGYPT
    ///  (+2 GMT) get the time according to timezone</summary>
    /// <param name="timeZone"></param>
    /// <returns> Get current date</returns>
    public static DateTime GetCurrentDate(int timeZone = 2)
    {
        return DateTime.UtcNow.AddHours(timeZone);
    }

    // Summary:
    //     Get the datetime of string date the default format is : dd/MM/yyyy
    public static DateTime ConvertFromStringToDate(this string date, string format = "dd/MM/yyyy")
    {
        return DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
    }

    //
    // Summary:
    //     It take the date to get how much was the time is passed it could be in Arabic
    //     (ar) or English (en) language. the default language is Arabic (ar)
    //
    // Parameters:
    //   date:
    //
    //   lang:
    //
    // Returns:
    //     Get the date in a period format (2 hours)
    public static string FormatDate(DateTime date, string lang = "ar")
    {
        TimeSpan timeSpan = GetCurrentDate() - date;
        string result = "5 ث";
        if (timeSpan.Days < 10)
        {
            if (timeSpan.Days > 0)
            {
                return (lang == "ar") ? ("منذ " + timeSpan.Days + " يوم ") : ("from " + timeSpan.Days + " day ");
            }

            if (timeSpan.Hours > 0)
            {
                return (lang == "ar") ? ("منذ " + timeSpan.Hours + " ساعة ") : ("from " + timeSpan.Hours + " Hour ");
            }

            if (timeSpan.Minutes > 0)
            {
                return (lang == "ar") ? (" منذ " + timeSpan.Minutes + " دقيقة ") : (" from " + timeSpan.Minutes + " minute ");
            }

            return result;
        }

        return (lang == "ar") ? (date.Date.Day + "/" + date.Date.Month + "/" + date.Date.Year) : (date.Date.Day + "/" + date.Date.Month + "/" + date.Date.Year);
    }

    //
    // Summary:
    //     Get the days as date between startDate and endDate
    //
    // Parameters:
    //   startDate:
    //
    //   endDate:
    //
    // Returns:
    //     List of dates
    public static List<DateTime> GetDatesBetween(DateTime startDate, DateTime endDate)
    {
        List<DateTime> list = new List<DateTime>();
        DateTime dateTime = startDate;
        while (dateTime <= endDate)
        {
            list.Add(dateTime);
            dateTime = dateTime.AddDays(1.0);
        }

        return list;
    }

    //
    // Summary:
    //     Get the expected time period starting from current datetime it support two languages
    //     Arabic (ar) and English (en)
    //
    // Parameters:
    //   date:
    //
    //   lang:
    //
    // Returns:
    //     Get the expected time period in format (remaining)
    public static string ExpectedDate(this DateTime date, string lang = "ar")
    {
        TimeSpan timeSpan = date - GetCurrentDate();
        string result = "1 د";
        if (timeSpan.Days < 10)
        {
            if (timeSpan.Days > 0)
            {
                return (lang == "ar") ? ("متبقى " + timeSpan.Days + " يوم ") : ("remaining " + timeSpan.Days + " day ");
            }

            if (timeSpan.Hours > 0)
            {
                return (lang == "ar") ? ("متبقى " + timeSpan.Hours + " ساعة ") : ("remaining " + timeSpan.Hours + " Hour ");
            }

            if (timeSpan.Minutes > 0)
            {
                return (lang == "ar") ? (" متبقى " + timeSpan.Minutes + " دقيقة ") : (" remaining " + timeSpan.Minutes + " minute ");
            }

            return result;
        }

        return (lang == "ar") ? (date.Date.Day + "/" + date.Date.Month + "/" + date.Date.Year) : (date.Date.Day + "/" + date.Date.Month + "/" + date.Date.Year);
    }

    //
    // Summary:
    //     This method is to check if the string date that passed have a valid format or
    //     not
    //
    // Parameters:
    //   Date:
    //
    //   format:
    //
    // Returns:
    //     Get the result of date format which is valid or not
    public static bool IsDateValid(string Date, string format)
    {
        if (DateTime.TryParseExact(Date, format, null, DateTimeStyles.None, out var _))
        {
            return true;
        }

        return false;
    }
}