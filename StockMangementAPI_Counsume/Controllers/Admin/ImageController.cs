using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
namespace StockMangementAPI_Counsume.Controllers.Admin
{

    public class ImageController : Controller
    {
        private readonly HttpClient _httpClient;

        public ImageController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5000/api/image/");
        }

        // Method to upload image
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError("", "Please select an image.");
                return View();
            }

            using (var content = new MultipartFormDataContent())
            {
                var stream = imageFile.OpenReadStream();
                content.Add(new StreamContent(stream), "imageFile", imageFile.FileName);

                var response = await _httpClient.PostAsync("upload", content);
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Image uploaded successfully!";
                }
                else
                {
                    ViewBag.Message = "Image upload failed.";
                }
            }

            return View();
        }

        // Method to fetch and display the image
        [HttpGet]
        public async Task<IActionResult> DisplayImage(string fileName)
        {
            var response = await _httpClient.GetAsync($"get/{fileName}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound("Image not found");
            }

            var base64Data = await response.Content.ReadAsStringAsync();
            ViewBag.Base64Image = base64Data;

            return View();
        }
    }
}

