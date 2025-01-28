using Microsoft.AspNetCore.Mvc;

namespace StockMangementAPI_Counsume.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
