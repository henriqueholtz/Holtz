using Holtz.PostreSQL.Api.Context;
using Holtz.PostreSQL.Api.Interfaces;
using Holtz.PostreSQL.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Holtz.PostreSQL.Api.Repositories
{
    public class PersonRepository (HoltzPostgreSqlContext context) : IPersonRepository
    {
        private readonly HoltzPostgreSqlContext _context = context;

        public async Task<Person> CreateAsync(Person entity)
        {
            _context.People.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await _context.People.ToListAsync();
        }

        public async Task<Person?> GetByIdAsync(int id)
        {
            return await _context.People.FirstOrDefaultAsync(p => p.Id.Equals(id));
        }
    }
}
