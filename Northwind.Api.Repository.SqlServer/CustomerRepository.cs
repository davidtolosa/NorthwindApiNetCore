using System;
using System.Collections.Generic;
using Northwind.Api.Models;

namespace Northwind.Api.Repository.SqlServer
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(NorthwindDbContext context) : base(context)
        {
        }
    }
}
