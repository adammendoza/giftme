using System.Collections.Generic;
using System.Web.Mvc;
using Ammeep.GiftRegister.Web.Domain.Model;

namespace Ammeep.GiftRegister.Web.Models
{
    public class RegistryPage
    {
        public IEnumerable<Gift> Gifts { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        public SelectList CategoriesSelectList
        {
            get { return new SelectList(Categories,"CategoryId","Name");}
        }
    }
}