using company.G03.BLL.Repository;
using company.G03.DAL.Models;
using company.G03.PL.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index()
        {
           var model= _deptRepository.GetAll();
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            if (ModelState.IsValid)
            {

            }
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
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(dto);
        }
    }
}
