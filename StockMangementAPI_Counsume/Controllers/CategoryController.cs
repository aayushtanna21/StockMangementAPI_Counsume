using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockMangementAPI_Counsume.Models;
using System.Text;

namespace StockMangementAPI_Counsume.Controllers
{
    public class CategoryController : Controller
    {
		Uri baseaddress = new Uri("http://localhost:5165/api");
		private readonly HttpClient _httpClient;
		public CategoryController()
		{
			_httpClient = new HttpClient();
			_httpClient.BaseAddress = baseaddress;
		}
		public IActionResult Index()
        {
            return View();
        }
		[HttpGet]
		public IActionResult CategoryList()
		{
			List<CategoryModel> category = new List<CategoryModel>();
			HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient}/Category/GetAll").Result;
			if (response.IsSuccessStatusCode)
			{
				string data = response.Content.ReadAsStringAsync().Result;
				dynamic jsonobj = JsonConvert.DeserializeObject<dynamic>(data);
				var extdata = JsonConvert.SerializeObject(jsonobj);
				category = JsonConvert.DeserializeObject<List<CategoryModel>>(extdata);
			}
			return View("CategoryList", category);
		}
		public async Task<IActionResult> CategoryForm(int? CategoryID)
		{
			if (CategoryID.HasValue)
			{
				List<CategoryModel> category = new List<CategoryModel>();
				HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/Category/GetByID/{CategoryID}").Result;
				if (response.IsSuccessStatusCode)
				{
					string data = response.Content.ReadAsStringAsync().Result;
					dynamic jsonobject = JsonConvert.DeserializeObject(data);
					var extData = JsonConvert.SerializeObject(jsonobject);
					category = JsonConvert.DeserializeObject<List<CategoryModel>>(extData);
					//ViewBag.userList = await GetStatesByCountryID(city.CountryID);
					CategoryModel categoryModel = new CategoryModel
					{
						CategoryID = category[0].CategoryID,
						CategoryName = category[0].CategoryName,
						Created=category[0].Created,
						Modified=category[0].Modified,
					};
					return View(categoryModel);
				}
				else
				{
					return RedirectToAction("CategoryList");
				}

			}
			return View("CategoryForm", new CategoryModel());
		}
		[HttpPost]
		public async Task<IActionResult> CategorySave([FromForm] CategoryModel category)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var json = JsonConvert.SerializeObject(category);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpResponseMessage response;

					if (category.CategoryID == null)
					{
						response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/Category/Insert", content);
						if (response.IsSuccessStatusCode)
						{
							TempData["Message"] = "Record Inserted Successfully";
							return RedirectToAction("CategoryList");
						}
					}

					else
					{
						response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}/Category/Update/{category.CategoryID}", content);
						if (response.IsSuccessStatusCode)
						{
							TempData["Message"] = "Record Updated Successfully";
							return RedirectToAction("CategoryList");
						}
					}
				}
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = ex.Message;
				Console.WriteLine(TempData["ErrorMessage"]);
			}
			return RedirectToAction("CategoryList");
		}
		public async Task<IActionResult> Delete(int CategoryID)
		{
			var response = await _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/Category/Delete/{CategoryID}");
			return RedirectToAction("Index");
		}
	}
}
