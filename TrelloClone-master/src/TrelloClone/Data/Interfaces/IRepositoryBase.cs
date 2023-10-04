using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TrelloClone.Data.Repositories
{
    public interface IRepositoryBase<T>
    {
        Task<List<T>> GetAll(bool trackChanges);
        Task<List<T>> GetByCondition(Expression<System.Func<T, bool>> expression, bool trackChanges);
        Task Create(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
