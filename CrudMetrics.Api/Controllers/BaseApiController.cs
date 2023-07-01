using CrudMetrics.Api.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CrudMetrics.Api.Controllers
{
    public abstract class BaseApiController : ControllerBase
    {
        protected readonly ApplicationOptions _applicationOptions;

        public BaseApiController(IOptions<ApplicationOptions> applicationOptions)
        {
            _applicationOptions = applicationOptions.Value;
        }

        protected virtual IActionResult InternalServerError()
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        protected virtual IActionResult InternalServerError(Exception exception)
        {
            if (_applicationOptions.ShowExceptions)
                return StatusCode(StatusCodes.Status500InternalServerError, exception.ToString());

            return InternalServerError();
        }
    }
}