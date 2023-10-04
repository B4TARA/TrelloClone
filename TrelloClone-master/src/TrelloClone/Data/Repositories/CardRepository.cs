using TrelloClone.Models;

namespace TrelloClone.Data.Repositories
{
    public class CardRepository : RepositoryBase<Card>
    {
        private readonly TrelloCloneDbContext _db;
        public CardRepository(TrelloCloneDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
