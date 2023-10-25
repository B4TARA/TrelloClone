using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using TrelloClone.Models;
using System.Linq;

namespace TrelloClone.Data.Repositories
{
    public class CardRepository : RepositoryBase<Card>
    {
        private readonly TrelloCloneDbContext _db;
        public CardRepository(TrelloCloneDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Card?> GetCardById(bool trackChanges, int id)
        {
            Expression<Func<Card, bool>> expression = m => m.Id == id;

            List<Card> cardsByCondition = await GetByCondition(expression, trackChanges);
            if (cardsByCondition.Count() == 0)
            {
                return null;
            }
            return cardsByCondition.First();
        }
    }
}
