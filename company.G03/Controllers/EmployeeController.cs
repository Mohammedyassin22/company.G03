using AutoMapper;
using company.G03.BLL;
using company.G03.BLL.Interface;
using company.G03.BLL.Repository;
using company.G03.DAL.Models;
using company.G03.PL.Models;
using Microsoft.AspNetCore.Mvc;

namespace company.G03.PL.Controllers
{
    public class EmployeeController : Controller
    {
        //private readonly IEmpRepository _empRepository;
        //private readonly IDeptRepository _deptRepository;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _mapper;
        public EmployeeController(IEmpRepository empRepository,IDeptRepository deptRepository, IMapper mapper,IUnitOfWork unitOfWork) 
        {
            //_empRepository = empRepository;
            //_deptRepository = deptRepository;
            _UnitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(string? searchInput)
        {
            IEnumerable<Employee> emp;

            if (string.IsNullOrEmpty(searchInput))
            {
                emp = _UnitOfWork.EmpRepository.GetAll();
            }
            else
            {
                emp = _UnitOfWork.EmpRepository.GetName(searchInput);
            }

            ViewData["Message"] = $"The number of employees is {emp.Count()}";

            return View(emp);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var dept = _UnitOfWork.DeptRepository.GetAll();
            ViewData["Department"]=dept;
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmpDto dto)
        {
            if (ModelState.IsValid)
            {
                //var emp = new Employee()
                //{
                //    Name = dto.Name,
                //    Phone=dto.Phone,
                //    Address=dto.Address,
                //    Age=dto.Age,
                //    HiringDate=dto.HiringDate,
                //    CreateAt=dto.CreateAt,
                //    Email=dto.Email,
                //    IsActive=dto.IsActive,
                //    Salary=dto.Salary,
                //    DepartmentID=dto.DepartmentID,
                //};
                var emp=_mapper.Map<Employee>(dto);
               _UnitOfWork.EmpRepository.Add(emp);
                var count = _UnitOfWork.Complete();
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
            var emp = _UnitOfWork.EmpRepository.Get(id.Value);
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
                 _UnitOfWork.EmpRepository.Update(employee);
                var emp = _UnitOfWork.Complete();
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
            var employee = _UnitOfWork.EmpRepository.Get(id);
            if (employee == null)
            {
                return NotFound();
            }

           _UnitOfWork.EmpRepository.Delete(employee);
            var result = _UnitOfWork.Complete();
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
