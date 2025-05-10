using HSS.System.V2.Domain.Models.Queues;

using System.Linq.Expressions;

namespace HSS.System.V2.Domain.Models.Common;

public interface IQueueIncludeStrategy<TQueue> where TQueue : SystemQueue
{
    Expression<Func<TQueue, object>> GetInclude();
}
