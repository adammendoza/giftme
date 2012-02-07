using System.Collections.Generic;
using System.Web.Mvc;
using Ammeep.GiftRegister.Web.Domain.Model;

namespace Ammeep.GiftRegister.Web.Models
{
    public class GiftStatusesPage
    {
        public IEnumerable<Gift> GiftsNotActive { get; private set; }
        public IEnumerable<PendingGift> GiftsPendingReservation { get; private set; }
        public IEnumerable<ReservedGift> GiftsConfirmedAsReserved { get; private set; }
   

        public GiftStatusesPage(IEnumerable<Gift> deactivatedGifts, IEnumerable<PendingGift> pendingGifts, IEnumerable<ReservedGift> reservedGifts)
        {
            GiftsNotActive = deactivatedGifts;
            GiftsPendingReservation = pendingGifts;
            GiftsConfirmedAsReserved = reservedGifts;
        }
    }
}