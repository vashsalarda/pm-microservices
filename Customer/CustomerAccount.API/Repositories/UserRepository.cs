using Dapper;
using CustomerAccount.API.Entities;
using CustomerAccount.API.Repositories.Interfaces;
using Npgsql;

namespace CustomerAccount.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var users = await connection.QueryAsync<User>
                ("SELECT * FROM Users");

            return users;
        }

		public async Task<User> GetUser(string id)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var user = await connection.QueryFirstOrDefaultAsync<User>
                ("SELECT * FROM Users WHERE Id = @Id", new { Id = id });

            return user;
        }

		public async Task<User> GetUserByEmail(string email)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var user = await connection.QueryFirstOrDefaultAsync<User>
                ("SELECT * FROM Users WHERE Email = @Email", new { Email = email });

            return user;
        }

        public async Task<bool> CreateUser(User user)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected =
                await connection.ExecuteAsync
                    ("INSERT INTO Users (Id, Email, FirstName, LastName, MobileNumber, Country, State, City, Role, Designation, Password) VALUES (@Id, @Email, @FirstName, @LastName, @MobileNumber, @Country, @State, @City, @Role, @Designation, @Password)",
                        new { 
                            Id = user.Id,
                            Email = user.Email,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            MobileNumber = user.MobileNumber,
                            Country = user.Country,
                            State = user.State,
                            City = user.City,
                            Role = user.Role,
                            Designation = user.Designation,
                            Password = user.Password
                        });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> UpdateUser(User user, string id)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
                ("UPDATE Users SET Email = @Email, FirstName = @FirstName, LastName = @LastName, MobileNumber = @MobileNumber, Country = @Country, State = @State, City = @City, Role = @Role, Designation = @Designation WHERE Id = @Id",
                    new {
                        Id = id,
                        Email = user.Email,
                        LastName = user.LastName,
                        FirstName = user.FirstName,
                        MobileNumber = user.MobileNumber,
                        Country = user.Country,
                        State = user.State,
                        City = user.City,
                        Role = user.Role,
                        Designation = user.Designation
                    });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> DeleteUser(string id)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync("DELETE FROM Users WHERE Id = @id",
                new { Id = id });

            if (affected == 0)
                return false;

            return true;
        }
    }
}
