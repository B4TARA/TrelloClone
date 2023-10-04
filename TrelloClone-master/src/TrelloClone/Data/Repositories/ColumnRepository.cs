using TrelloClone.Models;

namespace TrelloClone.Data.Repositories
{
    public class ColumnRepository : RepositoryBase<Column>
    {
        private readonly TrelloCloneDbContext _db;
        public ColumnRepository(TrelloCloneDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
