using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.CosmosDB;
using Microsoft.Azure.ServiceBus;
namespace FunctionApp4
{

    public class Product {
        public string id { get; set; }
        public int quantity { get; set; }
        public int Price { get; set; }

        public string Location { get; set; }
    }


    public static class ProductWrite
    {
        [FunctionName("ProductWrite")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] Product req,
            [CosmosDB("Product","ProductWrite",ConnectionStringSetting = "productwrite",Id ="{id}",PartitionKey ="{Location}")] IAsyncCollector<Product> products,
            [ServiceBus("ordertopic", Connection = "sbproduct", EntityType = Microsoft.Azure.WebJobs.ServiceBus.EntityType.Topic)] IAsyncCollector<Product> sbproduct,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            await products.AddAsync(req);
            await sbproduct.AddAsync(req);
            return (ActionResult)new OkObjectResult($"Your Product has been submitted !!");                
        }
    }
    public static class ProductRead
    {
        [FunctionName("ProductRead")]
        public static async Task Run(
            [ServiceBusTrigger("ordertopic", "orderreadsub", Connection = "sbproduct")] Product req,
            [CosmosDB("Product", "ProductRead", ConnectionStringSetting = "productwrite", Id = "{id}", PartitionKey = "{Location}")] IAsyncCollector<Product> products,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            await products.AddAsync(req);           

        }
    }
}
