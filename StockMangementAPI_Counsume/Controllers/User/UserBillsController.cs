using Microsoft.AspNetCore.Mvc;

namespace StockMangementAPI_Counsume.Controllers.User
{
    public class UserBillsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
