using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ammeep.GiftRegister.Web.Domain.Model
{
    public class UserFeedback
    {
        [Required(ErrorMessage = "Who are you?")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please let us know what you thought")]
        [DisplayName("What was your intepretation of what you were asked to do on this online registry?")]
        [StringLength(4000)]
        public string InterpretationComments { get; set; }

        [Required]
        [DisplayName("Was it clear?")]
        public bool WasItClear { get; set; }

        [DisplayName("If not, why not")]
        [StringLength(4000)]
        public string NotClearComments { get; set; }

        [DisplayName("Do you think it would have been helpful to have seen the items already reserved by other people? (e.g shaded out)")]
        public bool WantsToSeeReservedGifts { get; set; }

        [DisplayName("Comments")]
        [StringLength(4000)]
        public string SeeReservedGiftsComments { get; set; }

        [DisplayName("If you were to use this for you own online wedding registry, would you consider either of the pricing models bellow?")]
        public bool AgreesWithPricingModels { get; set; }

        [DisplayName("Comments")]
        [StringLength(4000)]
        public string PricingModelComments { get; set; }

        [DisplayName("Do you have anything else you would like to let us know about your online registry experience?")]
        [StringLength(4000)]
        public string FinalComments { get; set; }
    }
}