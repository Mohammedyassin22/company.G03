using System.ComponentModel.DataAnnotations;

namespace company.G03.PL.Models
{
    public class ResetPassDto
    {
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirm password does not match the password")]
        public string ConfirmPassword { get; set; }
    }
}
