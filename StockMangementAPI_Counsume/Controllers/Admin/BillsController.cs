using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockMangementAPI_Counsume.Models;
using System.Text;


namespace StockMangementAPI_Counsume.Controllers.Admin
{
    public class BillsController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5165/api");
        private readonly HttpClient _client;
        public BillsController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }
        #region BillsList
        [HttpGet]
        public IActionResult BillsList()
        {
            List<BillsModel> bills = new List<BillsModel>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Bills/GetAll").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonobject = JsonConvert.DeserializeObject(data);
                var extData = JsonConvert.SerializeObject(jsonobject);
                bills = JsonConvert.DeserializeObject<List<BillsModel>>(extData);

            }

            return View(bills);
        }
        #endregion
        #region BillsForm
        public async Task<IActionResult> BillsForm(int? BillID)
        {
            await LoadCustomersList();
            await LoadUserList();
            if (BillID.HasValue)
            {
                var response = await _client.GetAsync($"{_client.BaseAddress}/Bills/GetbyID/{BillID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var bills = JsonConvert.DeserializeObject<BillsModel>(data);
                    return View(bills);
                }
            }
            return View("BillsForm", new BillsModel());
        }
        #endregion
        #region BillsSave

        [HttpPost]
        public async Task<IActionResult> BillsSave([FromForm] BillsModel bills)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var json = JsonConvert.SerializeObject(bills);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response;

                    if (bills.BillID == null || bills.BillID == 0)
                    {
                        response = await _client.PostAsync($"{_client.BaseAddress}/Bills/Insert", content);
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["Message"] = "Record Inserted Successfully";
                            return RedirectToAction("BillsList");
                        }
                    }

                    else
                    {
                        response = await _client.PutAsync($"{_client.BaseAddress}/Bills/Update/{bills.BillID}", content);
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["Message"] = "Record Updated Successfully";
                            return RedirectToAction("BillsList");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                Console.WriteLine(TempData["ErrorMessage"]);
            }
            await LoadCustomersList();
            await LoadUserList();
            return RedirectToAction("BillsList");
        }
        #endregion
        #region BillsDelete
        public async Task<IActionResult> BillsDelete(int BillID)
        {
            var response = await _client.DeleteAsync($"{_client.BaseAddress}/Bills/Delete/{BillID}");
            return RedirectToAction("BillsList");
        }
        #endregion
        #region LoadCustomersList
        private async Task LoadCustomersList()
        {
            var response = await _client.GetAsync($"{_client.BaseAddress}/Bills/CustomerDropDown");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var customers = JsonConvert.DeserializeObject<List<CustomersDropDownModel>>(data);
                ViewBag.customersList = customers;
            }
        }
        #endregion
        #region LoadUserList
        private async Task LoadUserList()
        {
            var response = await _client.GetAsync($"{_client.BaseAddress}/Bills/UserDropDown");
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
