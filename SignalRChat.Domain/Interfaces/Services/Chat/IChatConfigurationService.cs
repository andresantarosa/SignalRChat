using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRChat.Domain.Interfaces.Services.Chat
{
    public interface IChatConfigurationService
    {
        int GetMessageLimit();
    }
}
