using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ObservabilitySampleApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DogController : ControllerBase
    {
        // GET api/dog
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Log.Information("Getting a dog image from API running on k8s..");
            
            using (HttpClient client = new HttpClient())
            {
                // https://dog.ceo/dog-api/
                var response = await client.GetAsync("https://dog.ceo/api/breeds/image/random");

                var smallResponse = new
                {
                    response.StatusCode,
                    response.ReasonPhrase,
                    response.RequestMessage.RequestUri,
                    response.RequestMessage.Method.Method
                };
                    
                Log.Information("Dog image response from API running on k8s {@response}", smallResponse);
                
                response.EnsureSuccessStatusCode();
                
                var stringResult = await response.Content.ReadAsStringAsync();
                return new JsonResult(stringResult);
            }
        }
    }
}