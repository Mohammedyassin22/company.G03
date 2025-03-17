using company.G03.BLL.Interface;
using company.G03.BLL.Repository;
using company.G03.DAL.Models;
using company.G03.PL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace company.G03.PL.Controllers
{
    public class DeptController : Controller
    {
        private readonly DeptRepository _deptRepository;
        public DeptController(DeptRepository deptRepository)
        {
            _deptRepository = deptRepository;
        }
        [HttpGet]
        public IActionResult Index(string? searchInput)
        {
            IEnumerable<Department> dept;

            if (string.IsNullOrEmpty(searchInput))
            {
                dept = _deptRepository.GetAll();
            }
            else
            {
                dept = _deptRepository.GetName(searchInput);
            }

            ViewData["Message"] = $"The number of employees is {dept.Count()}";

            return View(dept);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateDeptDto dto)
        {
            if (ModelState.IsValid)
            {
                var dept = new Department()
                {
                    Name = dto.Name,
                    Code = dto.Code,
                    CreateAt = dto.CreateAt
                };
                var count=_deptRepository.Add(dept);
                if(count > 0)
                {
                    TempData["SuccessMessage"] = "Section Added successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(dto);
        }
       
        [HttpGet]
        public IActionResult Details(int? id,string ViewName)
        {
           if(id is null)
            {
                return BadRequest("is valid");
            }
            var dept = _deptRepository.Get(id.Value);
            if(dept == null)
            {
                return NotFound(new{Message="Department with id is not found"});
            }
            return View(ViewName, dept);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int id,Department department)
        {
            if(ModelState.IsValid)
            {
                if (id != department.Id)
                    return BadRequest();
                var dept=_deptRepository.Update(department);
                if (dept > 0)
                {
                    TempData["SuccessMessage"] = "Section modified successfully!";
                    return RedirectToAction(nameof(Index));
                }
               
            }
            return View(department);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id,"Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Department department)
        {
            if (ModelState.IsValid)
            {
                if (id != department.Id)
                    return BadRequest();
                var dept = _deptRepository.Delete(department);
                if (dept > 0)
                {
                    TempData["SuccessMessage"] = "Section Deleted successfully!";
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(department);
        }
    }
}
