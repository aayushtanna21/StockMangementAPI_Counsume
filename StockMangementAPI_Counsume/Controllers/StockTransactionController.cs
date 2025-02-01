using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockMangementAPI_Counsume.Models;
using System.Text;

namespace StockMangementAPI_Counsume.Controllers
{
    public class StockTransactionController : Controller
    {
		Uri baseAddress = new Uri("http://localhost:5165/api");
		private readonly HttpClient _client;
		public StockTransactionController()
		{
			_client = new HttpClient();
			_client.BaseAddress = baseAddress;
		}
		#region StockTransactionList
		[HttpGet]
		public IActionResult StockTransactionList()
		{
			List<StockTransactionsModel> stocktransaction = new List<StockTransactionsModel>();
			HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/StockTransactions/GetAll").Result;
			if (response.IsSuccessStatusCode)
			{
				string data = response.Content.ReadAsStringAsync().Result;
				dynamic jsonobject = JsonConvert.DeserializeObject(data);
				var extData = JsonConvert.SerializeObject(jsonobject);
				stocktransaction = JsonConvert.DeserializeObject<List<StockTransactionsModel>>(extData);

			}

			return View(stocktransaction);
		}
		#endregion
		#region StockTransactionForm
		public async Task<IActionResult> StockTransactionForm(int? StockTransactionID)
		{
			await LoadProductList();
			await LoadUserList();
			if (StockTransactionID.HasValue)
			{
				var response = await _client.GetAsync($"{_client.BaseAddress}/StockTransactions/GetbyID/{StockTransactionID}");
				if (response.IsSuccessStatusCode)
				{
					var data = await response.Content.ReadAsStringAsync();
					var stocktransaction = JsonConvert.DeserializeObject<StockTransactionsModel>(data);
					return View(stocktransaction);
				}
			}
			return View("StockTransactionForm", new StockTransactionsModel());
		}
		#endregion
		#region StockTransactionSave

		[HttpPost]
		public async Task<IActionResult> StockTransactionSave([FromForm] StockTransactionsModel stocktransaction)
		{
			try
			{
				if (ModelState.IsValid)
				{

					var json = JsonConvert.SerializeObject(stocktransaction);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpResponseMessage response;

					if (stocktransaction.StockTransactionID == null || stocktransaction.StockTransactionID == 0)
					{
						response = await _client.PostAsync($"{_client.BaseAddress}/StockTransactions/Insert", content);
						if (response.IsSuccessStatusCode)
						{
							TempData["Message"] = "Record Inserted Successfully";
							return RedirectToAction("StockTransactionList");
						}
					}

					else
					{
						response = await _client.PutAsync($"{_client.BaseAddress}/StockTransaction/Update/{stocktransaction.StockTransactionID}", content);
						if (response.IsSuccessStatusCode)
						{
							TempData["Message"] = "Record Updated Successfully";
							return RedirectToAction("StockTransactionList");
						}
					}
				}
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = ex.Message;
				Console.WriteLine(TempData["ErrorMessage"]);
			}
			await LoadProductList();
			await LoadUserList();
			return RedirectToAction("StockTransactionList");
		}
		#endregion
		#region StockTransactionDelete
		public async Task<IActionResult> StockTransactionDelete(int StockTransactionID)
		{
			var response = await _client.DeleteAsync($"{_client.BaseAddress}/StockTransactions/Delete/{StockTransactionID}");
			return RedirectToAction("StockTransactionList");
		}
		#endregion
		#region LoadProductList
		private async Task LoadProductList()
		{
			var response = await _client.GetAsync($"{_client.BaseAddress}/StockTransactions/ProductDropDown");
			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadAsStringAsync();
				var product = JsonConvert.DeserializeObject<List<ProductDropDownModel>>(data);
				ViewBag.productList = product;
			}
		}
		#endregion
		#region LoadUserList
		private async Task LoadUserList()
		{
			var response = await _client.GetAsync($"{_client.BaseAddress}/StockTransactions/UserDropDown");
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
