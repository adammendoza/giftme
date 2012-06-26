using System;

namespace Ammeep.GiftRegister.Web.Domain.Model
{
    public class Feedback
    {
        public string Name { get; set; }

        public string InterpretationComments { get; set; }

        public bool WasItClear { get; set; }

        public string NotClearComments { get; set; }

        public bool WantsToSeeReservedGifts { get; set; }

        public string SeeReservedGiftsComments { get; set; }

        public bool AgreesWithPricingModels { get; set; }

        public string PricingModelComments { get; set; }

        public string FinalComments { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public int LastUpdatedBy { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreatedBy { get; set; }
    }
}