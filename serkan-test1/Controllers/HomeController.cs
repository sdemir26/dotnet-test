using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using serkan_test1.Models;
using System.Diagnostics;

namespace serkan_test1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper mapper;

        public HomeController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        

        public IActionResult Index()
        {
            var model = new MusteriDto() { Adi = "Serkan" };
            return View(model);
        }

        [HttpPost]
        public IActionResult PostIndex([FromForm] MusteriDto musteri)
        {
           var Veritabani= mapper.Map<Customer>(musteri);
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
