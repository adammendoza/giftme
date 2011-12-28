using System.ComponentModel.DataAnnotations;

namespace Ammeep.GiftRegister.Web.Models
{
    public class AdminLoginModel 
    {
        [Required(ErrorMessage = "Requried")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Requried")]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}