using Microsoft.AspNetCore.Mvc;

namespace StockMangementAPI_Counsume.Controllers
{
    public class SuppliersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
