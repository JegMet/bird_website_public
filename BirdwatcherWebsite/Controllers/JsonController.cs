using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BirdwatcherWebsite.Controllers
{
    public class JsonController : Controller
    {
        [HttpPost]
        public IActionResult PostJson([FromBody] JObject jsonInput)
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
                StoreImage(imageBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing image: " + ex.Message);
                return BadRequest("Error processing image");
            }

            return Ok("Image processed and stored successfully.");
        }

        private void StoreImage(byte[] imageBytes)
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
        }

        private string GenerateUniqueFileName(string fileExtension)
        {
            // Generate a unique file name using a GUID
            return $"{Guid.NewGuid()}.{fileExtension}";
        }
    }
}
