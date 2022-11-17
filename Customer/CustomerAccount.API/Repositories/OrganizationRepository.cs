using Dapper;
using CustomerAccount.API.Entities;
using CustomerAccount.API.Repositories.Interfaces;
using Npgsql;

namespace CustomerAccount.API.Repositories
{
	public class OrganizationRepository : IOrganizationRepository
	{
		private readonly IConfiguration _configuration;

		public OrganizationRepository(IConfiguration configuration)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		public async Task<IEnumerable<Organization>> GetOrganizations()
		{
			using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var organizations = await connection.QueryAsync<Organization>
					("SELECT * FROM Organizations");

			return organizations;
		}

		public async Task<Organization> GetOrganization(string id)
		{
			using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var organization = await connection.QueryFirstOrDefaultAsync<Organization>
					("SELECT * FROM Organizations WHERE Id = @Id", new { Id = id });

			return organization;
		}

		public async Task<Organization> GetOrganizationByEmail(string email)
		{
			using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var organization = await connection.QueryFirstOrDefaultAsync<Organization>
					("SELECT * FROM Organizations WHERE Email = @Email", new { Email = email });

			return organization;
		}

		public async Task<bool> CreateOrganization(Organization organization)
		{
			using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var affected =
					await connection.ExecuteAsync
							("INSERT INTO Organizations (Id, CustomerId, CompanyName, Country, State, City, Address, MobileNumber, Landline, Industry, Language, CompanySize) VALUES (@Id, @CustomerId, @CompanyName, @Country, @State, @City, @Address, @MobileNumber, @Landline, @Industry, @Language, @CompanySize)",
									new { 
										Id = organization.Id,
										CustomerId = organization.CustomerId,
										CompanyName = organization.CompanyName,
										Country = organization.Country,
										State = organization.State,
										City = organization.City,
										Address = organization.Address,
										MobileNumber = organization.MobileNumber,
										Landline = organization.Landline,
										Industry = organization.Industry,
										Language = organization.Language,
										CompanySize = organization.CompanySize
									});

			if (affected == 0)
				return false;

			return true;
		}

		public async Task<bool> UpdateOrganization(Organization organization, string id)
		{
			using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var affected = await connection.ExecuteAsync
					("UPDATE Organizations SET CustomerId = @CustomerId, CompanyName = @CompanyName, Country = @Country, State = @State, City = @City, Address = @Address, MobileNumber = @MobileNumber, Landline = @Landline, Industry = @Industry, Language = @Language, CompanySize = @CompanySize WHERE Id = @Id",
							new { 
								Id = organization.Id,
								CompanyName = organization.CompanyName,
								Country = organization.Country,
								State = organization.State,
								City = organization.City,
								Address = organization.Address,
								MobileNumber = organization.MobileNumber,
								Landline = organization.Landline,
								Industry = organization.Industry,
								Language = organization.Language,
								CompanySize = organization.CompanySize
							});

			if (affected == 0)
				return false;

			return true;
		}

		public async Task<bool> DeleteOrganization(string id)
		{
			using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var affected = await connection.ExecuteAsync("DELETE FROM Organizations WHERE Id = @id",
					new { Id = id });

			if (affected == 0)
				return false;

			return true;
		}
	}
}
