using CrudMetrics.Api.Models;
using CrudMetrics.Api.Preservers;

namespace CrudMetrics.Api.Validators
{
    public class Validator : IValidator
    {
        private readonly IPreserver _preserver;

        public Validator(IPreserver preserver)
        {
            _preserver = preserver;
        }

        public async Task<ValidationResult> ValidateCreateAsync(User user)
        {
            if (user is null)
                return new ValidationResult(false, $"{nameof(User)} cannot be null.");

            var validationResult = user.ValidateDataAnnotations();
            if (!validationResult.IsValid)
                return validationResult;

            if (user.ExternalId is not null)
                return new ValidationResult(false, $"{nameof(user.ExternalId)} cannot be set on create.");

            if (String.IsNullOrWhiteSpace(user.Name))
                return new ValidationResult(false, $"{nameof(user.Name)} cannot be empty.");

            var existingUsers = await _preserver.ReadUserAsync(user.Name);
            if (existingUsers is not null && existingUsers.Any())
                return new ValidationResult(false, $"A {nameof(User)} with the {nameof(user.Name)}: '{user.Name}' already exists.");

            if (user.Age < 0)
                return new ValidationResult(false, $"{nameof(user.Age)} cannot be less than 0.");

            return new ValidationResult(true);
        }
    }
}
