using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TrelloClone.Data.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected TrelloCloneDbContext Db;
        public RepositoryBase(TrelloCloneDbContext db)
        {
            Db = db;
        }

        public async Task<List<T>> GetAll(bool trackChanges) =>
            !trackChanges ?
                await Db.Set<T>()
                    .AsNoTracking().ToListAsync() :
                await Db.Set<T>().ToListAsync();

        public async Task<List<T>> GetByCondition(Expression<System.Func<T, bool>> expression,
            bool trackChanges) =>
            !trackChanges ?
                await Db.Set<T>()
                    .Where(expression)
                    .AsNoTracking().ToListAsync() :
                await Db.Set<T>()
                    .Where(expression).ToListAsync();

        public void Update(T entity) => Db.Set<T>().Update(entity);
        public async Task Create(T entity) => await Db.Set<T>().AddAsync(entity);
        public void Delete(T entity) => Db.Set<T>().Remove(entity);
    }
}
