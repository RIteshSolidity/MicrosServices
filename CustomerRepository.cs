using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CustomersLibrary;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace Customers
{
    public class CustomerRepository : ICustomerRepository
    {
        private IReliableStateManager _manager;
        public CustomerRepository(IReliableStateManager manager)
        {
            _manager = manager;
        }
        public async Task AddCustomer(CustomerEntity cust)
        {
           var customer = await _manager.GetOrAddAsync<IReliableDictionary<int, CustomerEntity>>("CustomerStore");
            using (var tx = _manager.CreateTransaction()) {
              var result =  await customer.AddOrUpdateAsync(tx, cust.CustomerId, cust, (id, val) => cust);
                await tx.CommitAsync();
            }

        }

        public async Task<IEnumerable<CustomerEntity>> GetAllCustomers()
        {
            var customer = await _manager.GetOrAddAsync<IReliableDictionary<int, CustomerEntity>>("CustomerStore");
            List<CustomerEntity> allCustomers = new List<CustomerEntity>();
            using (var tx = _manager.CreateTransaction()) {
                var custEnu = await customer.CreateEnumerableAsync(tx,EnumerationMode.Ordered);
                using (var enumerator = custEnu.GetAsyncEnumerator()) {
                    while (await enumerator.MoveNextAsync(CancellationToken.None)) {
                        KeyValuePair<int, CustomerEntity> custObjects = enumerator.Current;
                        allCustomers.Add(custObjects.Value);
                    }
                    //
                }

                return allCustomers;

            }
        }
    }
}
