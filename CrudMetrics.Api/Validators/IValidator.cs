using CrudMetrics.Api.Models;

namespace CrudMetrics.Api.Validators
{
    public interface IValidator
    {
        Task<ValidationResult> ValidateCreateAsync(User user);
        Task<ValidationResult> ValidateUpdateAsync(Guid id, User user);
        Task<ValidationResult> ValidatePartialUpdateAsync(User user);
    }
}
