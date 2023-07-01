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
            _logger.LogError(ex, $"Error creating user.");
            return InternalServerError(ex);
        }
    }
}