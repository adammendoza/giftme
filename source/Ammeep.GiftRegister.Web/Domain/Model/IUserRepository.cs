using System.Collections.Generic;
using Simple.Data;

namespace Ammeep.GiftRegister.Web.Domain.Model
{
    public interface IUserRepository
    {
        bool IsUsernameUnique(string userName);
        void InsertAdminUser(Account account);
        Account GetAdminUserByUsername(string userName);
        IEnumerable<Account> GetAllAdminUsers();
        IEnumerable<Account> GetAllGuestUsers();
        Account GetAccountById(int accountId);
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

        public void InsertAdminUser(Account account)
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            connection.Account.Insert(account);
        }

        public Account GetAdminUserByUsername(string userName)
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            var findByUserName = connection.Account.FindByUserName(userName);
            return (Account)findByUserName;
        }

        public IEnumerable<Account> GetAllAdminUsers()
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            return connection.Account.FindAllByAccountType(new[] { AccountType.Admin, AccountType.Host }).OrderByAccountTypeDescending().Cast<Account>();
        }

        public IEnumerable<Account> GetAllGuestUsers()
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            return connection.Account.FindAllByAccountType(AccountType.Guest).Cast<Account>();
        }

        public Account GetAccountById(int accountId)
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            var findByUserName = connection.Account.FindByAccountId(accountId);
            return (Account)findByUserName;
        }
    }
}