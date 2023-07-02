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

        public Task<ValidationResult> ValidateUpdateAsync(Guid id, User user)
        {
            var validationResult = user.ValidateDataAnnotations();
            if (!validationResult.IsValid)
                return Task.FromResult(validationResult);

            if (id == Guid.Empty)
                return Task.FromResult(new ValidationResult(false, "Id cannot be empty."));

            if (user is null)
                return Task.FromResult(new ValidationResult(false, $"{nameof(User)} cannot be null."));

            if (id != user.ExternalId)
                return Task.FromResult(new ValidationResult(false, $"{nameof(user.ExternalId)} cannot be altered."));

            if (String.IsNullOrWhiteSpace(user.Name))
                return Task.FromResult(new ValidationResult(false, $"{nameof(user.Name)} cannot be empty."));

            if (user.Age < 0)
                return Task.FromResult(new ValidationResult(false, $"{nameof(user.Age)} cannot be less than zero."));

            return Task.FromResult(new ValidationResult(true));
        }

        public Task<ValidationResult> ValidatePartialUpdateAsync(User user)
        {
            if (String.IsNullOrWhiteSpace(user.Name))
                return Task.FromResult(new ValidationResult(false, $"{nameof(user.Name)} cannot be empty."));

            if (user.Age < 0)
                return Task.FromResult(new ValidationResult(false, $"{nameof(user.Age)} cannot be less than zero."));

            return Task.FromResult(new ValidationResult(true));
        }
    }
}
