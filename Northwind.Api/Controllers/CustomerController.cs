using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Northwind.Api.Repository;
using Northwind.Api.Models;
using System.Linq;

namespace Northwind.Api.Controllers
{
    public class CustomerController : ApiController
    {
        private readonly ICustomerRepository _repository;

        public CustomerController(ICustomerRepository repository){
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Customer>> GetAllCustomers(){
            var result = _repository.ReadAll();
            
            if(!result.Any()) return NoContent();
            
            return Ok(_repository.ReadAll());
        }

        [HttpGet("{id:int}", Name = "GetCustomer")]
        public ActionResult<Customer> GetCustomer([FromRoute] int id){
            if( id <= 0) return BadRequest();

            var result = _repository.Read(id);
           
            if(result == null) return NotFound();
            
            return Ok(result);
        }
    }
}
