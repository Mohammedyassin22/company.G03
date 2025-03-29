using System.ComponentModel.DataAnnotations;

namespace company.G03.PL.Models
{
	public class SignUpDto
	{
		[Required(ErrorMessage ="User name is required")]
        public string UserName { get; set; }
		[Required(ErrorMessage = "FirstName is required")]
		public string FirstName { get; set; }
		[Required(ErrorMessage = "LastName is required")]
		public string LastName { get; set; }
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress]
		public string Email { get; set; }
		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password is required")]
		[Compare(nameof(Password),ErrorMessage ="Confirm password does not match the password")]
		public string ConfirmPassword { get; set; }
      //  [Range(typeof(bool), "true", "true", ErrorMessage = "Terms and Conditions must be agreed to.")]
        public bool IsAgree { get; set; }
	}
}
