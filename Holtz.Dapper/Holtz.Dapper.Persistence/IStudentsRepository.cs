using Holtz.Dapper.Domain.Entities;

namespace Holtz.Dapper.Persistence
{
    public interface IStudentsRepository
    {
        Task<IEnumerable<Student>> GetAllAsync();
    }
}
