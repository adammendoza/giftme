using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ammeep.GiftRegister.Web.Domain.Model;

namespace Ammeep.GiftRegister.Web.Models
{
    public class RegistryItemsPage
    {

        public RegistryItemsPage(IPagedList<Gift> gifts, IEnumerable<Category> categories, int pageSize,int selectedCategory)
        {
            Gifts =gifts.Select((gift, i) => new GiftRow {Item = gift, IsFirst = i == 0});
            Categories = categories.ToList();
            TotalNumberOfItems = gifts.TotalItemCount;
            PageNumber = gifts.PageNumber;
            PageCount = gifts.PageCount;
            FirstItemIndex = gifts.FirstItemIndex;
            LastItemIndex = gifts.LastItemIndex;           
            PageSize = pageSize;
            SelectedCategoryId = selectedCategory;
        }

        public List<Category> Categories { get; set; }

        protected int LastItemIndex { get; set; }

        protected int FirstItemIndex { get; set; }

        public int PageCount { get; set; }

        public int PageNumber { get; set; }

        public int TotalNumberOfItems { get; set; }

        public IEnumerable<GiftRow> Gifts { get; private set; }   
        public int PageSize { get; private set; }
        public SelectList CategoriesSelectList { get; private set; }

        public bool HasItemsToDisplay
        {
            get { return Gifts != null && Gifts.Any(); }
        }

        public int SelectedCategoryId { get; private set; }
       
        public string PageSummary()
        {
            return string.Format(
                "Displaying {0}-{1} of {2} Items",
                FirstItemIndex,
                LastItemIndex,
                TotalNumberOfItems);

        }
    }
}