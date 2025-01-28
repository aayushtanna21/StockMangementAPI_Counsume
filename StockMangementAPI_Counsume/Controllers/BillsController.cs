using Microsoft.AspNetCore.Mvc;

namespace StockMangementAPI_Counsume.Controllers
{
    public class BillsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
