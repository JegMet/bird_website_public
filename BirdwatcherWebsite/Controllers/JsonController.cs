using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Newtonsoft.Json;

namespace BirdwatcherWebsite.Controllers
{
    public class JsonController : Controller
    {
        // POST: api/Json
        [HttpPost]
        public IActionResult PostJson([FromBody] object jsonInput)
        {
            // Log the received JSON data to the console
            string jsonString = JsonConvert.SerializeObject(jsonInput, Formatting.Indented);
            Console.WriteLine("Received JSON:");
            Console.WriteLine(jsonString);

           // System.Diagnostics.Debug.WriteLine(jsonString);


            // Optionally, return the JSON object back as a response
            return Ok(jsonInput);
        }
    }
}
