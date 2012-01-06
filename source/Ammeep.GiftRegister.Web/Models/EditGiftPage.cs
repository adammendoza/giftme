using System.Collections.Generic;
using System.Web.Mvc;
using Ammeep.GiftRegister.Web.Domain.Model;

namespace Ammeep.GiftRegister.Web.Models
{
    public class EditGiftPage 
    {
        public EditGiftPage(Gift gift, IEnumerable<Category> categories)
        {
            Gift =gift;
            CategoriesSelectList = new SelectList(categories,"CategoryId","Name");
        }

        public SelectList CategoriesSelectList { get; set; }

        public Gift Gift { get; set; }
    }
}