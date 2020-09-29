using System;
using System.Collections.Generic;
using Northwind.Api.Models;
using Northwind.Api.Repository.SqlServer;
using GenFu;

namespace Northwind.Api.Integration.Tests.Builders
{
    public class CustomerBuilder
    {
        private readonly NorthwindDbContext _contex;
        public CustomerBuilder(NorthwindDbContext context)
        {
            _contex = context;
            CleanCustomerTable();
        }

        private void CleanCustomerTable()
        {
            _contex.RemoveRange(_contex.Customer);
            _contex.SaveChanges();
        }

        public CustomerBuilder WithSpecificCustomer(Customer customer){
            AddCustomer(customer);
            return this;
        }

        public CustomerBuilder WithOneCustomerAndIdValue(int id){
            A.Configure<Customer>().Fill(c => c.Id, () => {return id;});

            AddCustomer(A.New<Customer>());

            return this;
        }

        public CustomerBuilder With10Customers(){
            AddCustomersToDbContext(CreateCustomer(10));
           h
        }

        private void AddCustomersToDbContext(IEnumerable<Customer> customers)
        {
            _contex.AddRange(customers);
            _contex.SaveChanges();
        }

        private IEnumerable<Customer> CreateCustomer(int quantity)
        {
            int id = 1;
            GenFu.GenFu.Configure<Customer>().Fill(c => c.Id, () => {return id++;});

            return A.ListOf<Customer>(quantity);
        }

        private void AddCustomer(Customer customer)
        {
            _contex.Add(customer);
            _contex.SaveChanges();
        }

        public NorthwindDbContext Build(){
            return _contex;
        }
    }
}
