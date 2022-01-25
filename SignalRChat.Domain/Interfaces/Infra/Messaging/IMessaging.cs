using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRChat.Domain.Interfaces.Infra.Messaging
{
    public interface IMessaging
    {
        void Enqueue<T>(string queue, T message);
    }
}
