using Microsoft.AspNetCore.Mvc;
using CustomerAccount.API.Data;
using CustomerAccount.API.Entities;
using CustomerAccount.API.Repositories.Interfaces;
using System.Net;

namespace CustomerAccount.API.Controllers
{
	[Route("api/organizations")]
	[ApiController]
	public class OrganizationController : ControllerBase
	{
		private readonly IOrganizationRepository _repository;
		private readonly ILogger<UserController> _logger;
		private readonly CustomerAccountContext _context;

		public OrganizationController(IOrganizationRepository repository, ILogger<UserController> logger, CustomerAccountContext context)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_context = context;
		}

		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<Organization>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<Organization>>> GetOrganizations()
		{
			var users = await _repository.GetOrganizations();
			return Ok(users);
		}

		[HttpGet("{id}", Name = "GetOrganization")]
		[ProducesResponseType(typeof(Organization), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Organization>> GetOrganization(string id)
		{
			var organization = await _repository.GetOrganization(id);

			if (organization == null)
			{
				_logger.LogError($"Organization with id: {id}, not found.");
				return NotFound();
			}

			return Ok(organization);
		}

		[HttpPost]
		[ProducesResponseType(typeof(Organization), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Organization>> CreateOrganization(Organization organization)
		{
			organization.Id = Guid.NewGuid().ToString();
			await _repository.CreateOrganization(organization);

			return CreatedAtAction("GetOrganization", new { id = organization.Id }, organization);
		}

		[HttpPatch("{id}")]
		public async Task<IActionResult> UpdateOrganization(string id, OrganizationUpdate payload)
		{
			Organization organization = new();

			if (payload.CompanyName != null)
				organization.CompanyName = payload.CompanyName;
			if (payload.Country != null)
				organization.Country = payload.Country;
			if (payload.State != null)
				organization.State = payload.State;
			if (payload.City != null)
				organization.City = payload.City;
			if (payload.MobileNumber != null)
				organization.MobileNumber = payload.MobileNumber;
			if (payload.Landline != null)
				organization.Landline = payload.Landline;
			if (payload.Industry != null)
				organization.Industry = payload.Industry;
			if (payload.Language != null)
				organization.Language = payload.Language;
			if (payload.CompanySize > 0)
				organization.CompanySize = payload.CompanySize;

			if (await _repository.UpdateOrganization(organization, id))
				return NoContent();
			else
				return NotFound();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteOrganization(string id)
		{
			if (await _repository.DeleteOrganization(id))
				return NoContent();
			else
				return NotFound();
		}

	}
}
