using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ObservabilitySampleApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DogController : ControllerBase
    {
        private readonly ILogger<DogController> _logger;

        public DogController(ILogger<DogController> logger)
        {
            this._logger = logger;
        }
        
        // GET api/dog
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Getting a dog image from API running on k8s..");
            
            using (HttpClient client = new HttpClient())
            {
                // https://dog.ceo/dog-api/
                var response = await client.GetAsync("https://dog.ceo/api/breeds/image/random");
                
                _logger.LogInformation("Dog image response from API running on k8s {response}", response);
                
                response.EnsureSuccessStatusCode();
                
                var stringResult = await response.Content.ReadAsStringAsync();
                return new JsonResult(stringResult);
            }
        }
    }
}