using Backend.Entities.SecurityAccounts.Enums;
using Backend.Entities.Tables.DTO;
using Backend.Repository.Sample.Services.Contracts;
using BackendRestApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : BaseController
    {

        private iCustomerService serv;

        public CustomerController(iCustomerService _serv)
        {
            serv = _serv;
        }

        [Authorize(Role.User, Role.Admin)]
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

        [Authorize(Role.Admin, Role.User)]
        [HttpGet("GetCustomersBySearch/{searchString}", Name = nameof(GetCustomersBySearch))]
        [ProducesResponseType(200, Type = typeof(ResponseDTO<List<CustomerDTO>>))]
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

            return Ok(respuesta);
        }

        [Authorize(Role.Admin, Role.User)]
        [HttpGet("GetCustomerById/{id}", Name = nameof(GetCustomerById))]
        [ProducesResponseType(200, Type = typeof(ResponseDTO<List<CustomerDTO>>))]
        public async Task<IActionResult> GetCustomerById(long id)
        {
            ResponseDTO<List<CustomerDTO>> respuesta = new ResponseDTO<List<CustomerDTO>>();

            CustomerDTO result = await serv.GetDataById(id);
            List<CustomerDTO> resultList = new List<CustomerDTO>();

            resultList.Add(result);

            if (result != null)
            {
                respuesta.status = "200";
                respuesta.message = "Data loaded successfully.";
                respuesta.data = resultList;

                return Ok(respuesta);
            }

            respuesta.status = "404";
            respuesta.message = "Data not found.";
            respuesta.data = null;

            return Ok(respuesta);
        }

        [Authorize(Role.Admin, Role.User)]
        [HttpPost("CreateCustomer/{user}", Name = nameof(CreateCustomer))]
        [ProducesResponseType(201, Type = typeof(ResponseDTO<List<CustomerDTO>>))]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDTO customer, string user)
        {
            ResponseDTO<List<CustomerDTO>> respuesta = new ResponseDTO<List<CustomerDTO>>();

            if (customer == null || string.IsNullOrWhiteSpace(user))
            {
                respuesta.status = "400";
                respuesta.message = "Bad request, one of the parameters is missing.";
                respuesta.data = null;

                return Ok(respuesta);
            }

            if (!ModelState.IsValid)
            {
                respuesta.status = "400";
                respuesta.message = "Invalid data model.";
                respuesta.data = null;

                return Ok(respuesta);
            }

            CustomerDTO added = await serv.AddAsync(customer, user);
            List<CustomerDTO> addedList = new List<CustomerDTO>();

            addedList.Add(added);

            if (added != null)
            {
                respuesta.status = "201";
                respuesta.message = "Data saved successfully.";
                respuesta.data = addedList;

                return Ok(respuesta);
            }

            respuesta.status = "404";
            respuesta.message = "Data not saved.";
            respuesta.data = null;

            return Ok(respuesta);

        }

        [Authorize(Role.Admin, Role.User)]
        [HttpPut("UpdateCustomer/{user}", Name = nameof(UpdateCustomer))]
        [ProducesResponseType(204, Type = typeof(ResponseDTO<List<CustomerDTO>>))]
        public async Task<IActionResult> UpdateCustomer([FromBody] CustomerDTO customer, string user)
        {
            ResponseDTO<List<CustomerDTO>> respuesta = new ResponseDTO<List<CustomerDTO>>();

            if (customer == null || customer.Id == 0 || string.IsNullOrWhiteSpace(user))
            {
                respuesta.status = "400";
                respuesta.message = "Bad Request.";
                respuesta.data = null;

                return Ok(respuesta);
            }

            if (!ModelState.IsValid)
            {
                respuesta.status = "400";
                respuesta.message = "Invalid data model.";
                respuesta.data = null;

                return Ok(respuesta);
            }

            CustomerDTO result = await serv.UpdateAsync(customer, user);
            List<CustomerDTO> resultList = new List<CustomerDTO>();

            resultList.Add(result);

            if (result != null)
            {
                if (result.Id == 0)
                {
                    respuesta.status = "404";
                    respuesta.message = "Record not found.";
                    respuesta.data = null;

                    return Ok(respuesta);
                }

                respuesta.status = "204";
                respuesta.message = "Data saved successfully.";
                respuesta.data = resultList;

                return Ok(respuesta);
            }

            respuesta.status = "404";
            respuesta.message = "Changes not saved.";
            respuesta.data = null;

            return NotFound(respuesta);

        }

        [Authorize(Role.Admin, Role.User)]
        [HttpDelete("DeleteCustomer/{id}", Name = nameof(DeleteCustomer))]
        [ProducesResponseType(204, Type = typeof(ResponseDTO<List<CustomerDTO>>))]
        public async Task<IActionResult> DeleteCustomer(long id, string user)
        {
            ResponseDTO<List<CustomerDTO>> respuesta = new ResponseDTO<List<CustomerDTO>>();

            if (id <= 0 || string.IsNullOrWhiteSpace(user))
            {
                respuesta.status = "400";
                respuesta.message = "Bad Request.";
                respuesta.data = null;

                return Ok(respuesta);
            }

            CustomerDTO result = await serv.RemoveAsync(id, user);
            List<CustomerDTO> resultList = new List<CustomerDTO>();

            resultList.Add(result);

            if (result != null)
            {
                if (result.Id == 0)
                {
                    respuesta.status = "404";
                    respuesta.message = "Record not found.";
                    respuesta.data = null;

                    return Ok(respuesta);
                }

                respuesta.status = "204";
                respuesta.message = "Data deleted successfully.";
                respuesta.data = resultList;

                return Ok(respuesta);
            }

            respuesta.status = "404";
            respuesta.message = "Changes not saved.";
            respuesta.data = null;

            return Ok(respuesta);
        }

    }
}
