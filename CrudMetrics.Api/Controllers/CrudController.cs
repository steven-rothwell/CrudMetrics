using CrudMetrics.Api.Models;
using CrudMetrics.Api.Options;
using CrudMetrics.Api.Preservers;
using CrudMetrics.Api.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CrudMetrics.Api.Controllers;

[ApiController]
[Route("api")]
public class CrudController : BaseApiController
{
    private readonly ILogger<CrudController> _logger;
    private readonly IValidator _validator;
    private readonly IPreserver _preserver;

    public CrudController(IOptions<ApplicationOptions> applicationOptions, ILogger<CrudController> logger, IValidator validator, IPreserver preserver)
        : base(applicationOptions)
    {
        _logger = logger;
        _validator = validator;
        _preserver = preserver;
    }

    [Route("users"), HttpPost]
    public async Task<IActionResult> CreateUserAsync(User user)
    {
        try
        {
            var validationResult = await _validator.ValidateCreateAsync(user);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Message);

            var createdModel = await _preserver.CreateAsync(user);

            return Ok(createdModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user.");
            return InternalServerError(ex);
        }
    }

    [Route("users/{id:guid}"), HttpGet]
    public async Task<IActionResult> ReadUserAsync(Guid id)
    {
        try
        {
            var user = await _preserver.ReadUserAsync(id);

            if (user is null)
                return NotFound("User not found.");

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading user.");
            return InternalServerError(ex);
        }
    }

    [Route("users"), HttpGet]
    public async Task<IActionResult> ReadUserAsync(String name)
    {
        try
        {
            var user = await _preserver.ReadUserAsync(name);

            if (user is null)
                return NotFound("User not found.");

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading user.");
            return InternalServerError(ex);
        }
    }

    [Route("users/{id:guid}"), HttpPut]
    public async Task<IActionResult> UpdateUserAsync(Guid id, User user)
    {
        try
        {
            var validationResult = await _validator.ValidateUpdateAsync(id, user);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Message);

            var updatedModel = await _preserver.UpdateAsync(id, user);

            if (updatedModel is null)
                return NotFound("User not found.");

            return Ok(updatedModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user.");
            return InternalServerError(ex);
        }
    }

    [Route("users/{id:guid}"), HttpPatch]
    public async Task<IActionResult> PartialUpdateUserAsync(Guid id, User user)
    {
        try
        {
            var validationResult = await _validator.ValidatePartialUpdateAsync(user);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Message);

            var updatedModel = await _preserver.PartialUpdateAsync(id, user);

            if (updatedModel is null)
                return NotFound("User not found.");

            return Ok(updatedModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error partially updating user.");
            return InternalServerError(ex);
        }
    }

    [Route("users"), HttpPatch]
    public async Task<IActionResult> PartialUpdateUserAsync(User user, String name)
    {
        try
        {
            var validationResult = await _validator.ValidatePartialUpdateAsync(user);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Message);

            var updatedModel = await _preserver.PartialUpdateAsync(user, name);

            if (updatedModel is null)
                return NotFound("User not found.");

            return Ok(updatedModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error partially updating user.");
            return InternalServerError(ex);
        }
    }

    [Route("users/{id:guid}"), HttpDelete]
    public async Task<IActionResult> DeleteUserAsync(Guid id)
    {
        try
        {
            var deletedCount = await _preserver.DeleteUserAsync(id);

            if (deletedCount == 0)
                return NotFound("User not found.");

            return Ok(deletedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting user.");
            return InternalServerError(ex);
        }
    }
}
