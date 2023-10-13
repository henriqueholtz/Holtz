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

        private string GetStatusSqlAsString(bool isActive)
        {
            return isActive ? "1" : "0";
        }
        private string GetBirthDateSqlAsString(DateTime? birthDate)
        {
            if (birthDate == null) return "null";

            return $"'{birthDate}'";
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string sql = $"SELECT * FROM Students WHERE IsActive = {GetStatusSqlAsString(true)}";
                IEnumerable<Student> students = await sqlConnection.QueryAsync<Student>(sql);
                return students;
            }
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string sql = $"SELECT TOP 1 * FROM Students WHERE Id = {id}";
                Student student = await sqlConnection.QueryFirstAsync<Student>(sql);
                return student;
            }
        }

        public async Task InsertAsync(Student student)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string sql = $"INSERT INTO Students VALUES('{student.FullName}', {GetStatusSqlAsString(student.IsActive)}, " +
                    $"{GetBirthDateSqlAsString(student.BirthDate)})";
                await sqlConnection.ExecuteAsync(sql);
            }
        }

        public async Task RemoveAsync(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string sql = $"DELETE Students WHERE Id = {id}";
                await sqlConnection.ExecuteAsync(sql);
            }
        }

        public async Task UpdateAsync(Student student)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string sql = $"UPDATE Students Set FullName = '{student.FullName}', IsActive = {GetStatusSqlAsString(student.IsActive)}, " +
                    $"BirthDate = {GetBirthDateSqlAsString(student.BirthDate)}" +
                    $" WHERE Id = {student.Id}";
                await sqlConnection.ExecuteAsync(sql);
            }
        }
    }
}