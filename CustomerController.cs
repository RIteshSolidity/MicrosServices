using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CustomersLibrary;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace CustomerClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private ICustomerService _service;
        public ValuesController()
        {
           _service =  ServiceProxy.Create<ICustomerService>(new Uri("fabric:/Flipkart/Customers"), new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(0));
        }
        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<CustomerEntity>> Get()
        {
           return await _service.GetAllCustomers();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] CustomerEntity  value)
        {
            _service.NewCustomer(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
