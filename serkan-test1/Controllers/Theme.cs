using Microsoft.AspNetCore.Mvc;

namespace serkan_test1.Controllers;

public class Theme : Controller
{
    //GET
    public IActionResult Index()
    {
        return View();
    }
    
    // GET
    public IActionResult Test()
    {
        return View();
    }
    
   
}