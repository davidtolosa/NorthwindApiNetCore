using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alba;
using Northwind.Api.Models;
using Xunit;
using FluentAssertions;
using Northwind.Api.Repository.SqlServer;
using GenFu;
using Northwind.Api.Integration.Tests.Builders;
using System.Net;

namespace Northwind.Api.Integration.Tests
{
    public class TestSuiteCustomerController : IClassFixture<WebApiFixture> 
    {
        private readonly SystemUnderTest _system;
        private readonly NorthwindDbContext _context;

        public TestSuiteCustomerController(WebApiFixture app){
            _system = app.systemUnderTest;
            _context = app.northwindDbContext;
        }
        
        [Fact]
        public async Task Verify_GetAllCustomers_200ResponseCode_With_Data()
        {
        //Given
            new CustomerBuilder(_context).With10Customers();
        //When
            var results =  await _system.GetAsJson<IList<Customer>>("/api/customer");
        //Then
            results.Count.Should().Be(10);
        }

        [Fact]
        public async Task Verify_GetAllCustomers_204ResponseCode()
        {
        //Given
            new CustomerBuilder(_context);
        //When
            await _system.Scenario(s => {
                s.Get.Url("/api/customer");

                //Then
                s.StatusCodeShouldBe(HttpStatusCode.NoContent);
            });      
        }

        [Fact]
        public async Task Verify_GetCustomer_200ResponseCod()
        {
        //Given
            var customerId = int.MaxValue-1;
            new CustomerBuilder(_context).WithOneCustomerAndIdValue(customerId);
        //When
            var customer = await _system.GetAsJson<Customer>($"/api/customer/{customerId}");
        //Then
            customer.Id.Should().Be(customerId);                
        }
    }
}
