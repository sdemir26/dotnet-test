using AutoMapper;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using serkan_test1.Models;
using serkan_test1.Data;
using System.Diagnostics;

namespace serkan_test1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly UygulamaDbContext applicationDbContext;

        public HomeController(IMapper mapper,
            UygulamaDbContext applicationDbContext,
            ILogger<HomeController> logger)
        {
            this._mapper = mapper;
            this.applicationDbContext = applicationDbContext;
            _logger = logger;
        }



        public IActionResult Index()
        {
            var model = new MusteriDto() { Adi = "Serkan" };
            return View(model);
        }
        //fromform, frombody 
        //modelbinding, form json
        //attribute 
        //routing


        [HttpPost]
        public async Task<IActionResult> PostIndex([FromForm] MusteriDto musteri)
        {
            if (musteri == null)
            {
                return BadRequest("Müşteri bilgileri boş olamaz");
            }
            
            var YeniMusteri = _mapper.Map<Customer>(musteri);
            await applicationDbContext.Customers.AddAsync(YeniMusteri);
            await applicationDbContext.SaveChangesAsync();
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


    [Route("api")]
    [ApiController]
    public class HomeApi : ControllerBase
    {
        private readonly UygulamaDbContext applicationDbContext;
        private readonly IMapper _mapper;

        public HomeApi(UygulamaDbContext applicationDbContext, IMapper mapper)
        {
            this.applicationDbContext = applicationDbContext;
            this._mapper = mapper;
        }

        [HttpPost, Route("postindex")]
        public async Task<IActionResult> PostIndex([FromBody] MusteriDto musteri)
        {
            if (musteri == null)
            {
                return BadRequest("Müşteri bilgileri boş olamaz");
            }
            
            var YeniMusteri = _mapper.Map<Customer>(musteri);
            await applicationDbContext.Customers.AddAsync(YeniMusteri);
            await applicationDbContext.SaveChangesAsync();
            return new JsonResult(YeniMusteri);
        }
    }

}
