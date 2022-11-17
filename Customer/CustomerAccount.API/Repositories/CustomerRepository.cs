using Dapper;
using CustomerAccount.API.Entities;
using CustomerAccount.API.Repositories.Interfaces;
using Npgsql;

namespace CustomerAccount.API.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IConfiguration _configuration;

        public CustomerRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var customers = await connection.QueryAsync<Customer>
                ("SELECT * FROM Customers");

            return customers;
        }

        public async Task<Customer> GetCustomer(string id)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var customer = await connection.QueryFirstOrDefaultAsync<Customer>
                ("SELECT * FROM Customers WHERE Id = @Id", new { Id = id });

            return customer;
        }

        public async Task<bool> CreateCustomer(Customer customer)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected =
                await connection.ExecuteAsync
                    ("INSERT INTO Customers (Id, UserId, Email, FirstName, LastName, MobileNumber, Country, State, City) VALUES (@Id, @UserId, @Email, @FirstName, @LastName, @MobileNumber, @Country, @State, @City)",
                        new { 
                            Id = customer.Id,
                            UserId = customer.UserId,
                            Email = customer.Email,
                            FirstName = customer.FirstName,
                            LastName = customer.LastName,
                            MobileNumber = customer.MobileNumber,
                            Country = customer.Country,
                            State = customer.State,
                            City = customer.City
                        });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> UpdateCustomer(Customer customer, string id)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
                ("UPDATE Customers SET Email=@Email, FirstName=@FirstName, LastName = @LastName, MobileNumber = @MobileNumber, Country = @Country, State = @State, City = @City WHERE Id = @Id",
                    new {
                        Id = id,
                        Email = customer.Email,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        MobileNumber = customer.MobileNumber,
                        Country = customer.Country,
                        State = customer.State,
                        City = customer.City
                    });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> DeleteCustomer(string id)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync("DELETE FROM Customers WHERE Id = @id",
                new { Id = id });

            if (affected == 0)
                return false;

            return true;
        }
    }
}
