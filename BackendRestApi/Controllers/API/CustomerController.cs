using Backend.Entities.Tables.DTO;
using Backend.Repository.Sample.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {

        private iCustomerService serv;

        public CustomerController(iCustomerService _serv)
        {
            serv = _serv;
        }

        /// <summary>
        /// Get all customers
        /// </summary>
        /// <returns>List of customers</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ResponseDTO<List<CustomerDTO>>))]
        [Route("GetCustomers")]
        public async Task<IActionResult> GetCustomers()
        {
            ResponseDTO<List<CustomerDTO>> respuesta = new ResponseDTO<List<CustomerDTO>>();

            var result = await serv.GetAll();

            if (result != null)
            {
                respuesta.status = "200";
                respuesta.message = "Data loaded successfully.";
                respuesta.data = result;

                return Ok(respuesta);
            }

            respuesta.status = "404";
            respuesta.message = "Data not found.";
            respuesta.data = null;

            return Ok(respuesta);
        }

        [HttpGet("GetCustomersBySearch/{searchString}", Name = nameof(GetCustomersBySearch))]
        [ProducesResponseType(200, Type = typeof(ResponseDTO<List<CustomerDTO>>))]
        [ProducesResponseType(404, Type = typeof(ResponseDTO<string>))]
        public async Task<IActionResult> GetCustomersBySearch(string searchString)
        {
            ResponseDTO<List<CustomerDTO>> respuesta = new ResponseDTO<List<CustomerDTO>>();

            var result = await serv.GetAllBySearch(searchString);

            if (result != null)
            {
                respuesta.status = "200";
                respuesta.message = "Data loaded successfully.";
                respuesta.data = result;

                return Ok(respuesta);
            }

            respuesta.status = "404";
            respuesta.message = "Data not found.";
            respuesta.data = null;

            return NotFound(respuesta);
        }

        [HttpGet("GetCustomerById/{id}", Name = nameof(GetCustomerById))]
        [ProducesResponseType(200, Type = typeof(ResponseDTO<CustomerDTO>))]
        [ProducesResponseType(404, Type = typeof(ResponseDTO<string>))]
        public async Task<IActionResult> GetCustomerById(long id)
        {
            ResponseDTO<CustomerDTO> respuesta = new ResponseDTO<CustomerDTO>();

            var result = await serv.GetDataById(id);

            if (result != null)
            {
                respuesta.status = "200";
                respuesta.message = "Data loaded successfully.";
                respuesta.data = result;

                return Ok(respuesta);
            }

            respuesta.status = "404";
            respuesta.message = "Data not found.";
            respuesta.data = null;

            return NotFound(respuesta);
        }

        [HttpPost("CreateCustomer/{user}", Name = nameof(CreateCustomer))]
        [ProducesResponseType(201, Type = typeof(ResponseDTO<long>))]
        [ProducesResponseType(400, Type = typeof(ResponseDTO<long>))]
        [ProducesResponseType(404, Type = typeof(ResponseDTO<long>))]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDTO customer, string user)
        {
            ResponseDTO<long> respuesta = new ResponseDTO<long>();

            if (customer == null || string.IsNullOrWhiteSpace(user))
            {
                respuesta.status = "400";
                respuesta.message = "Bad request, one of the parameters is missing.";
                respuesta.data = 0;

                return BadRequest(respuesta);
            }

            if (!ModelState.IsValid)
            {
                respuesta.status = "400";
                respuesta.message = "Invalid data model.";
                respuesta.data = 0;

                return BadRequest(respuesta);
            }

            long added = await serv.AddAsync(customer, user);

            if (added != 0)
            {
                respuesta.status = "201";
                respuesta.message = "Data saved successfully.";
                respuesta.data = added;

                return Ok(respuesta);
            }

            respuesta.status = "404";
            respuesta.message = "Data not saved.";
            respuesta.data = 0;

            return NotFound(respuesta);

        }

        [HttpPut("UpdateCustomer/{user}", Name = nameof(UpdateCustomer))]
        [ProducesResponseType(204, Type = typeof(ResponseDTO<CustomerDTO>))]
        [ProducesResponseType(400, Type = typeof(ResponseDTO<string>))]
        [ProducesResponseType(404, Type = typeof(ResponseDTO<string>))]
        public async Task<IActionResult> UpdateCustomer([FromBody] CustomerDTO customer, string user)
        {
            ResponseDTO<CustomerDTO> respuesta = new ResponseDTO<CustomerDTO>();

            if (customer == null || customer.Id == 0 || string.IsNullOrWhiteSpace(user))
            {
                respuesta.status = "400";
                respuesta.message = "Bad Request.";
                respuesta.data = null;

                return BadRequest(respuesta);
            }

            if (!ModelState.IsValid)
            {
                respuesta.status = "400";
                respuesta.message = "Invalid data model.";
                respuesta.data = null;

                return BadRequest(respuesta);
            }

            var result = await serv.UpdateAsync(customer, user);

            if (result != null)
            {
                if (result.Id == 0)
                {
                    respuesta.status = "404";
                    respuesta.message = "Record not found.";
                    respuesta.data = null;

                    return NotFound(respuesta);
                }

                respuesta.status = "204";
                respuesta.message = "Data saved successfully.";
                respuesta.data = result;

                return Ok(respuesta);
            }

            respuesta.status = "404";
            respuesta.message = "Changes not saved.";
            respuesta.data = null;

            return NotFound(respuesta);

        }

        [HttpDelete("DeleteCustomer/{id}", Name = nameof(DeleteCustomer))]
        [ProducesResponseType(204, Type = typeof(ResponseDTO<CustomerDTO>))]
        [ProducesResponseType(400, Type = typeof(ResponseDTO<string>))]
        [ProducesResponseType(404, Type = typeof(ResponseDTO<string>))]
        public async Task<IActionResult> DeleteCustomer(long id, string user)
        {
            ResponseDTO<CustomerDTO> respuesta = new ResponseDTO<CustomerDTO>();

            if (id <= 0 || string.IsNullOrWhiteSpace(user))
            {
                respuesta.status = "400";
                respuesta.message = "Bad Request.";
                respuesta.data = null;

                return BadRequest(respuesta);
            }

            var result = await serv.RemoveAsync(id, user);

            if (result != null)
            {
                if (result.Id == 0)
                {
                    respuesta.status = "404";
                    respuesta.message = "Record not found.";
                    respuesta.data = null;

                    return NotFound(respuesta);
                }

                respuesta.status = "204";
                respuesta.message = "Data deleted successfully.";
                respuesta.data = result;

                return Ok(respuesta);
            }

            respuesta.status = "404";
            respuesta.message = "Changes not saved.";
            respuesta.data = null;

            return NotFound(respuesta);
        }

    }
}
