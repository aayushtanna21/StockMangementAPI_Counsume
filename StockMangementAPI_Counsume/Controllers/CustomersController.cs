using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockMangementAPI_Counsume.Models;
using System.Text;

namespace StockMangementAPI_Counsume.Controllers
{
    public class CustomersController : Controller
    {
		Uri baseAddress = new Uri("http://localhost:5165/api");
		private readonly HttpClient _client;
		public CustomersController()
		{
			_client = new HttpClient();
			_client.BaseAddress = baseAddress;
		}
		#region CustomerList
		[HttpGet]
		public IActionResult CustomerList()
		{
			List<CustomersModel> customers = new List<CustomersModel>();
			HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Customers/GetAll").Result;
			if (response.IsSuccessStatusCode)
			{
				string data = response.Content.ReadAsStringAsync().Result;
				dynamic jsonobject = JsonConvert.DeserializeObject(data);
				var extData = JsonConvert.SerializeObject(jsonobject);
				customers = JsonConvert.DeserializeObject<List<CustomersModel>>(extData);

			}

			return View(customers);
		}
		#endregion
		#region CustomerForm
		public async Task<IActionResult> CustomerForm(int? CustomerID)
		{
			await LoadUserList();
			if (CustomerID.HasValue)
			{
				var response = await _client.GetAsync($"{_client.BaseAddress}/Customers/GetbyID/{CustomerID}");
				if (response.IsSuccessStatusCode)
				{
					var data = await response.Content.ReadAsStringAsync();
					var customers = JsonConvert.DeserializeObject<CustomersModel>(data);
					return View(customers);
				}
			}
			return View("CustomerForm", new CustomersModel());
		}
		#endregion
		#region CustomerSave

		[HttpPost]
		public async Task<IActionResult> CustomerSave([FromForm] CustomersModel customers)
		{
			try
			{
				if (ModelState.IsValid)
				{

					var json = JsonConvert.SerializeObject(customers);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpResponseMessage response;

					if (customers.CustomerID == null || customers.CustomerID == 0)
					{
						response = await _client.PostAsync($"{_client.BaseAddress}/Customers/Insert", content);
						if (response.IsSuccessStatusCode)
						{
							TempData["Message"] = "Record Inserted Successfully";
							return RedirectToAction("CustomerList");
						}
					}

					else
					{
						response = await _client.PutAsync($"{_client.BaseAddress}/Customers/Update/{customers.CustomerID}", content);
						if (response.IsSuccessStatusCode)
						{
							TempData["Message"] = "Record Updated Successfully";
							return RedirectToAction("CustomerList");
						}
					}
				}
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = ex.Message;
				Console.WriteLine(TempData["ErrorMessage"]);
			}
			await LoadUserList();
			return RedirectToAction("CustomerList");
		}
		#endregion
		#region CustomerDelete
		public async Task<IActionResult> CustomerDelete(int CustomerID)
		{
			var response = await _client.DeleteAsync($"{_client.BaseAddress}/Customers/Delete/{CustomerID}");
			return RedirectToAction("CustomerList");
		}
		#endregion
		#region LoadUserList
		private async Task LoadUserList()
		{
			var response = await _client.GetAsync($"{_client.BaseAddress}/Customers/UserDropDown");
			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadAsStringAsync();
				var user = JsonConvert.DeserializeObject<List<UserDropDownModel>>(data);
				ViewBag.userList = user;
			}
		}
		#endregion
	}
}
