﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace company.G03.DAL.Models
{
    public class Employee:BaseEntity
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public decimal Salary { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public DateTime HiringDate { get; set; }
        public DateTime CreateAt { get; set; }
        [Display(Name = "Department")]
        public int? DepartmentID { get; set; }
        public Department? Dept { get; set; }
        public string? Image {  get; set; }
    }
}
