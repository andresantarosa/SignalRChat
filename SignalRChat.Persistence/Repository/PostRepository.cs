using SignalRChat.Domain.Entities;
using SignalRChat.Domain.Interfaces.Persistence.Repository;
using SignalRChat.Persistence.Base;
using SignalRChat.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRChat.Persistence.Repository
{
    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        public PostRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task Add(Post post) =>
            await DbSet.AddAsync(post);
    }
}
