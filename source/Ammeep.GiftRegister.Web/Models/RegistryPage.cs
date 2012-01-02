using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ammeep.GiftRegister.Web.Domain.Model;

namespace Ammeep.GiftRegister.Web.Models
{
    public class RegistryPage
    {
        public RegistryPage(IEnumerable<Gift> gifts, IEnumerable<Category> categories,int pageSize)
        {
            Gifts =gifts.Select((gift, i) => new GiftRow {Item = gift, IsFirst = i == 0});
            CategoriesSelectList = new SelectList(categories,"CategoryId","Name");
            PageSize = pageSize;
        }

        public IEnumerable<GiftRow> Gifts { get; private set; }   
        public int PageSize { get; private set; }
        public SelectList CategoriesSelectList { get; private set; }
   

    }
}