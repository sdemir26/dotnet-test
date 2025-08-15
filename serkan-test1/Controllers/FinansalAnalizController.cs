using Microsoft.AspNetCore.Mvc;

namespace serkan_test1.Controllers
{
    /// <summary>
    /// Finansal analiz sayfaları için controller
    /// </summary>
    public class FinansalAnalizController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Finansal Analiz 1 sayfasını gösterir
        /// </summary>
        /// <returns>Finansal analiz 1 view'ı</returns>
        public IActionResult Analiz1()
        {
            return View();
        }

        /// <summary>
        /// Finansal Analiz 2 sayfasını gösterir
        /// </summary>
        /// <returns>Finansal analiz 2 view'ı</returns>
        public IActionResult Analiz2()
        {
            return View();
        }

        public IActionResult Analiz3()
        {
            return View();
        }

        public IActionResult Analiz4()
        {
            return View();
        }
    }
}
