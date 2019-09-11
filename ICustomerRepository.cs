using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CustomersLibrary;

namespace Customers
{
    public interface ICustomerRepository
    {

        Task AddCustomer(CustomerEntity cust);

        Task<IEnumerable<CustomerEntity>> GetAllCustomers();
    }
}
