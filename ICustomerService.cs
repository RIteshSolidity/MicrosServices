using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CustomersLibrary
{
    public interface ICustomerService : IService
    {
        Task NewCustomer(CustomerEntity cust);
        Task<IEnumerable<CustomerEntity>> GetAllCustomers();
    }
}
