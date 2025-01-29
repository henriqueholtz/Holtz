using System.Net;
using Holtz.ElasticSearch.Api.Models;
using Holtz.ElasticSearch.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Holtz.ElasticSearch.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IElasticSearch _elasticSearch;

    public UsersController(ILogger<UsersController> logger, IElasticSearch elasticSearch)
    {
        _logger = logger;
        _elasticSearch = elasticSearch;
    }

    [HttpPost("create-index")]
    public async Task<IActionResult> CreateIndexAsync(string indexName, CancellationToken cancellationToken)
    {
        await _elasticSearch.CreateIndexIfNotExistsAsync(indexName, cancellationToken);
        return Ok($"Index {indexName} created or already exists.");
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserAsync([FromBody] User user, CancellationToken cancellationToken)
    {
        var result = await _elasticSearch.AddOrUpdate(user, cancellationToken);
        if (result)
            return Ok($"User {user.FirstName} created or updated successfully.");

        return StatusCode((int)HttpStatusCode.InternalServerError, "Error adding or updating a user.");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUserAsync([FromBody] User user, CancellationToken cancellationToken)
    {
        var result = await _elasticSearch.AddOrUpdate(user, cancellationToken);
        if (result)
            return Ok($"User {user.FirstName} created or updated successfully.");

        return StatusCode((int)HttpStatusCode.InternalServerError, "Error adding or updating a user.");
    }

    [HttpGet("{key}")]
    public async Task<IActionResult> GetUserAsync(string key, CancellationToken cancellationToken)
    {
        var user = await _elasticSearch.GetAsync(key, cancellationToken);
        if (user != null)
            return Ok(user);

        return NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> GetUsersAsync(CancellationToken cancellationToken)
    {
        var users = await _elasticSearch.GetAllAsync(cancellationToken);
        if (users != null)
            return Ok(users);

        return StatusCode((int)HttpStatusCode.InternalServerError, "Error retrieving users.");
    }

    [HttpDelete("{key}")]
    public async Task<IActionResult> DeleteUserAsync(string key, CancellationToken cancellationToken)
    {
        var result = await _elasticSearch.DeleteAsync(key, cancellationToken);
        if (result)
            return Ok("User deleted successfully.");

        return StatusCode((int)HttpStatusCode.InternalServerError, "Error deleting users.");
    }
}

