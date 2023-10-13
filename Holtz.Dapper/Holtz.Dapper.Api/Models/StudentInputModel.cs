namespace Holtz.Dapper.Api.Models
{
    public class StudentInputModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
