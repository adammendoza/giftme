using System;
using System.Collections.Generic;
using Simple.Data;

namespace Ammeep.GiftRegister.Web.Domain.Model
{
    public interface IUserRepository
    {
        bool IsUsernameUnique(string userName);
        Account InsertAdminUser(Account account);
        Account GetAdminUserByUsername(string userName);
        IEnumerable<Account> GetAllAdminUsers();
        IEnumerable<Account> GetAllGuestUsers();
        Account GetAccountById(int accountId);
        void InserstGuestGiftReservation(Guest guest, GiftPruchase pruchase);
        GiftPruchase GetGiftReservationByConfirmationId(Guid confirmationId);
        void UpdateGiftReservation(GiftPruchase reservation);
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

        public Account InsertAdminUser(Account account)
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            return connection.Account.Insert(account);
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

        public void InserstGuestGiftReservation(Guest guest, GiftPruchase pruchase)
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            Guest savedGuest = connection.Guest.Insert(guest);
            pruchase.GuestId = savedGuest.GuestId;
            connection.GiftPurchase.Insert(pruchase);
        }

        public GiftPruchase GetGiftReservationByConfirmationId(Guid confirmationId)
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            var reservation = connection.GiftPurchase.FindByConfirmationId(confirmationId);
            return (GiftPruchase) reservation;
        }

        public void UpdateGiftReservation(GiftPruchase reservation)
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            connection.GiftPurchase.Update(reservation);
        }
    }
}