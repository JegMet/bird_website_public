using BirdwatcherWebsite.Data;
using BirdwatcherWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BirdwatcherWebsite.Controllers
{
    public class JsonController : Controller
    {
        private readonly BirdwatcherWebsiteContext _context;

        public JsonController(BirdwatcherWebsiteContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostJson([FromBody] JObject jsonInput)
        {
            try
            {
                // Log the received JSON data to the console
                string jsonString = JsonConvert.SerializeObject(jsonInput, Formatting.Indented);
                Console.WriteLine("Received JSON:");
                Console.WriteLine(jsonString);

                // Extract image data from JSON object
                string base64Image = jsonInput["image"].ToString();
                byte[] imageBytes = Convert.FromBase64String(base64Image);

                // Store image file and get the file path
                string filePath = StoreImage(imageBytes);

                // Create a new Picture entity
                var picture = new Picture
                {
                    ImagePath = filePath,
                    DateTimeImgTaken = DateTime.Now, // Consider extracting from JSON if available
                    BirdType = "Unknown" // Add a way to include bird type if your JSON includes it
                };

                // Save the Picture entity to the database
                _context.Add(picture);
                await _context.SaveChangesAsync();

                // Build the absolute URL to the stored image
                var request = HttpContext.Request;
                var baseUrl = $"{request.Scheme}://{request.Host}";
                var imageUrl = $"{baseUrl}/Photos/{filePath}";

                return Ok(new { Message = "Image processed and stored successfully", ImageUrl = imageUrl });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing image: " + ex.Message);
                return BadRequest("Error processing image");
            }
        }

        private string StoreImage(byte[] imageBytes)
        {
            string fileName = GenerateUniqueFileName("jpg");
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Photos", fileName);

            // Ensure the Photos directory exists
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Write the image bytes to the file
            System.IO.File.WriteAllBytes(filePath, imageBytes);
            Console.WriteLine($"Image saved to {filePath}");

            return fileName;
        }

        private string GenerateUniqueFileName(string fileExtension)
        {
            // Generate a unique file name using a GUID
            return $"{Guid.NewGuid()}.{fileExtension}";
        }
    }
}
