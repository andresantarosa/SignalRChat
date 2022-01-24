using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRChat.Domain.Interfaces.Persistence.UoW
{
    public interface IUnitOfWork
    {
        Task<(bool success, string error)> Save();
    }
}
