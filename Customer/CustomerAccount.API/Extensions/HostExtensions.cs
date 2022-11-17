using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using Polly;
using System;

namespace CustomerAccount.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();

                try
                {
                    logger.LogInformation("Migrating postresql database.");

                    var retry = Policy.Handle<NpgsqlException>()
                            .WaitAndRetry(
                                retryCount: 5,
                                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // 2,4,8,16,32 sc
                                onRetry: (exception, retryCount, context) =>
                                {
                                    logger.LogError($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
                                });

                    //if the postgresql server container is not created on run docker compose this
                    //migration can't fail for network related exception. The retry options for database operations
                    //apply to transient exceptions                    
                    retry.Execute(() => ExecuteMigrations(configuration));
                    
                    logger.LogInformation("Migrated postresql database.");
                }
                catch (NpgsqlException ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the postresql database");
                }
            }

            return host;
        }

        private static void ExecuteMigrations(IConfiguration configuration)
        {
            using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            connection.Open();

            using var command = new NpgsqlCommand
            {
                Connection = connection
            };

            command.CommandText = "DROP TABLE IF EXISTS Customers";
            command.ExecuteNonQuery();

            command.CommandText = @"CREATE TABLE Customers(Id SERIAL PRIMARY KEY, 
                                UserId VARCHAR(64) NOT NULL,
                                Email VARCHAR(128) NOT NULL,
                                FirstName VARCHAR(128) NOT NULL,
                                LastName VARCHAR(128) NOT NULL,
                                MobileNumber VARCHAR(32) NOT NULL,
                                Country VARCHAR(128) NOT NULL,
                                City VARCHAR(128) NOT NULL)";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO Customers(Email, FirstName, LastName, Country, City) VALUES('admin.moonshot@gmail.com', 'Admin', 'Moonshot', 'PH', 'MC');";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO Customers(Email, FirstName, LastName, Country, City) VALUES('vash.moonshot@gmail.com', 'Vash', 'Salarda', 'PH', 'MC');";
            command.ExecuteNonQuery();
        }        
    }
}
