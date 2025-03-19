using AutoMapper;
using company.G03.BLL;
using company.G03.BLL.Interface;
using company.G03.BLL.Repository;
using company.G03.DAL.Models;
using company.G03.PL.Helper;
using company.G03.PL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace company.G03.PL.Controllers
{
    public class DeptController : Controller
    {
        //private readonly DeptRepository _deptRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DeptController(DeptRepository deptRepository,IMapper mapper,IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? searchInput)
        {
            IEnumerable<Department> dept;

            if (string.IsNullOrEmpty(searchInput))
            {
                dept =await _unitOfWork.DeptRepository.GetAllasync();
            }
            else
            {
                dept =await _unitOfWork.DeptRepository.GetNameasync(searchInput);
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
        public async Task<IActionResult> Create(CreateDeptDto dto)
        {
            if (ModelState.IsValid)
            {
                //var dept = new Department()
                //{
                //    Name = dto.Name,
                //    Code = dto.Code,
                //    CreateAt = dto.CreateAt
                //};
                var dept=_mapper.Map<Department>(dto);
                await _unitOfWork.DeptRepository.Addasync(dept);
                var count =await _unitOfWork.Completeasync();
                if (count > 0)
                {
                    TempData["SuccessMessage"] = "Section Added successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(dto);
        }
       
        [HttpGet]
        public async Task<IActionResult> Details(int? id,string ViewName)
        {
           if(id is null)
            {
                return BadRequest("is valid");
            }
            var dept =await _unitOfWork.DeptRepository.Getasync(id.Value);
            if(dept == null)
            {
                return NotFound(new{Message="Department with id is not found"});
            }
            return View(ViewName, dept);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateDeptDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
             var dept =await _unitOfWork.DeptRepository.Getasync(id);
            if (dept == null)
            {
                return NotFound();
            }

            // تحديث بيانات الموظف باستخدام AutoMapper
            _mapper.Map(model, dept);


            _unitOfWork.DeptRepository.Update(dept);
            var count =await _unitOfWork.Completeasync();

            if (count > 0)
            {
                TempData["SuccessMessage"] = "Employee updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id,"Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, Department department)
        {
            if (ModelState.IsValid)
            {
                if (id != department.Id)
                    return BadRequest();
                 _unitOfWork.DeptRepository.Delete(department);
                var dept =await _unitOfWork.Completeasync();
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
