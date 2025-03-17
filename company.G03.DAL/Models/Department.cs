using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace company.G03.DAL.Models
{
    public class Department:BaseEntity
    {
        public string Name { get; set; }
        public String Code { get; set; }
        public DateTime CreateAt { get; set; }
        public List<Employee> Emp { get; set; }
    }
}
