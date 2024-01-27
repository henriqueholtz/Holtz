using Holtz.PostreSQL.Api.Interfaces;
using Holtz.PostreSQL.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Holtz.PostreSQL.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController (IPersonRepository personRepository) : ControllerBase
    {
        private readonly IPersonRepository _personRepository = personRepository;

        [HttpGet]
        public async Task<IActionResult> GetPersonsAsync()
        {
            return Ok(await _personRepository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            return Ok(await _personRepository.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Person person)
        {
            await _personRepository.CreateAsync(person);
            return Created();
        }
    }
}
