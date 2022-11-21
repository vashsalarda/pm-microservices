using CustomerAccount.API.Data;
using CustomerAccount.API.Repositories;
using CustomerAccount.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
// using HealthChecks.UI.Client;
// using Microsoft.AspNetCore.Diagnostics.HealthChecks;
// using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddDbContext<CustomerAccountContext>();

builder.Services.AddHealthChecks()
		.AddNpgSql(builder.Configuration["DatabaseSettings:ConnectionString"]);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<CustomerAccountContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
app.UseSwagger();

app.UseSwaggerUI();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();