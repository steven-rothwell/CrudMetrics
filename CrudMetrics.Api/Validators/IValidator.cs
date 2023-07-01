using CrudMetrics.Api.Models;

namespace CrudMetrics.Api.Validators
{
    public interface IValidator
    {
        Task<ValidationResult> ValidateCreateAsync(User user);
    }
}
