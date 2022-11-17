using CustomerAccount.API.Entities;

namespace CustomerAccount.API.Repositories.Interfaces
{
    public interface IOrganizationRepository
    {
        Task<IEnumerable<Organization>> GetOrganizations ();
        Task<Organization> GetOrganization(string id);
        Task<bool> CreateOrganization(Organization organization);
        Task<bool> UpdateOrganization(Organization organization, string id);
        Task<bool> DeleteOrganization(string id);
    }
}
