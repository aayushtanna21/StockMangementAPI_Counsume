using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockMangementAPI_Counsume.Models;
using System.Text;

namespace StockMangementAPI_Counsume.Controllers.Admin
{
    public class CategoryController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5165/api");
        private readonly HttpClient _client;

        public CategoryController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }
        #region CategoryList
        [HttpGet]
        public IActionResult CategoryList()
        {
            List<CategoryModel> category = new List<CategoryModel>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Category/GetAll").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonobject = JsonConvert.DeserializeObject(data);
                var extData = JsonConvert.SerializeObject(jsonobject);
                category = JsonConvert.DeserializeObject<List<CategoryModel>>(extData);

            }

            return View(category);
        }
        #endregion
        #region CategoryForm
        public async Task<IActionResult> CategoryForm(int? CategoryID)
        {
            //await LoadUserList();
            if (CategoryID.HasValue)
            {
                var response = await _client.GetAsync($"{_client.BaseAddress}/Category/GetbyID/{CategoryID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var category = JsonConvert.DeserializeObject<CategoryModel>(data);
                    //ViewBag.userList = await GetStatesByCountryID(city.CountryID);
                    return View(category);
                }
            }
            return View("CategoryForm", new CategoryModel());
        }
        #endregion
        #region CategorySave

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

                    if (category.CategoryID == null || category.CategoryID == 0)
                    {
                        response = await _client.PostAsync($"{_client.BaseAddress}/Category/Insert", content);
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["Message"] = "Record Inserted Successfully";
                            return RedirectToAction("CategoryList");
                        }
                    }

                    else
                    {
                        response = await _client.PutAsync($"{_client.BaseAddress}/Category/Update/{category.CategoryID}", content);
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
            //await LoadUserList();
            return RedirectToAction("CategoryList");
        }
        #endregion
        #region CategoryDelete
        public async Task<IActionResult> CategoryDelete(int CategoryID)
        {
            var response = await _client.DeleteAsync($"{_client.BaseAddress}/Category/Delete/{CategoryID}");
            return RedirectToAction("CategoryList");
        }
        #endregion
    }
}
