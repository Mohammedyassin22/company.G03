using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace company.G03.PL.Models
{
    public class CreateEmpDto
    {
        [Required(ErrorMessage = "Name is Required!!!")]
        public string Name { get; set; }
        [Range(18,60), Required(ErrorMessage = "Age is Required!!!")]
        public int? Age { get; set; }
        [DataType(DataType.EmailAddress,ErrorMessage = "Email is Required!!!")]
        public string Email { get; set; }
        [RegularExpression(@"[0-9]{1,3}-[a-z A-Z]{5,10}-[a-z A-Z]{4,10}-[a-z A-Z]{5,10}$",ErrorMessage ="Address Address like 123-street-cit-country")]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        [Phone(ErrorMessage = "Phone is Required!!!")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "IsActive is Required!!!")]
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "IsDelete is Required!!!")]
        public DateTime HiringDate { get; set; }
        [DisplayName("Create At is Required!!!")]
        public DateTime CreateAt { get; set; }
        public int? DepartmentID { get; set; }
    }
}
