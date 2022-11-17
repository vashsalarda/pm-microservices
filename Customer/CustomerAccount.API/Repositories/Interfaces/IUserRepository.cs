using CustomerAccount.API.Entities;

namespace CustomerAccount.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers ();
        Task<User> GetUser(string id);
        Task<User> GetUserByEmail(string email);
        Task<bool> CreateUser(User user);
        Task<bool> UpdateUser(User user, string id);
        Task<bool> DeleteUser(string id);
    }
}
