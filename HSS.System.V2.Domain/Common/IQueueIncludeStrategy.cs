using HSS.System.V2.Domain.Queues;

using System.Linq.Expressions;

namespace HSS.System.V2.Domain.Common;

public interface IQueueIncludeStrategy<TQueue> where TQueue : SystemQueue
{
    Expression<Func<TQueue, object>> GetInclude();
}
