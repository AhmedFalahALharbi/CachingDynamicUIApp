using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CachingDynamicUIApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _env;

        public ImagesController(IWebHostEnvironment env)
        {
            _httpClient = new HttpClient();
            _env = env;
        }

        [HttpGet("avatar/{email}")]
        public async Task<IActionResult> GetAvatar(string email)
        {
            try
            {
                // Create a cache folder if it doesn't exist
                var cacheFolder = Path.Combine(_env.WebRootPath, "cache", "avatars");
                Directory.CreateDirectory(cacheFolder);

                // Generate a filename based on the email
                var filename = $"{Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(email)).Replace("/", "_").Replace("+", "-")}.webp";
                var cacheFilePath = Path.Combine(cacheFolder, filename);

                // Check if the image exists in cache
                if (System.IO.File.Exists(cacheFilePath))
                {
                    var bytes = await System.IO.File.ReadAllBytesAsync(cacheFilePath);
                    return File(bytes, "image/webp");
                }

                // If not in cache, get from avatar service
                var avatarUrl = $"https://i.pravatar.cc/150?u={email}";
                var response = await _httpClient.GetAsync(avatarUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return NotFound();
                }

                var imageBytes = await response.Content.ReadAsByteArrayAsync();

                // For a real implementation, you would convert the image to WebP format here
                // For this demo, we'll simply save the original image
                await System.IO.File.WriteAllBytesAsync(cacheFilePath, imageBytes);

                // Return the image with the correct content type
                return File(imageBytes, response.Content.Headers.ContentType.ToString());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}