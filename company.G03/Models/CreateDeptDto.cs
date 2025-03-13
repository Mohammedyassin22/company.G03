using System.ComponentModel.DataAnnotations;

namespace company.G03.PL.Models
{
    public class CreateDeptDto
    {
        [Required (ErrorMessage ="Name is Required!!!")]
        public string Name { get; set; }
        [Required (ErrorMessage ="Code is Required!!!")]
        public String Code { get; set; }
        [Required(ErrorMessage ="Create at is Required!!!")]
        public DateTime CreateAt { get; set; }
    }
}
