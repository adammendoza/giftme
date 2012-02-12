using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Simple.Data;

namespace Ammeep.GiftRegister.Web.Domain.Model
{
    public class GiftRepository : IGiftRepository
    {
        private readonly IConfiguration _configuration;

        public GiftRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<Category> GetCategories()
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            return connection.Category.All().OrderByName().Cast<Category>();       
        }

        public IEnumerable<Gift> GetGifts()
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            return connection.Gifts.All().Cast<Gift>();           
        }

        public IEnumerable<Gift> GetAllGiftsForCategory(int categoryId)
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            return connection.Gifts.FindAllByCategoryAndIsActive(categoryId).Cast<Gift>();
        }

        public IPagedList<Gift> GetPagedGifts(int pageSize, int pageNumber)
        {
            int shiftedPageNum = pageNumber > 0 ? pageNumber-- : pageNumber;
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            IEnumerable<Gift> page = connection.Gifts.FindAllByIsActiveAndReservedAndPendingReservation(true, false,false).Skip(shiftedPageNum).Take(pageSize).Cast<Gift>();
            int totalNumberOfGifts = connection.Gifts.FindAllByIsActiveAndReservedAndPendingReservation(true, false,false).Count();
            return new PagedList<Gift>(page, pageNumber, pageSize, totalNumberOfGifts);
        }

        public IPagedList<Gift> GetPagedGiftsForCategory(int pageSize, int pageNumber, int categoryId)
        {
            int shiftedPageNum = pageNumber > 0 ? pageNumber-- : pageNumber;
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            IEnumerable<Gift> page = connection.Gifts.FindAllByIsActiveAndCategoryAndReservedAndPendingReservation(true, categoryId,false, false).Skip(shiftedPageNum).Take(pageSize).Cast<Gift>();
            int totalNumberOfGifts = connection.Gifts.FindAllByIsActiveAndCategoryAndReservedAndPendingReservation(true, categoryId, false, false).Count();
            return new PagedList<Gift>(page, pageNumber, pageSize, totalNumberOfGifts);
        }

        public Gift GetGift(int giftId)
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            return connection.Gifts.FindByGiftId(giftId);
        }

        public void UpdateGift(Gift gift)
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            connection.Gifts.Update(gift);
        }

        public void DeactivateGift(int giftId, int updatedByAccountId, DateTime updatedDateTime)
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            connection.Gifts.UpdateByGiftId(GiftId: giftId, LastUpdatedDate: updatedDateTime, LastUpdatedBy: updatedByAccountId, IsActive:false);
        }

        public void ReactivateGift(int giftId, int updatedByAccountId, DateTime updatedDateTime)
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            connection.Gifts.UpdateByGiftId(GiftId: giftId, LastUpdatedDate: updatedDateTime, LastUpdatedBy: updatedByAccountId, IsActive: true);
        }

        public void InsertGift(Gift gift)
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            connection.Gifts.Insert(gift);
        }

        public IEnumerable<Gift> GetDeactivatedGifts()
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            return connection.Gifts.FindAllByIsActive(false).Cast<Gift>();
        }

        public IEnumerable<PendingGift> GetPendingGifts()
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            var giftPurchases = connection.GiftPurchase.FindAllByConfirmedAndIsActive(false,true).Cast<GiftPruchase>();
            List<PendingGift> pendingGifts = new List<PendingGift>();
            foreach (var pruchase in giftPurchases)
            {
                PendingGift pendingGift = new PendingGift();
                pendingGift.GiftPruchase = pruchase;
                pendingGift.Guest = (Guest) connection.Guest.FindByGuestId(pruchase.GuestId);
                pendingGift.Gift =(Gift) connection.Gifts.FindByGiftId(pruchase.GiftId);
                pendingGifts.Add(pendingGift);
            }
            return pendingGifts.ToList();
        }

        public IEnumerable<ReservedGift> GetConfirmedGifts()
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            var giftPurchases = connection.GiftPurchase.FindAllByConfirmed(true).Cast<GiftPruchase>();
            List<ReservedGift> pendingGifts = new List<ReservedGift>();
            foreach (var pruchase in giftPurchases)
            {
                ReservedGift pendingGift = new ReservedGift();
                pendingGift.GiftPruchase = pruchase;
                pendingGift.Guest = (Guest)connection.Guest.FindByGuestId(pruchase.GuestId);
                pendingGift.Gift = (Gift)connection.Gifts.FindByGiftId(pruchase.GiftId);
                pendingGifts.Add(pendingGift);
            }
            return pendingGifts;
        }

        public void RemovePendingStatus(int giftId, int updatedByAccountId, DateTime updatedDateTime)
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            connection.Gifts.UpdateByGiftId(GiftId: giftId, LastUpdatedDate: updatedDateTime, LastUpdatedBy: updatedByAccountId, PendingReservation: false);
        }

        public void DeactivateGiftPurhcase(int giftPurchaseId, int updatedByAccountId, DateTime updatedDateTime)
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            connection.GiftPurchase.UpdateByGiftPurchaseId(GiftPurchaseId: giftPurchaseId, LastUpdatedDate: updatedDateTime, LastUpdatedBy: updatedByAccountId, IsActive: false);

        }

        public GiftPruchase GetGiftPurchase(int giftPurhcaseId)
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            return connection.GiftPurchase.FindByGiftPurchaseId(giftPurhcaseId);
        }
    }

    public interface IPagedList<T> : IEnumerable<T>
    {
        int FirstItemIndex { get; }
        int LastItemIndex { get; }
        int PageCount { get; }
        int TotalItemCount { get; }
        int PageIndex { get; }
        int PageNumber { get; }
        int PageSize { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
        bool IsFirstPage { get; }
        bool IsLastPage { get; }
        int Count { get; }
    }

    public class PagedList<T> : List<T>, IPagedList<T>
    {
        public PagedList(IEnumerable<T> source, int index, int pageSize, int totalItemCount)
        {
            Initialize(source, index, pageSize, totalItemCount);
        }

        public PagedList(IEnumerable<T> source, int? index, int pageSize): this(source, index ?? 0, pageSize)
        {
        }

        public PagedList(IEnumerable<T> source, int index, int pageSize)
        {
            if (source is IQueryable<T>)
                Initialize((IQueryable<T>)source, index, pageSize);
            else
                Initialize(source.AsQueryable(), index, pageSize);
        }

       

        public int FirstItemIndex { get; private set; }
        public int LastItemIndex { get; private set; }
        public int PageCount { get; private set; }
        public int TotalItemCount { get; private set; }
        public int PageIndex { get; private set; }

        public int PageNumber
        {
            get { return PageIndex + 1; }
        }

        public int PageSize { get; private set; }
        public bool HasPreviousPage { get; private set; }
        public bool HasNextPage { get; private set; }
        public bool IsFirstPage { get; private set; }
        public bool IsLastPage { get; private set; }

  

        protected void Initialize(IEnumerable<T> source, int index, int pageSize, int totalItemCount)
        {
            if (source == null)
                source = new List<T>();

            CalculateProperties(totalItemCount, index, pageSize);

            AddRange(source);
        }

        protected void Initialize(IQueryable<T> source, int index, int pageSize)
        {
            if (source == null)
                source = new List<T>().AsQueryable();

            int totalItemCount = source.Count();

            CalculateProperties(totalItemCount, index, pageSize);

            if (TotalItemCount > 0)
            {
                AddRange(source.Skip((index) * pageSize).Take(pageSize).ToList());
            }
        }

        private void CalculateProperties(int totalItemCount, int index, int pageSize)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "PageIndex cannot be below 0.");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException("index", "PageSize cannot be less than 1.");

            TotalItemCount = totalItemCount;
            PageSize = pageSize;
            PageIndex = index;
            if (TotalItemCount > 0)
            {
                PageCount = (int)Math.Ceiling(TotalItemCount / (double)PageSize);
                FirstItemIndex = (PageIndex * PageSize) + 1;
                LastItemIndex = Math.Min(((PageIndex + 1) * PageSize), TotalItemCount);
            }
            else
            {
                PageCount = 0;
                FirstItemIndex = 0;
                LastItemIndex = 0;
            }
            HasPreviousPage = (PageIndex > 0);
            HasNextPage = (PageIndex < (PageCount - 1));
            IsFirstPage = (PageIndex <= 0);
            IsLastPage = (PageIndex >= (PageCount - 1));
        }
    }

}