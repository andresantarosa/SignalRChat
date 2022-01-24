using Microsoft.EntityFrameworkCore;
using SignalRChat.Domain.Interfaces.Persistence.Base;
using SignalRChat.Persistence.Data;

namespace SignalRChat.Persistence.Base
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected ApplicationDbContext Context;
        protected DbSet<TEntity> DbSet;

        public RepositoryBase(ApplicationDbContext context)
        {
            Context = context;
            DbSet = Context.Set<TEntity>();
        }
    }
}
