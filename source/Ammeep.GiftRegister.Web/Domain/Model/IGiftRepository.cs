using System.Collections.Generic;

namespace Ammeep.GiftRegister.Web.Domain.Model
{
    public interface IGiftRepository
    {
        IEnumerable<Category> GetCategories();
        IEnumerable<Gift> GetGifts();
    }
}