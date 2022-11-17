using Microsoft.EntityFrameworkCore;
using CustomerAccount.API.Entities;

namespace CustomerAccount.API.Data
{
	public class CustomerAccountContext : DbContext
	{
		protected readonly IConfiguration _configuration;

		public CustomerAccountContext(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public DbSet<User> Users { get; set; } = null!;
		public DbSet<Customer> Customers { get; set; } = null!;
		public DbSet<Organization> Organizations { get; set; } = null!;
		public DbSet<Role> Roles { get; set; } = null!;

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Customer>(e =>
			{
				e.Property(e => e.Id).HasColumnName("id");
				e.Property(e => e.UserId).HasColumnName("userid");
				e.Property(e => e.Email).HasColumnName("email");
				e.Property(e => e.FirstName).HasColumnName("firstname");
				e.Property(e => e.LastName).HasColumnName("lastname");
				e.Property(e => e.MobileNumber).HasColumnName("mobilenumber");
				e.Property(e => e.Country).HasColumnName("country");
				e.Property(e => e.State).HasColumnName("state");
				e.Property(e => e.City).HasColumnName("city");
				e.ToTable("customers")
					.HasIndex(x => x.Email)
					.IsUnique();
			});

			modelBuilder.Entity<User>(e =>
			{
				e.Property(e => e.Id).HasColumnName("id");
				e.Property(e => e.Email).HasColumnName("email");
				e.Property(e => e.FirstName).HasColumnName("firstname");
				e.Property(e => e.LastName).HasColumnName("lastname");
				e.Property(e => e.MobileNumber).HasColumnName("mobilenumber");
				e.Property(e => e.Country).HasColumnName("country");
				e.Property(e => e.State).HasColumnName("state");
				e.Property(e => e.City).HasColumnName("city");
				e.Property(e => e.Role).HasColumnName("role");
				e.Property(e => e.Designation).HasColumnName("designation");
				e.Property(e => e.Password).HasColumnName("password");
				e.ToTable("users")
					.HasIndex(x => x.Email)
					.IsUnique();
			});

			modelBuilder.Entity<Organization>(e =>
			{
				e.Property(e => e.Id).HasColumnName("id");
				e.Property(e => e.CustomerId).HasColumnName("customerid");
				e.Property(e => e.CompanyName).HasColumnName("companyname");
				e.Property(e => e.Country).HasColumnName("country");
				e.Property(e => e.State).HasColumnName("state");
				e.Property(e => e.City).HasColumnName("city");
				e.Property(e => e.Address).HasColumnName("address");
				e.Property(e => e.MobileNumber).HasColumnName("mobilenumber");
				e.Property(e => e.Landline).HasColumnName("landline");
				e.Property(e => e.Industry).HasColumnName("industry");
				e.Property(e => e.Language).HasColumnName("language");
				e.Property(e => e.CompanySize).HasColumnName("companysize");
				e.ToTable("organizations")
					.HasIndex(x => x.CompanyName)
					.IsUnique();
			});

			modelBuilder.Entity<Role>(e =>
			{
				e.Property(e => e.Id).HasColumnName("id");
				e.Property(e => e.Name).HasColumnName("name");
				e.Property(e => e.Key).HasColumnName("key");
				e.ToTable("roles");
			});
			
			base.OnModelCreating(modelBuilder);
		}
	}
}
