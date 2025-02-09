using Microsoft.AspNetCore.Mvc;

namespace StockMangementAPI_Counsume.Controllers.User
{
    public class UserCustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
