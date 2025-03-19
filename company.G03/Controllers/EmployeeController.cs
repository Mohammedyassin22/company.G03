using AutoMapper;
using company.G03.BLL;
using company.G03.BLL.Interface;
using company.G03.BLL.Repository;
using company.G03.DAL.Models;
using company.G03.PL.Helper;
using company.G03.PL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
                // رفع الصورة إن وجدت وتخزين اسمها في `dto.ImageName`
                if (dto.Iamge is not null)
                {
                    dto.ImageName = DocumentSetting.UploadFile(dto.Iamge, "Images");
                }

                // تحويل `dto` إلى `Employee`
                var emp = _mapper.Map<Employee>(dto);

                // حفظ اسم الصورة في `emp.Image`
                emp.Image = dto.ImageName;

                // إضافة الموظف إلى قاعدة البيانات
                _UnitOfWork.EmpRepository.Add(emp);
                var count = _UnitOfWork.Complete();

                if (count > 0)
                {
                    TempData["SuccessMessage"] = "Employee added successfully!";
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

        public IActionResult Edit(int id)
        {
            var employee = _UnitOfWork.EmpRepository.Get(id);
            if (employee == null)
            {
                return NotFound();
            }

            // تحويل Employee إلى CreateEmpDto باستخدام AutoMapper
            var model = _mapper.Map<CreateEmpDto>(employee);

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, CreateEmpDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var employee = _UnitOfWork.EmpRepository.Get(id);
            if (employee == null)
            {
                return NotFound();
            }

            // تحديث بيانات الموظف باستخدام AutoMapper
            _mapper.Map(model, employee);

            if (model.Iamge is not null)
            {
                if (!string.IsNullOrEmpty(employee.Image))
                {
                    DocumentSetting.Delete(employee.Image, "Images");
                }
                employee.Image = DocumentSetting.UploadFile(model.Iamge, "Images");
            }

            _UnitOfWork.EmpRepository.Update(employee);
            var count = _UnitOfWork.Complete();

            if (count > 0)
            {
                TempData["SuccessMessage"] = "Employee updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id,CreateEmpDto dto)
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
                if(dto.ImageName is not null)
                {
                    DocumentSetting.Delete(dto.ImageName, "Images");
                }
                TempData["SuccessMessage"] = "Employee deleted successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Failed to delete employee.";
            return View(employee);
        }
    }
}
