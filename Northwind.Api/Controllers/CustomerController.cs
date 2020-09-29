using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Northwind.Api.Repository;
using Northwind.Api.Models;
using System.Linq;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using AutoMapper;

namespace Northwind.Api.Controllers
{
    public class CustomerController : ApiController
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerRepository repository, IMapper mapper){
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [SwaggerOperation("Get Customers","List of customers from database")]
        [SwaggerResponse((int) HttpStatusCode.OK,"List of customers",typeof(IEnumerable<Models.Dto.Customer>))]
        [SwaggerResponse((int) HttpStatusCode.NoContent, "No customers")]
        public ActionResult<IEnumerable<Models.Dto.Customer>> GetAllCustomers(){
            var result = _repository.ReadAll();
            
            if(!result.Any()) return NoContent();
            
            return Ok(_mapper.Map<Models.Dto.Customer>(_repository.ReadAll()));
        }

        [HttpGet("{id:int}", Name = "GetCustomer")]
        public ActionResult<Customer> GetCustomer([FromRoute] int id){
            if( id <= 0) return BadRequest();

            var result = _repository.Read(id);
           
            if(result == null) return NotFound();
            
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<Customer> Post([FromBody] Customer customer){

            if( customer == null) return BadRequest();

            _repository.Create(customer);

            return CreatedAtAction("GetCustomer", new {id = customer.Id}, customer);
        }

        [HttpPut]
        public ActionResult<Customer> Put([FromBody] Customer customer){
            if (customer == null) return BadRequest();

            if(!_repository.Exist(customer.Id)) return NoContent();

            _repository.Update(customer);

            return Ok(customer);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Customer> Delete([FromRoute] int id){
            if (id <= 0) return BadRequest();

            var customer = _repository.Read(id);

            if(customer == null) return NoContent();

            _repository.Delete(customer);

            return Ok();
        }
    }
}
