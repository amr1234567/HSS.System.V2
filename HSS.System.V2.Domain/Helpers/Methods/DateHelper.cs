using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Appointments;

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
                return query.Where(a => a.SchaudleStartAt >= dateFrom.Value && a.SchaudleStartAt <= dateTo);
            }
            else
                return query.Where(a => a.SchaudleStartAt >= dateFrom.Value);
        }
        return query;
    }
}
