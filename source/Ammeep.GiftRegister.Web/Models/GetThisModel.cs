using System.ComponentModel.DataAnnotations;

namespace Ammeep.GiftRegister.Web.Models
{
    public class GetThisModel
    {
        [Required]
        public int Quantity { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public int GiftId { get; set; }
    }
}