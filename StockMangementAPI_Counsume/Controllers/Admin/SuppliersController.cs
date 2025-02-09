using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockMangementAPI_Counsume.Models;
using System.Text;

namespace StockMangementAPI_Counsume.Controllers.Admin
{
    public class SuppliersController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5165/api");
        private readonly HttpClient _client;
        public SuppliersController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }
        #region SupplierList
        [HttpGet]
        public IActionResult SupplierList()
        {
            List<SuppliersModel> suppliers = new List<SuppliersModel>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Suppliers/GetAll").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonobject = JsonConvert.DeserializeObject(data);
                var extData = JsonConvert.SerializeObject(jsonobject);
                suppliers = JsonConvert.DeserializeObject<List<SuppliersModel>>(extData);

            }

            return View(suppliers);
        }
        #endregion
        #region SupplierForm
        public async Task<IActionResult> SupplierForm(int? SupplierID)
        {
            await LoadUserList();
            if (SupplierID.HasValue)
            {
                var response = await _client.GetAsync($"{_client.BaseAddress}/Suppliers/GetbyID/{SupplierID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var suppliers = JsonConvert.DeserializeObject<SuppliersModel>(data);
                    return View(suppliers);
                }
            }
            return View("SupplierForm", new SuppliersModel());
        }
        #endregion
        #region SupplierSave

        [HttpPost]
        public async Task<IActionResult> SupplierSave([FromForm] SuppliersModel suppliers)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var json = JsonConvert.SerializeObject(suppliers);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response;

                    if (suppliers.SupplierID == null || suppliers.SupplierID == 0)
                    {
                        response = await _client.PostAsync($"{_client.BaseAddress}/Suppliers/Insert", content);
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["Message"] = "Record Inserted Successfully";
                            return RedirectToAction("SupplierList");
                        }
                    }

                    else
                    {
                        response = await _client.PutAsync($"{_client.BaseAddress}/Suppliers/Update/{suppliers.SupplierID}", content);
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["Message"] = "Record Updated Successfully";
                            return RedirectToAction("SupplierList");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                Console.WriteLine(TempData["ErrorMessage"]);
            }
            await LoadUserList();
            return RedirectToAction("SupplierList");
        }
        #endregion
        #region SupplierDelete
        public async Task<IActionResult> SupplierDelete(int SupplierID)
        {
            var response = await _client.DeleteAsync($"{_client.BaseAddress}/Suppliers/Delete/{SupplierID}");
            return RedirectToAction("SupplierList");
        }
        #endregion
        #region LoadUserList
        private async Task LoadUserList()
        {
            var response = await _client.GetAsync($"{_client.BaseAddress}/Suppliers/DropDown");
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
