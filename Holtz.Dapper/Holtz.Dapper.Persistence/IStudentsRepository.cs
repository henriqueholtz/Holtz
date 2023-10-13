using Holtz.Dapper.Domain.Entities;

namespace Holtz.Dapper.Persistence
{
    public interface IStudentsRepository
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student> GetByIdAsync(int id);
        Task InsertAsync(Student student);
        Task UpdateAsync(Student student);
        Task RemoveAsync(int id);
    }
}
