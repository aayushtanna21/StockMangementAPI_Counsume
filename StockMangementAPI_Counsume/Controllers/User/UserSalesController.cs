using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockMangementAPI_Counsume.Models;
using System.Text;

namespace StockMangementAPI_Counsume.Controllers.User
{
    public class UserSalesController : Controller
    {

        Uri baseAddress = new Uri("http://localhost:5165/api");
        private readonly HttpClient _client;
        public UserSalesController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }
        #region SalesList
        [HttpGet]
        public IActionResult SalesList()
        {
            List<UserSalesModel> sales = new List<UserSalesModel>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/UserSales/GetAll").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonobject = JsonConvert.DeserializeObject(data);
                var extData = JsonConvert.SerializeObject(jsonobject);
                sales = JsonConvert.DeserializeObject<List<UserSalesModel>>(extData);

            }

            return View(sales);
        }
        #endregion
        #region SalesForm
        public async Task<IActionResult> SalesForm(int? SalesID)
        {
            await LoadProductList();
            await LoadCustomersList();
            if (SalesID.HasValue)
            {
                var response = await _client.GetAsync($"{_client.BaseAddress}/UserSales/GetbyID/{SalesID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var sales = JsonConvert.DeserializeObject<UserSalesModel>(data);
                    return View(sales);
                }
            }
            return View("SalesForm", new UserSalesModel());
        }
        #endregion
        #region SalesSave

        [HttpPost]
        public async Task<IActionResult> SalesSave([FromForm] UserSalesModel usersalesModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var json = JsonConvert.SerializeObject(usersalesModel);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response;

                    if (usersalesModel.SalesID == null || usersalesModel.SalesID == 0)
                    {
                        response = await _client.PostAsync($"{_client.BaseAddress}/UserSales/Insert", content);
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["Message"] = "Record Inserted Successfully";
                            return RedirectToAction("SalesList");
                        }
                    }

                    else
                    {
                        response = await _client.PutAsync($"{_client.BaseAddress}/UserSales/Update/{usersalesModel.SalesID}", content);
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["Message"] = "Record Updated Successfully";
                            return RedirectToAction("SalesList");
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
            await LoadCustomersList();
            return RedirectToAction("SalesList");
        }
        #endregion
        #region SalesDelete
        public async Task<IActionResult> SalesDelete(int SalesID)
        {
            var response = await _client.DeleteAsync($"{_client.BaseAddress}/UserSales/Delete/{SalesID}");
            return RedirectToAction("SalesList");
        }
        #endregion
        #region LoadProductList
        private async Task LoadProductList()
        {
            var response = await _client.GetAsync($"{_client.BaseAddress}/UserSales/ProductDropDown");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<List<ProductDropDownModel>>(data);
                ViewBag.productList = product;
            }
        }
        #endregion
        #region LoadCustomersList
        private async Task LoadCustomersList()
        {
            var response = await _client.GetAsync($"{_client.BaseAddress}/UserSales/CustomerDropDown");
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
