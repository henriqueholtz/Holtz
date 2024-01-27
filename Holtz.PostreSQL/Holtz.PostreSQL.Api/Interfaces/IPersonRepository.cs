using Holtz.PostreSQL.Api.Models;

namespace Holtz.PostreSQL.Api.Interfaces
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetAllAsync();
        Task<Person?> GetByIdAsync(int id);
        Task<Person> CreateAsync(Person entity);
    }
}
