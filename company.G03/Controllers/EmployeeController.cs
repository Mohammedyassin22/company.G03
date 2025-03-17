using company.G03.BLL.Interface;
using company.G03.BLL.Repository;
using company.G03.DAL.Models;
using company.G03.PL.Models;
using Microsoft.AspNetCore.Mvc;

namespace company.G03.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmpRepository _empRepository;
        private readonly IDeptRepository _deptRepository;
        public EmployeeController(IEmpRepository empRepository,IDeptRepository deptRepository) 
        {
            _empRepository = empRepository;
            _deptRepository = deptRepository;
        }

        [HttpGet]
        public IActionResult Index(string? searchInput)
        {
            IEnumerable<Employee> emp;

            if (string.IsNullOrEmpty(searchInput))
            {
                emp = _empRepository.GetAll();
            }
            else
            {
                emp = _empRepository.GetName(searchInput);
            }

            ViewData["Message"] = $"The number of employees is {emp.Count()}";

            return View(emp);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var dept = _deptRepository.GetAll();
            ViewData["Department"]=dept;
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmpDto dto)
        {
            if (ModelState.IsValid)
            {
                var emp = new Employee()
                {
                    Name = dto.Name,
                    Phone=dto.Phone,
                    Address=dto.Address,
                    Age=dto.Age,
                    HiringDate=dto.HiringDate,
                    CreateAt=dto.CreateAt,
                    Email=dto.Email,
                    IsActive=dto.IsActive,
                    Salary=dto.Salary,
                    DepartmentID=dto.DepartmentID,
                };
                var count = _empRepository.Add(emp);
                if (count > 0)
                {
                    TempData["SuccessMessage"] = "Section Added successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(dto);
        }

        [HttpGet]
        public IActionResult Details(int? id, string ViewName)
        {
            if (id is null)
            {
                return BadRequest("is valid");
            }
            var emp = _empRepository.Get(id.Value);
            if (emp == null)
            {
                return NotFound(new { Message = "Employee with id is not found" });
            }
            return View(ViewName, emp);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {

            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (id != employee.Id)
                    return BadRequest();
                var emp = _empRepository.Update(employee);
                if (emp > 0)
                {
                    TempData["SuccessMessage"] = "Section modified successfully!";
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(employee);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id)
        {
            var employee = _empRepository.Get(id);
            if (employee == null)
            {
                return NotFound();
            }

            var result = _empRepository.Delete(employee);
            if (result > 0)
            {
                TempData["SuccessMessage"] = "Employee deleted successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Failed to delete employee.";
            return View(employee);
        }
    }
}
