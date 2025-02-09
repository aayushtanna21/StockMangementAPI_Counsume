using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockMangementAPI_Counsume.Models;
using System.Text;

namespace StockMangementAPI_Counsume.Controllers.User
{
    public class UserPurchaseReturnController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5165/api");
        private readonly HttpClient _client;
        public UserPurchaseReturnController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }
        #region PurchaseReturnList
        [HttpGet]
        public IActionResult PurchaseReturnList()
        {
            List<UserPurchaseReturnModel> purchasereturn = new List<UserPurchaseReturnModel>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/UserPurchaseReturn/GetAll").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonobject = JsonConvert.DeserializeObject(data);
                var extData = JsonConvert.SerializeObject(jsonobject);
                purchasereturn = JsonConvert.DeserializeObject<List<UserPurchaseReturnModel>>(extData);

            }

            return View(purchasereturn);
        }
        #endregion
        #region PurchaseReturnForm
        public async Task<IActionResult> PurchaseReturnForm(int? PurchaseReturnID)
        {
            await LoadProductList();
            await LoadCustomersList();
            await LoadSuppliersList();
            if (PurchaseReturnID.HasValue)
            {
                var response = await _client.GetAsync($"{_client.BaseAddress}/UserPurchaseReturn/GetbyID/{PurchaseReturnID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var purchasereturn = JsonConvert.DeserializeObject<UserPurchaseReturnModel>(data);
                    return View(purchasereturn);
                }
            }
            return View("PurchaseReturnForm", new UserPurchaseReturnModel());
        }
        #endregion
        #region PurchaseReturnSave

        [HttpPost]
        public async Task<IActionResult> PurchaseReturnSave([FromForm] UserPurchaseReturnModel userpurchaseReturnModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var json = JsonConvert.SerializeObject(userpurchaseReturnModel);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response;

                    if (userpurchaseReturnModel.PurchaseReturnID == null || userpurchaseReturnModel.PurchaseReturnID == 0)
                    {
                        response = await _client.PostAsync($"{_client.BaseAddress}/UserPurchaseReturn/Insert", content);
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["Message"] = "Record Inserted Successfully";
                            return RedirectToAction("PurchaseReturnList");
                        }
                    }

                    else
                    {
                        response = await _client.PutAsync($"{_client.BaseAddress}/UserPurchaseReturn/Update/{userpurchaseReturnModel.PurchaseReturnID}", content);
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["Message"] = "Record Updated Successfully";
                            return RedirectToAction("PurchaseReturnList");
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
            await LoadSuppliersList();
            return RedirectToAction("PurchaseReturnList");
        }
        #endregion
        #region PurchaseReturnDelete
        public async Task<IActionResult> PurchaseReturnDelete(int PurchaseReturnID)
        {
            var response = await _client.DeleteAsync($"{_client.BaseAddress}/UserPurchaseReturn/Delete/{PurchaseReturnID}");
            return RedirectToAction("PurchaseReturnList");
        }
        #endregion
        #region LoadProductList
        private async Task LoadProductList()
        {
            var response = await _client.GetAsync($"{_client.BaseAddress}/UserPurchaseReturn/ProductDropDown");
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
            var response = await _client.GetAsync($"{_client.BaseAddress}/UserPurchaseReturn/CustomerDropDown");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var customers = JsonConvert.DeserializeObject<List<CustomersDropDownModel>>(data);
                ViewBag.customersList = customers;
            }
        }
        #endregion
        #region LoadSuppliersList
        private async Task LoadSuppliersList()
        {
            var response = await _client.GetAsync($"{_client.BaseAddress}/UserPurchaseReturn/SuppliersDropDown");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var suppliers = JsonConvert.DeserializeObject<List<SuppliersDropDownModel>>(data);
                ViewBag.suppliersList = suppliers;
            }
        }
        #endregion
    }
}
