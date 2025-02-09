using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockMangementAPI_Counsume.Models;
using System.Text;

namespace StockMangementAPI_Counsume.Controllers.Admin
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
            await LoadCustomersList();
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

        [HttpPost]
        public async Task<IActionResult> ProductSave(ProductModel product)
        {
            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(product.ProductName), "ProductName");
                content.Add(new StringContent(product.CategoryID.ToString()), "CategoryID");
                content.Add(new StringContent(product.StockQuantity.ToString()), "StockQuantity");
                content.Add(new StringContent(product.Price.ToString()), "Price");
                content.Add(new StringContent(product.CostPrice.ToString()), "CostPrice");
                content.Add(new StringContent(product.ReorderLevel.ToString()), "ReorderLevel");
                content.Add(new StringContent(product.Unit), "Unit");
                content.Add(new StringContent(product.CustomerID.ToString()), "CustomerID");

                if (product.ImageFile != null)
                {
                    var fileStream = product.ImageFile.OpenReadStream();
                    content.Add(new StreamContent(fileStream), "ImageFile", product.ImageFile.FileName);
                }

                HttpResponseMessage response = await _client.PostAsync($"{_client.BaseAddress}/Product/Insert", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Product added successfully!";
                    return RedirectToAction("ProductList");
                }
            }

            TempData["ErrorMessage"] = "Error inserting product.";
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
        #region LoadCustomersList
        private async Task LoadCustomersList()
        {
            var response = await _client.GetAsync($"{_client.BaseAddress}/Product/CustomerDropDown");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var customers = JsonConvert.DeserializeObject<List<CustomersDropDownModel>>(data);
                ViewBag.customersList = customers;
            }
        }
        #endregion
    }
}
