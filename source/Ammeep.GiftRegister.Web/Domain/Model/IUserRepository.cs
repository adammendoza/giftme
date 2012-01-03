using System.Collections.Generic;
using Simple.Data;

namespace Ammeep.GiftRegister.Web.Domain.Model
{
    public interface IUserRepository
    {
        bool IsUsernameUnique(string userName);
        void InsertAdminUser(AdminAccount account);
        AdminAccount GetAdminUserByUsername(string userName);
        IEnumerable<AdminAccount> GetAllAdminUsers();
        IEnumerable<GuestAccount> GetAllGuestUsers();
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

        public void InsertAdminUser(AdminAccount account)
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            connection.Account.Insert(account);
        }

        public AdminAccount GetAdminUserByUsername(string userName)
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            var findByUserName = connection.Account.FindByUserName(userName);
            return (AdminAccount) findByUserName;
        }

        public IEnumerable<AdminAccount> GetAllAdminUsers()
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            return connection.Account.FindAllByAccountType(new[] {AccountType.Admin,AccountType.Host}).OrderByAccountTypeDescending().Cast<AdminAccount>();
        }

        public IEnumerable<GuestAccount> GetAllGuestUsers()
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            return connection.Account.FindAllByAccountType(AccountType.Guest).Cast<GuestAccount>();
        }
    }
}