﻿using Microsoft.AspNetCore.Mvc;

namespace StockMangementAPI_Counsume.Controllers
{
	public class MainHomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult RedirectToAdmin()
		{
			return RedirectToAction("Index", "Home");
		}

		public IActionResult RedirectToUser()
		{
			return RedirectToAction("Index", "UserHome");
		}

		public IActionResult MainHomeDisplay()
		{
			return View();
		}
	}
}
