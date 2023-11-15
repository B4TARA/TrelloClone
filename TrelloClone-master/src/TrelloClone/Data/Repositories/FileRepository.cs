using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using TrelloClone.Models;
using System.Linq;

namespace TrelloClone.Data.Repositories
{
    public class FileRepository : RepositoryBase<File>
    {
        private readonly TrelloCloneDbContext _db;
        public FileRepository(TrelloCloneDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<File?> GetFileById(bool trackChanges, int id)
        {
            Expression<Func<File, bool>> expression = m => m.Id == id;

            List<File> filesByCondition = await GetByCondition(expression, trackChanges);
            if (filesByCondition.Count() == 0)
            {
                return null;
            }
            return filesByCondition.First();
        }     
    }
}
