using System;
using System.Collections.Generic;

namespace Ammeep.GiftRegister.Web.Domain.Model
{
    public interface IGiftRepository
    {
        IEnumerable<Category> GetCategories();
        IEnumerable<Gift> GetGifts();
        IEnumerable<Gift> GetAllGiftsForCategory(int categoryId);
        IPagedList<Gift> GetPagedGifts(int pageSize, int pageNumber);
        IPagedList<Gift> GetPagedGiftsForCategory(int pageSize, int pageNumber, int categoryId);
        Gift GetGift(int giftId);
        void UpdateGift(Gift gift);
        void DeactivateGift(int giftId, int updatedByAccountId, DateTime updatedDateTime);
        void ReactivateGift(int giftId, int updatedByAccountId, DateTime updatedDateTime);
        void InsertGift(Gift gift);
        IEnumerable<Gift> GetDeactivatedGifts();
        IEnumerable<PendingGift> GetPendingGifts();
        IEnumerable<ReservedGift> GetConfirmedGifts();
        void RemovePendingStatus(int giftId, int updatedByAccountId, DateTime updatedDateTime);
        void DeactivateGiftPurhcase(int giftPurchaseId, int updatedByAccountId, DateTime updatedDateTime);
        GiftPruchase GetGiftPurchase(int giftPurhcaseId);
    }
}