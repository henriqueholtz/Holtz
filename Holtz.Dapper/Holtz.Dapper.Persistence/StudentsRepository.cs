using Dapper;
using Holtz.Dapper.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Holtz.Dapper.Persistence
{
    public class StudentsRepository : IStudentsRepository
    {
        private readonly string? _connectionString;
        public StudentsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Holtz.Dapper");
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                const string sql = "SELECT * FROM Students WHERE IsActive = 1";
                IEnumerable<Student> students = await sqlConnection.QueryAsync<Student>(sql);
                return students;
            }
        }
    }
}