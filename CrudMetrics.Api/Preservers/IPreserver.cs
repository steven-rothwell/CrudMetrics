using CrudMetrics.Api.Models;

namespace CrudMetrics.Api.Preservers
{
    public interface IPreserver
    {
        Task<User> CreateAsync(User model);
        Task<User> ReadUserAsync(Guid id);
        Task<IEnumerable<User>> ReadUserAsync(String name);
        Task<User> UpdateAsync(Guid id, User user);
        Task<User> PartialUpdateAsync(Guid id, User user);
        Task<Int64> DeleteUserAsync(Guid id);
    }
}
