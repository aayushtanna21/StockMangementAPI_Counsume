using Microsoft.AspNetCore.Mvc;

namespace StockMangementAPI_Counsume.Controllers
{
    public class CustomersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
