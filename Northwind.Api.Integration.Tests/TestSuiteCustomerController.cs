using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alba;
using Northwind.Api.Models;
using Xunit;
using FluentAssertions;

namespace Northwind.Api.Integration.Tests
{
    public class TestSuiteCustomerController : IClassFixture<WebApiFixture> 
    {
        public readonly SystemUnderTest _system;

        public TestSuiteCustomerController(WebApiFixture app){
            _system = app.systemUnderTest;
        }
        
        [Fact]
        public async Task Verify_GetAllCustomers_200ResponseCode_With_Data()
        {
        //Given
        
        //When
            var results =  await _system.GetAsJson<IList<Customer>>("/api/customer");
        //Then
            results.Count.Should().BeGreaterThan(0);
        }
    }
}
