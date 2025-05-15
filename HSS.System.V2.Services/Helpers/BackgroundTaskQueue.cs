using System.Threading.Channels;

namespace HSS.System.V2.Services.Helpers
{
    public partial class AccountServiceHelper
    {
        public class BackgroundTaskQueue : IBackgroundTaskQueue
        {
            private readonly Channel<Func<CancellationToken, Task>> _queue = Channel.CreateUnbounded<Func<CancellationToken, Task>>();

            public void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem)
            {
                _queue.Writer.TryWrite(workItem);
            }

            public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
            {
                var workItem = await _queue.Reader.ReadAsync(cancellationToken);
                return workItem;
            }

        }
       


    }
}
