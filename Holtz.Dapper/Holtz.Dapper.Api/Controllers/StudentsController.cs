using Holtz.Dapper.Api.Models;
using Holtz.Dapper.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Holtz.Dapper.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentsRepository _studentsRepository;
        public StudentsController(IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _studentsRepository.GetAllAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Insert(StudentInputModel model)
        {
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id)
        {
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            return Ok();
        }
    }
}
