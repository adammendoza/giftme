using System.Collections.Generic;
using Simple.Data;

namespace Ammeep.GiftRegister.Web.Domain.Model
{
    public interface IUserRepository
    {
        bool IsUsernameUnique(string userName);
        void InsertAdminUser(EventHostAccount account);
        EventHostAccount GetAdminUserByUsername(string userName);
        IEnumerable<EventHostAccount> GetAllEventHostUsers();
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
            return (connection.Account.FindByUserName(userName)) == null;
        }

        public void InsertAdminUser(EventHostAccount account)
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            connection.Account.Insert(account);
        }

        public EventHostAccount GetAdminUserByUsername(string userName)
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            var findByUserName = connection.Account.FindByUserName(userName);
            return (EventHostAccount) findByUserName;
        }

        public IEnumerable<EventHostAccount> GetAllEventHostUsers()
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            return connection.Account.FindAllByAccountType(AccountType.Host).Cast<EventHostAccount>();
        }
    }
}