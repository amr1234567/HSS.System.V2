using HSS.System.V2.Domain.Models.Queues;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSS.System.V2.Domain.Models.Common
{
    public interface IAppointmentModel<TQueue> where TQueue : SystemQueue
    {
        void SetQueue(TQueue? queue);
        TQueue? GetQueue();
    }
}
