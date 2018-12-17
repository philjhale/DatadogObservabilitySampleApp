using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;

namespace ObservabilitySampleApp.FunctionApps
{
    public static class GetDogImage
    {
        [FunctionName("GetDogImage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "getdogimage")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Getting a dog image from Function App...");

            using (HttpClient client = new HttpClient())
            {
                // https://dog.ceo/dog-api/
                var response = await client.GetAsync("https://dog.ceo/api/breeds/image/random");
                
                // Structured logging is supported out of the box
                log.LogInformation($"Dog image response from Function App {response}", response);
                
                response.EnsureSuccessStatusCode();
                
                var stringResult = await response.Content.ReadAsStringAsync();
                return new JsonResult(stringResult);
            }
        }
    }
}
