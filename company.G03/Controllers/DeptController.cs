using company.G03.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace company.G03.Controllers
{
    public class DeptController : Controller
    {
        private readonly ILogger<DeptController> _logger;

        public DeptController(ILogger<DeptController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
