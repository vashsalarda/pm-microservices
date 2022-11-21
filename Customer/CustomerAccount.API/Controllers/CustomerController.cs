using CustomerAccount.API.Entities;
using CustomerAccount.API.Repositories.Interfaces;
using DnsClient.Internal;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
	[ApiController]
	[Route("api/customers")]
	public class CustomerController : ControllerBase
	{
		private readonly ICustomerRepository _repository;
		private readonly ILogger<CustomerController> _logger;

		public CustomerController(ICustomerRepository repository, ILogger<CustomerController> logger)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<Customer>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
		{
			var customers = await _repository.GetCustomers();
			return Ok(customers);
		}

		[HttpGet("{id}", Name = "GetCustomer")]
		[ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Customer>> GetCustomer(string id)
		{
			var customer = await _repository.GetCustomer(id);

			if (customer == null)
			{
				_logger.LogError($"Customer with id: {id}, not found.");
				return NotFound();
			}

			return Ok(customer);
		}

		[HttpPost]
		[ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Customer>> CreateCustomer([FromBody] CustomerCreate payload)
		{
			Customer customer = new()
			{
				Id = Guid.NewGuid().ToString(),
				UserId = payload.UserId,
				Email = payload.Email,
				FirstName = payload.FirstName,
				LastName = payload.LastName,
				MobileNumber = payload.MobileNumber,
				Country = payload.Country,
				State = payload.State,
				City = payload.City
			};

			await _repository.CreateCustomer(customer);

			return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
		}

		[HttpPatch("{id}", Name = "UpdateCustomer")]
		[ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> UpdateCustomer([FromBody] CustomerUpdate payload, string id)
		{
			Customer customer = new();

			if (payload.Email != null)
				customer.Email = payload.Email;
			if (payload.FirstName != null)
				customer.FirstName = payload.FirstName;
			if (payload.LastName != null)
				customer.LastName = payload.LastName;
			if (payload.MobileNumber != null)
				customer.MobileNumber = payload.MobileNumber;
			if (payload.Country != null)
				customer.Country = payload.Country;
			if (payload.State != null)
				customer.State = payload.State;
			if (payload.City != null)
				customer.City = payload.City;

			if (await _repository.UpdateCustomer(customer, id))
				return NoContent();
			else
				return NotFound();
		}

		[HttpDelete("{id}", Name = "DeleteCustomer")]
		[ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> DeleteCustomer(string id)
		{
			if (await _repository.DeleteCustomer(id))
				return NoContent();
			else
				return NotFound();
		}
	}
}
