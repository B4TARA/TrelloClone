using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using TrelloClone.Models;
using System.Linq;

namespace TrelloClone.Data.Repositories
{
    public class ColumnRepository : RepositoryBase<Column>
    {
        private readonly TrelloCloneDbContext _db;
        public ColumnRepository(TrelloCloneDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Column?> GetColumnById(bool trackChanges, int id)
        {
            Expression<Func<Column, bool>> expression = m => m.Id == id;

            List<Column> columnsByCondition = await GetByCondition(expression, trackChanges);
            if (columnsByCondition.Count() == 0)
            {
                return null;
            }
            return columnsByCondition.First();
        }
    }
}
