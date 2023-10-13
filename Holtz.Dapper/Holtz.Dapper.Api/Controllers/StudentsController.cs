using Holtz.Dapper.Api.Models;
using Holtz.Dapper.Domain.Entities;
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
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            return Ok(await _studentsRepository.GetByIdAsync(id));
        }
        
        [HttpPost]
        public async Task<IActionResult> InsertAsync(StudentInputModel model)
        {
            Student student = new Student
            {
                BirthDate = model.BirthDate,
                FullName = model.FullName,
                Id = model.Id,
                IsActive = model.IsActive,
            };
            await _studentsRepository.InsertAsync(student);
            return Ok();
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, StudentInputModel model)
        {
            Student student = new Student
            {
                BirthDate = model.BirthDate,
                FullName = model.FullName,
                Id = id,
                IsActive = model.IsActive,
            };
            await _studentsRepository.UpdateAsync(student);
            return Ok();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            await _studentsRepository.RemoveAsync(id);
            return Ok();
        }
    }
}
