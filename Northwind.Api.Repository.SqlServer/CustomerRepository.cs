using System;
using Northwind.Api.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Api.Repository.SqlServer
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(NorthwindDbContext context) : base(context)
        {
        }

        public bool Exist(int id)
        {
           return _context.Customer.AsNoTracking().FirstOrDefault(c => c.Id == id) != null;
        }
    }
}
