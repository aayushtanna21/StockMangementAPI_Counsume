using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockMangementAPI_Counsume.Models;
using System.Text;

namespace StockMangementAPI_Counsume.Controllers
{
    public class ProductController : Controller
    {
		Uri baseAddress = new Uri("http://localhost:5165/api");
		private readonly HttpClient _client;
		public ProductController()
		{
			_client = new HttpClient();
			_client.BaseAddress = baseAddress;
		}
		#region ProductList
		[HttpGet]
		public IActionResult ProductList()
		{
			List<ProductModel> product = new List<ProductModel>();
			HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Product/GetAll").Result;
			if (response.IsSuccessStatusCode)
			{
				string data = response.Content.ReadAsStringAsync().Result;
				dynamic jsonobject = JsonConvert.DeserializeObject(data);
				var extData = JsonConvert.SerializeObject(jsonobject);
				product = JsonConvert.DeserializeObject<List<ProductModel>>(extData);

			}

			return View(product);
		}
		#endregion
		#region ProductForm
		public async Task<IActionResult> ProductForm(int? ProductID)
		{
			await LoadCategoryList();
			await LoadUserList();
			if (ProductID.HasValue)
			{
				var response = await _client.GetAsync($"{_client.BaseAddress}/Product/GetbyID/{ProductID}");
				if (response.IsSuccessStatusCode)
				{
					var data = await response.Content.ReadAsStringAsync();
					var product = JsonConvert.DeserializeObject<ProductModel>(data);
					return View(product);
				}
			}
			return View("ProductForm", new ProductModel());
		}
		#endregion
		#region ProductSave

		[HttpPost]
		public async Task<IActionResult> ProductSave([FromForm] ProductModel product)
		{
			try
			{
				if (ModelState.IsValid)
				{

					var json = JsonConvert.SerializeObject(product);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpResponseMessage response;

					if (product.ProductID == null || product.ProductID == 0)
					{
						response = await _client.PostAsync($"{_client.BaseAddress}/Product/Insert", content);
						if (response.IsSuccessStatusCode)
						{
							TempData["Message"] = "Record Inserted Successfully";
							return RedirectToAction("ProductList");
						}
					}

					else
					{
						response = await _client.PutAsync($"{_client.BaseAddress}/Product/Update/{product.ProductID}", content);
						if (response.IsSuccessStatusCode)
						{
							TempData["Message"] = "Record Updated Successfully";
							return RedirectToAction("ProductList");
						}
					}
				}
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = ex.Message;
				Console.WriteLine(TempData["ErrorMessage"]);
			}
			await LoadCategoryList();
			await LoadUserList();
			return RedirectToAction("ProductList");
		}
		#endregion
		#region ProductDelete
		public async Task<IActionResult> ProductDelete(int ProductID)
		{
			var response = await _client.DeleteAsync($"{_client.BaseAddress}/Product/Delete/{ProductID}");
			return RedirectToAction("ProductList");
		}
		#endregion
		#region LoadCategoryList
		private async Task LoadCategoryList()
		{
			var response = await _client.GetAsync($"{_client.BaseAddress}/Product/CategoryDropDown");
			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadAsStringAsync();
				var category = JsonConvert.DeserializeObject<List<CategoryDropDownModel>>(data);
				ViewBag.categoryList = category;
			}
		}
		#endregion
		#region LoadUserList
		private async Task LoadUserList()
		{
			var response = await _client.GetAsync($"{_client.BaseAddress}/Product/UserDropDown");
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
