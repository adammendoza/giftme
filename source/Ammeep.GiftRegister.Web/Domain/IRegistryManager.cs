using System;
using System.Collections.Generic;
using Ammeep.GiftRegister.Web.Domain.Logging;
using Ammeep.GiftRegister.Web.Domain.Model;
using Ammeep.GiftRegister.Web.Models;

namespace Ammeep.GiftRegister.Web.Domain
{
    public interface IRegistryManager
    {
        IEnumerable<Gift> GetRegistry();
        IEnumerable<Gift> GetRegisrty(int categoryId);
        IPagedList<Gift> GetRegistry(int pageSize, int pageNumber);
        IPagedList<Gift> GetRegistry(int pageSize, int pageNumber, int categoryId);
        Gift GetGift(int giftId);
        void UpdateGift(Gift gift);
        void DeleteGift(int giftId);
        IEnumerable<Category> GetCategories();
        void AddNewGift(Gift gift);
        void ReserveGift(string guestName, string guestEmail, int giftId, int quantityReserved);
        ReservationConfirmationPage ConfirmReservation(Guid confirmationId);
    }

    public class RegistryManager : IRegistryManager
    {
        private readonly IGiftRepository _giftRepository;
        private readonly ILoggingService _loggingService;
        private readonly ICurrentUser _currentUser;
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;

        public RegistryManager(IGiftRepository giftRepository, ILoggingService loggingService,ICurrentUser currentUser, IUserRepository userRepository, IMailService mailService)
        {
            _giftRepository = giftRepository;
            _loggingService = loggingService;
            _currentUser = currentUser;
            _userRepository = userRepository;
            _mailService = mailService;
        }

        public IEnumerable<Category> GetCategories()
        {
            _loggingService.LogDebug("Retrieving all gift categories");
            return _giftRepository.GetCategories();
        }

        public void AddNewGift(Gift gift)
        {
            _loggingService.LogInformation(string.Format("User {0}({1}) is inserting a new gift.",_currentUser.UserName,_currentUser.AccountId));
            gift.CreatedDate = DateTime.Now;
            gift.CreatedBy = _currentUser.AccountId;
            gift.LastUpdatedBy = _currentUser.AccountId;
            gift.LastUpdatedDate = DateTime.Now;
            gift.QuantityRemaining = gift.QuantityRequired;
            gift.IsActive = true;
            _giftRepository.InsertGift(gift);
        }

        public void ReserveGift(string guestName, string guestEmail, int giftId, int quantityReserved)
        {
            _loggingService.LogInformation(string.Format("Guest {0}({1}) is reserving {2} of gift {3}", guestName,guestEmail,giftId,quantityReserved));
            Guest guest = new Guest{Email = guestEmail, Name = guestName,CreatedDate = DateTime.Now};           
            var giftPurchase = new GiftPruchase(giftId,quantityReserved);
            _userRepository.InserstGuestGiftReservation(guest, giftPurchase);
            Gift gift = _giftRepository.GetGift(giftId);

            _mailService.SendPurchaseConfirmationEmail(guest, giftPurchase, gift);
        }

        public ReservationConfirmationPage ConfirmReservation(Guid confirmationId)
        {
           _loggingService.LogInformation(string.Format("Confirming reservation {0}",confirmationId));
            GiftPruchase reservation = _userRepository.GetGiftReservationByConfirmationId(confirmationId);
            if (reservation != null && !reservation.Confirmed)
            {
                return CompleteReservation(reservation);              
            }
            if(reservation == null)
            {
                return new ReservationConfirmationPage { ConfirmationErrors = new List<string> { "Reservation Not found" } };
            }
            return new ReservationConfirmationPage{ConfirmationErrors = new List<string>{"Already confirmed"}};
        }

        private ReservationConfirmationPage CompleteReservation(GiftPruchase reservation)
        {
            ReservationConfirmationPage reservationConfirmationPage = new ReservationConfirmationPage();
            reservation.Confirmed = true;
            reservation.ConfirmedOn = DateTime.Now;
            _userRepository.UpdateGiftReservation(reservation);
            Gift gift = _giftRepository.GetGift(reservation.GiftId);
            gift.QuantityRemaining -= reservation.Quantity;
            gift.IsActive = gift.QuantityRemaining > 0;
            _giftRepository.UpdateGift(gift);
            Guest guest = _userRepository.GetGuestById(reservation.GuestId);
            reservationConfirmationPage.Guest = guest;
            reservationConfirmationPage.GiftPruchase = reservation;
            reservationConfirmationPage.ReservedGift = gift;
            return reservationConfirmationPage;
        }

        public IEnumerable<Gift> GetRegistry()
        {
            return _giftRepository.GetGifts();
        }

        public IEnumerable<Gift> GetRegisrty(int categoryId)
        {
            _loggingService.LogDebug(string.Format("Retrieving all gifts in category {0}",categoryId));
            return _giftRepository.GetAllGiftsForCategory(categoryId);
        }

        public IPagedList<Gift> GetRegistry(int pageSize, int pageNumber)
        {
            _loggingService.LogDebug(string.Format("Retrieving {0} gifts from page {1} in all categories", pageSize, pageNumber));
            return _giftRepository.GetPagedGifts(pageSize,pageNumber);
        }

        public IPagedList<Gift> GetRegistry(int pageSize, int pageNumber, int categoryId)
        {
            _loggingService.LogDebug(string.Format("Retrieving {0} gifts from page {1} in category {2}", pageSize,pageNumber,categoryId));
            return categoryId == 0 ? _giftRepository.GetPagedGifts(pageSize, pageNumber) : _giftRepository.GetPagedGiftsForCategory(pageSize, pageNumber, categoryId);
        }

        public Gift GetGift(int giftId)
        {
            _loggingService.LogDebug(string.Format("Retrieving gift id {0}", giftId));
            return _giftRepository.GetGift(giftId);
        }

        public void UpdateGift(Gift gift)
        {
            _loggingService.LogInformation(string.Format("Updating gift id {0}", gift.GiftId));
            gift.LastUpdatedDate = DateTime.Now;
            gift.LastUpdatedBy = _currentUser.AccountId;
            _giftRepository.UpdateGift(gift);
        }

        public void DeleteGift(int giftId)
        {
            _loggingService.LogInformation(string.Format("Setting gift id {0} to inactive", giftId));
            _giftRepository.DeactivateGift (giftId,_currentUser.AccountId,DateTime.Now);
        }

        
    }
}