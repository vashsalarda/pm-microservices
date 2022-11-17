using CustomerAccount.API.Entities;

namespace CustomerAccount.API.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetCustomers();
        Task<Customer> GetCustomer(string id);
        Task<bool> CreateCustomer(Customer customer);
        Task<bool> UpdateCustomer(Customer customer, string id);
        Task<bool> DeleteCustomer(string id);
    }
}
