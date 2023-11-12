using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using TrelloClone.Models;

namespace TrelloClone.Data.Repositories
{
    public class CommentRepository : RepositoryBase<Comment>
    {
        private readonly TrelloCloneDbContext _db;
        public CommentRepository(TrelloCloneDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<Comment>> GetCardComments(bool trackChanges, int id)
        {
            Expression<Func<Comment, bool>> expression = m => m.CardId == id;

            List<Comment> commentsByCondition = await GetByCondition(expression, trackChanges);
           
            return commentsByCondition;
        }
    }
}
