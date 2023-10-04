using TrelloClone.Models;

namespace TrelloClone.Data.Repositories
{
    public class BoardRepository : RepositoryBase<Board>
    {
        private readonly TrelloCloneDbContext _db;
        public BoardRepository(TrelloCloneDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
