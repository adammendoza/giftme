using System;
using System.Collections.Generic;
using Ammeep.GiftRegister.Web.Domain.Logging;
using Ammeep.GiftRegister.Web.Domain.Model;

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
    }

    public class RegistryManager : IRegistryManager
    {
        private readonly IGiftRepository _giftRepository;
        private readonly ILoggingService _loggingService;
        private readonly ICurrentUser _currentUser;

        public RegistryManager(IGiftRepository giftRepository, ILoggingService loggingService,ICurrentUser currentUser)
        {
            _giftRepository = giftRepository;
            _loggingService = loggingService;
            _currentUser = currentUser;
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