using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockMangementAPI_Counsume.Models;
using System.Text;

namespace StockMangementAPI_Counsume.Controllers.User
{
    public class UserSalesReturnController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5165/api");
        private readonly HttpClient _client;
        public UserSalesReturnController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }
        #region SalesReturnList
        [HttpGet]
        public IActionResult SalesReturnList()
        {
            List<UserSalesReturnModel> salesreturn = new List<UserSalesReturnModel>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/UserSalesReturn/GetAll").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonobject = JsonConvert.DeserializeObject(data);
                var extData = JsonConvert.SerializeObject(jsonobject);
                salesreturn = JsonConvert.DeserializeObject<List<UserSalesReturnModel>>(extData);

            }

            return View(salesreturn);
        }
        #endregion
        #region SalesReturnForm
        public async Task<IActionResult> SalesReturnForm(int? SalesReturnID)
        {
            await LoadProductList();
            await LoadCustomersList();
            if (SalesReturnID.HasValue)
            {
                var response = await _client.GetAsync($"{_client.BaseAddress}/UserSalesReturn/GetbyID/{SalesReturnID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var salesreturn = JsonConvert.DeserializeObject<UserSalesReturnModel>(data);
                    return View(salesreturn);
                }
            }
            return View("SalesReturnForm", new UserSalesReturnModel());
        }
        #endregion
        #region SalesReturnSave

        [HttpPost]
        public async Task<IActionResult> SalesReturnSave([FromForm] UserSalesReturnModel usersalesreturnModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var json = JsonConvert.SerializeObject(usersalesreturnModel);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response;

                    if (usersalesreturnModel.SalesReturnID == null || usersalesreturnModel.SalesReturnID == 0)
                    {
                        response = await _client.PostAsync($"{_client.BaseAddress}/UserSalesReturn/Insert", content);
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["Message"] = "Record Inserted Successfully";
                            return RedirectToAction("SalesReturnList");
                        }
                    }

                    else
                    {
                        response = await _client.PutAsync($"{_client.BaseAddress}/UserSalesReturn/Update/{usersalesreturnModel.SalesReturnID}", content);
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["Message"] = "Record Updated Successfully";
                            return RedirectToAction("SalesReturnList");
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
            return RedirectToAction("SalesReturnList");
        }
        #endregion
        #region SalesReturnDelete
        public async Task<IActionResult> SalesReturnDelete(int SalesReturnID)
        {
            var response = await _client.DeleteAsync($"{_client.BaseAddress}/UserSalesReturn/Delete/{SalesReturnID}");
            return RedirectToAction("SalesReturnList");
        }
        #endregion
        #region LoadProductList
        private async Task LoadProductList()
        {
            var response = await _client.GetAsync($"{_client.BaseAddress}/UserSalesReturn/ProductDropDown");
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
            var response = await _client.GetAsync($"{_client.BaseAddress}/UserSalesReturn/CustomerDropDown");
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
