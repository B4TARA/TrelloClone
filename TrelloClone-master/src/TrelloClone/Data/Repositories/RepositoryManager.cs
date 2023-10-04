using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace TrelloClone.Data.Repositories
{
    public class RepositoryManager
    {
        private TrelloCloneDbContext _db;
        private BoardRepository? _boardRepository;
        private CardRepository? _cardRepository;
        private ColumnRepository? _columnRepository;
        private UserRepository? _userRepository;

        public RepositoryManager(TrelloCloneDbContext db)
        {
            _db = db;
        }

        public BoardRepository BoardRepository
        {
            get
            {
                if (_boardRepository == null)
                    _boardRepository = new BoardRepository(_db);

                return _boardRepository;
            }
        }

        public CardRepository CardRepository
        {
            get
            {
                if (_cardRepository == null)
                    _cardRepository = new CardRepository(_db);

                return _cardRepository;
            }
        }

        public ColumnRepository ColumnRepository
        {
            get
            {
                if (_columnRepository == null)
                    _columnRepository = new ColumnRepository(_db);

                return _columnRepository;
            }
        }

        public UserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_db);

                return _userRepository;
            }
        }   

        public async Task Save() => await _db.SaveChangesAsync();

        public async Task<IDbContextTransaction> BeginTransaction()
        {
            var dbContextTransaction = await _db.Database.BeginTransactionAsync();

            return dbContextTransaction;
        }
    }
}
