using System.ComponentModel.DataAnnotations;

namespace company.G03.PL.Models
{
    public class ForgetPassDto
    {

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
