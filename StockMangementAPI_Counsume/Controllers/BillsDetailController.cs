using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockMangementAPI_Counsume.Models;
using System.Text;

namespace StockMangementAPI_Counsume.Controllers
{
    public class BillsDetailController : Controller
    {
		Uri baseAddress = new Uri("http://localhost:5165/api");
		private readonly HttpClient _client;
		public BillsDetailController()
		{
			_client = new HttpClient();
			_client.BaseAddress = baseAddress;
		}
		#region BillsDetailList
		[HttpGet]
		public IActionResult BillsDetailList()
		{
			List<BillDetailsModel> billDetails = new List<BillDetailsModel>();
			HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/BillDetails/GetAll").Result;
			if (response.IsSuccessStatusCode)
			{
				string data = response.Content.ReadAsStringAsync().Result;
				dynamic jsonobject = JsonConvert.DeserializeObject(data);
				var extData = JsonConvert.SerializeObject(jsonobject);
				billDetails = JsonConvert.DeserializeObject<List<BillDetailsModel>>(extData);

			}

			return View(billDetails);
		}
		#endregion
		#region BillsDetailForm
		public async Task<IActionResult> BillsDetailForm(int? BillDetailID)
		{
			await LoadBillsList();
			await LoadProductList();
			if (BillDetailID.HasValue)
			{
				var response = await _client.GetAsync($"{_client.BaseAddress}/BillDetails/GetbyID/{BillDetailID}");
				if (response.IsSuccessStatusCode)
				{
					var data = await response.Content.ReadAsStringAsync();
					var billDetails = JsonConvert.DeserializeObject<BillDetailsModel>(data);
					return View(billDetails);
				}
			}
			return View("BillsDetailForm", new BillDetailsModel());
		}
		#endregion
		#region BillsDetailSave

		[HttpPost]
		public async Task<IActionResult> BillsDetailSave([FromForm] BillDetailsModel billDetails)
		{
			try
			{
				if (ModelState.IsValid)
				{

					var json = JsonConvert.SerializeObject(billDetails);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpResponseMessage response;

					if (billDetails.BillDetailID == null || billDetails.BillDetailID == 0)
					{
						response = await _client.PostAsync($"{_client.BaseAddress}/BillDetails/Insert", content);
						if (response.IsSuccessStatusCode)
						{
							TempData["Message"] = "Record Inserted Successfully";
							return RedirectToAction("BillsDetailList");
						}
					}

					else
					{
						response = await _client.PutAsync($"{_client.BaseAddress}/BillDetails/Update/{billDetails.BillDetailID}", content);
						if (response.IsSuccessStatusCode)
						{
							TempData["Message"] = "Record Updated Successfully";
							return RedirectToAction("BillsDetailList");
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
			await LoadProductList();
			return RedirectToAction("BillsDetailList");
		}
		#endregion
		#region BillsDetailDelete
		public async Task<IActionResult> BillsDetailDelete(int BillDetailID)
		{
			var response = await _client.DeleteAsync($"{_client.BaseAddress}/BillDetails/Delete/{BillDetailID}");
			return RedirectToAction("BillsDetailList");
		}
		#endregion
		#region LoadBillsList
		private async Task LoadBillsList()
		{
			var response = await _client.GetAsync($"{_client.BaseAddress}/BillDetails/BillDropDown");
			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadAsStringAsync();
				var bill = JsonConvert.DeserializeObject<List<BillsDropDownModel>>(data);
				ViewBag.billList = bill;
			}
		}
		#endregion
		#region LoadProductList
		private async Task LoadProductList()
		{
			var response = await _client.GetAsync($"{_client.BaseAddress}/BillDetails/ProductDropDown");
			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadAsStringAsync();
				var products = JsonConvert.DeserializeObject<List<ProductDropDownModel>>(data);
				ViewBag.productList = products;
			}
		}
		#endregion
	}
}
