using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using TrelloClone.Models;
using System.Linq;

namespace TrelloClone.Data.Repositories
{
    public class UserRepository : RepositoryBase<User>
    {
        private readonly TrelloCloneDbContext _db;
        public UserRepository(TrelloCloneDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task CreateUser(User entity)
        {
            await Create(entity);
        }

        public async Task<List<User>> GetAllUsers(bool trackChanges)
        {
            return await GetAll(trackChanges);
        }

        public async Task<User?> GetUserById(bool trackChanges, int id)
        {
            Expression<Func<User, bool>> expression = m => m.Id == id;

            List<User> usersByCondition = await GetByCondition(expression, trackChanges);
            if (usersByCondition.Count() == 0)
            {
                return null;
            }
            return usersByCondition.First();
        }

        public async Task<User?> GetUserByName(bool trackChanges, string name)
        {
            Expression<Func<User, bool>> expression = m => m.Name == name;

            List<User> usersByCondition = await GetByCondition(expression, trackChanges);
            if (usersByCondition.Count() == 0)
            {
                return null;
            }
            return usersByCondition.First();
        }

        public async Task<User?> GetUserByEmail(bool trackChanges, string email)
        {
            Expression<Func<User, bool>> expression = m => m.Email == email;

            List<User> usersByCondition = await GetByCondition(expression, trackChanges);
            if (usersByCondition.Count() == 0)
            {
                return null;
            }
            return usersByCondition.First();
        }

        public async Task<User?> GetUserByLogin(bool trackChanges, string? login)
        {
            if (login == null)
            {
                return null;
            }

            Expression<Func<User, bool>> expression = m => m.Login == login;

            List<User> usersByCondition = await GetByCondition(expression, trackChanges);
            if (usersByCondition.Count() == 0)
            {
                return null;
            }

            return usersByCondition.First();
        }

        public void UpdateUser(User entity)
        {
            Update(entity);
        }
    }
}
