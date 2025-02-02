using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockMangementAPI_Counsume.Models;
using System.Text;

namespace StockMangementAPI_Counsume.Controllers
{
    public class PaymentController : Controller
    {
		Uri baseAddress = new Uri("http://localhost:5165/api");
		private readonly HttpClient _client;
		public PaymentController()
		{
			_client = new HttpClient();
			_client.BaseAddress = baseAddress;
		}
		#region PaymentList
		[HttpGet]
		public IActionResult PaymentList()
		{
			List<PaymentsModel> payments = new List<PaymentsModel>();
			HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Payments/GetAll").Result;
			if (response.IsSuccessStatusCode)
			{
				string data = response.Content.ReadAsStringAsync().Result;
				dynamic jsonobject = JsonConvert.DeserializeObject(data);
				var extData = JsonConvert.SerializeObject(jsonobject);
				payments = JsonConvert.DeserializeObject<List<PaymentsModel>>(extData);

			}

			return View(payments);
		}
		#endregion
		#region PaymentForm
		public async Task<IActionResult> PaymentForm(int? PaymentID)
		{
			await LoadBillsList();
			if (PaymentID.HasValue)
			{
				var response = await _client.GetAsync($"{_client.BaseAddress}/Payments/GetbyID/{PaymentID}");
				if (response.IsSuccessStatusCode)
				{
					var data = await response.Content.ReadAsStringAsync();
					var payments = JsonConvert.DeserializeObject<PaymentsModel>(data);
					return View(payments);
				}
			}
			return View("PaymentForm", new PaymentsModel());
		}
		#endregion
		#region PaymentSave

		[HttpPost]
		public async Task<IActionResult> PaymentSave([FromForm] PaymentsModel payments)
		{
			try
			{
				if (ModelState.IsValid)
				{

					var json = JsonConvert.SerializeObject(payments);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpResponseMessage response;

					if (payments.PaymentID == null || payments.PaymentID == 0)
					{
						response = await _client.PostAsync($"{_client.BaseAddress}/Payments/Insert", content);
						if (response.IsSuccessStatusCode)
						{
							TempData["Message"] = "Record Inserted Successfully";
							return RedirectToAction("PaymentList");
						}
					}

					else
					{
						response = await _client.PutAsync($"{_client.BaseAddress}/Payments/Update/{payments.PaymentID}", content);
						if (response.IsSuccessStatusCode)
						{
							TempData["Message"] = "Record Updated Successfully";
							return RedirectToAction("PaymentList");
						}
					}
				}
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = ex.Message;
				Console.WriteLine(TempData["ErrorMessage"]);
			}
			await LoadBillsList();
			return RedirectToAction("PaymentList");
		}
		#endregion
		#region PaymentDelete
		public async Task<IActionResult> PaymentDelete(int PaymentID)
		{
			var response = await _client.DeleteAsync($"{_client.BaseAddress}/Payments/Delete/{PaymentID}");
			return RedirectToAction("PaymentList");
		}
		#endregion
		#region LoadBillsList
		private async Task LoadBillsList()
		{
			var response = await _client.GetAsync($"{_client.BaseAddress}/Payments/BillsDropDown");
			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadAsStringAsync();
				var bills = JsonConvert.DeserializeObject<List<BillsDropDownModel>>(data);
				ViewBag.billsList = bills;
			}
		}
		#endregion
	}
}
