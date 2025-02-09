using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockMangementAPI_Counsume.Models;
using System.Text;

namespace StockMangementAPI_Counsume.Controllers.User
{
    public class UserPurchaseController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5165/api");
        private readonly HttpClient _client;
        public UserPurchaseController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }
        #region PurchaseList
        [HttpGet]
        public IActionResult PurchaseList()
        {
            List<UserPurchaseModel> purchase = new List<UserPurchaseModel>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/UserPurchase/GetAll").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonobject = JsonConvert.DeserializeObject(data);
                var extData = JsonConvert.SerializeObject(jsonobject);
                purchase = JsonConvert.DeserializeObject<List<UserPurchaseModel>>(extData);

            }

            return View(purchase);
        }
        #endregion
        #region PurchaseForm
        public async Task<IActionResult> PurchaseForm(int? PurchaseID)
        {
            await LoadProductList();
            await LoadCustomersList();
            await LoadSuppliersList();
            if (PurchaseID.HasValue)
            {
                var response = await _client.GetAsync($"{_client.BaseAddress}/UserPurchase/GetbyID/{PurchaseID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var purchase = JsonConvert.DeserializeObject<UserPurchaseModel>(data);
                    return View(purchase);
                }
            }
            return View("PurchaseForm", new UserPurchaseModel());
        }
        #endregion
        #region PurchaseSave

        [HttpPost]
        public async Task<IActionResult> PurchaseSave([FromForm] UserPurchaseModel userpurchaseModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var json = JsonConvert.SerializeObject(userpurchaseModel);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response;

                    if (userpurchaseModel.PurchaseID == null || userpurchaseModel.PurchaseID == 0)
                    {
                        response = await _client.PostAsync($"{_client.BaseAddress}/UserPurchase/Insert", content);
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["Message"] = "Record Inserted Successfully";
                            return RedirectToAction("PurchaseList");
                        }
                    }

                    else
                    {
                        response = await _client.PutAsync($"{_client.BaseAddress}/UserPurchase/Update/{userpurchaseModel.PurchaseID}", content);
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["Message"] = "Record Updated Successfully";
                            return RedirectToAction("PurchaseList");
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
            return RedirectToAction("PurchaseList");
        }
        #endregion
        #region PurchaseDelete
        public async Task<IActionResult> PurchaseDelete(int PurchaseID)
        {
            var response = await _client.DeleteAsync($"{_client.BaseAddress}/UserPurchase/Delete/{PurchaseID}");
            return RedirectToAction("PurchaseList");
        }
        #endregion
        #region LoadProductList
        private async Task LoadProductList()
        {
            var response = await _client.GetAsync($"{_client.BaseAddress}/UserPurchase/ProductDropDown");
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
            var response = await _client.GetAsync($"{_client.BaseAddress}/UserPurchase/CustomerDropDown");
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
            var response = await _client.GetAsync($"{_client.BaseAddress}/UserPurchase/SuppliersDropDown");
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
