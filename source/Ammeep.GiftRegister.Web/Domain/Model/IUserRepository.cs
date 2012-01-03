using System.Collections.Generic;
using Simple.Data;

namespace Ammeep.GiftRegister.Web.Domain.Model
{
    public interface IUserRepository
    {
        bool IsUsernameUnique(string userName);
        void InsertUser(User user);
        User GetUserByUserName(string userName);
        IEnumerable<User> GetAllUsers();
    }

    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool IsUsernameUnique(string userName)
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            return (connection.Users.FindByUserName(userName)) == null;
        }

        public void InsertUser(User user)
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            connection.Users.Insert(user);
        }

        public User GetUserByUserName(string userName)
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            var findByUserName = connection.Users.FindByUserName(userName);
            return (User) findByUserName;
        }

        public IEnumerable<User> GetAllUsers()
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            return connection.Users.All().Cast<User>();
        }
    }
}