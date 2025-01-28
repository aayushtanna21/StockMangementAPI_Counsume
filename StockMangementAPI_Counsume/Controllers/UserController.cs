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
		public async Task<IActionResult> UserForm(int? UserID)
		{
			if (UserID.HasValue)
			{
				List<UserModel> user = new List<UserModel>();
				HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/NewUser/GetByID/{UserID}").Result;
				if (response.IsSuccessStatusCode)
				{
					string data = response.Content.ReadAsStringAsync().Result;
					dynamic jsonobject = JsonConvert.DeserializeObject(data);
					var extData = JsonConvert.SerializeObject(jsonobject);
					user = JsonConvert.DeserializeObject<List<UserModel>>(extData);
					//ViewBag.userList = await GetStatesByCountryID(city.CountryID);
					UserModel userModel = new UserModel
					{
						UserID = user[0].UserID,
						UserName = user[0].UserName,
						Password = user[0].Password,
						Email = user[0].Email,
						PhoneNumber = user[0].PhoneNumber,
						Created = user[0].Created,
						Modified = user[0].Modified
					};
					return View(userModel);
				}
				else
				{
					return RedirectToAction("UserList");
				}

			}
			return View("UserForm", new UserModel());
		}

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

					if (user.UserID == null)
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
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = ex.Message;
				Console.WriteLine(TempData["ErrorMessage"]);
			}
			return RedirectToAction("UserList");
		}
		public async Task<IActionResult> Delete(int UserID)
		{
			var response = await _client.DeleteAsync($"{_client.BaseAddress}/User/Delete/{UserID}");
			return RedirectToAction("Index");
		}
	}
}
