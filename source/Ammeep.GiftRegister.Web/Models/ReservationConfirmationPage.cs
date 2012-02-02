using System;
using System.Collections.Generic;
using System.Linq;
using Ammeep.GiftRegister.Web.Domain.Model;

namespace Ammeep.GiftRegister.Web.Models
{
    public class ReservationConfirmationPage
    {
        public Gift ReservedGift { get; set; }
        public Guest Guest { get; set; }
        public GiftPruchase GiftPruchase { get; set; }
        public bool Confirmed { get; set; }
        public IList<string> ConfirmationErrors { get; set; }

        public GiftRow GiftRow
        {
            get { return new GiftRow {IsFirst = true, Item = ReservedGift}; }
        }

        public bool IsConfirmed
        {
            get
            {
                throw new Exception();
                return (ConfirmationErrors != null && ConfirmationErrors.Count == 0) ||
                       (GiftPruchase != null && GiftPruchase.Confirmed);
            }
        }
    }
}