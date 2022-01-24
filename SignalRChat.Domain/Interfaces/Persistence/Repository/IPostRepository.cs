using SignalRChat.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRChat.Domain.Interfaces.Persistence.Repository
{
    public interface IPostRepository
    {
        Task Add(Post post);
    }
}
