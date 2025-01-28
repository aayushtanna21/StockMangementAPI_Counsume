using Microsoft.AspNetCore.Mvc;

namespace StockMangementAPI_Counsume.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
