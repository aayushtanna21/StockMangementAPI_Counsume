using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockMangementAPI_Counsume.Models;
using System.Text;

namespace StockMangementAPI_Counsume.Controllers
{
    public class UserController : Controller
    {
		Uri baseAddress = new Uri("http://localhost:5165/api");
		private readonly HttpClient _client;

		public UserController()
		{
			_client = new HttpClient();
			_client.BaseAddress = baseAddress;
		}
		#region UserList
		[HttpGet]
		public IActionResult UserList()
		{
			List<UserModel> user = new List<UserModel>();
			HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/User/GetAll").Result;
			if (response.IsSuccessStatusCode)
			{
				string data = response.Content.ReadAsStringAsync().Result;
				dynamic jsonobject = JsonConvert.DeserializeObject(data);
				var extData = JsonConvert.SerializeObject(jsonobject);
				user = JsonConvert.DeserializeObject<List<UserModel>>(extData);

			}

			return View(user);
		}
		#endregion
		#region UserForm
		public async Task<IActionResult> UserForm(int? UserID)
		{
			//await LoadUserList();
			if (UserID.HasValue)
			{
				var response = await _client.GetAsync($"{_client.BaseAddress}/User/GetbyID/{UserID}");
				if (response.IsSuccessStatusCode)
				{
					var data = await response.Content.ReadAsStringAsync();
					var user = JsonConvert.DeserializeObject<UserModel>(data);
					//ViewBag.userList = await GetStatesByCountryID(city.CountryID);
					return View(user);
				}
			}
			return View("UserForm", new UserModel());
		}
		#endregion
		#region UserSave

		[HttpPost]
		public async Task<IActionResult> UserSave([FromForm] UserModel user)
		{
			try  
			{
				if (ModelState.IsValid)
				{

					var json = JsonConvert.SerializeObject(user);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpResponseMessage response;

					if (user.UserID == null || user.UserID == 0)
					{
						response = await _client.PostAsync($"{_client.BaseAddress}/User/Insert", content);
						if (response.IsSuccessStatusCode)
						{
							TempData["Message"] = "Record Inserted Successfully";
							return RedirectToAction("UserList");
						}
					}

					else
					{
						response = await _client.PutAsync($"{_client.BaseAddress}/User/Update/{user.UserID}", content);
						if (response.IsSuccessStatusCode)
						{
							TempData["Message"] = "Record Updated Successfully";
							return RedirectToAction("UserList");
						}
					}
				}

				//if (response.IsSuccessStatusCode)
				//    return RedirectToAction("ProductDisplay");
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = ex.Message;
				Console.WriteLine(TempData["ErrorMessage"]);
			}
			//await LoadUserList();
			return RedirectToAction("UserList");
		}
		#endregion
		#region UserDelete
		public async Task<IActionResult> UserDelete(int UserID)
		{
			var response = await _client.DeleteAsync($"{_client.BaseAddress}/User/Delete/{UserID}");
			return RedirectToAction("UserList");
		}
		#endregion
	}
}
