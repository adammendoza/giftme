using System.Collections.Generic;
using System.Linq;
using Ammeep.GiftRegister.Web.Domain.Model;

namespace Ammeep.GiftRegister.Web.Models
{
    public class GiftStatusesPage
    {
        private IEnumerable<Gift> Gifts { get; set; }

        public IEnumerable<Gift> GiftsPendingReservation
        {
            get { return Gifts.Where(gift => gift.PendingReservation); }
        }

        public IEnumerable<Gift> GiftsConfirmedAsReserved
        {
            get { return Gifts.Where(gift => gift.Reserved); }
        }

        public IEnumerable<Gift> GiftsNotActive
        {
            get { return Gifts.Where(gift => !gift.IsActive); }
        }

        public GiftStatusesPage(IEnumerable<Gift> gifts)
        {
            Gifts = gifts;
        }
    }
}