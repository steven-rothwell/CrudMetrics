using CrudMetrics.Api.Models;

namespace CrudMetrics.Api.Preservers
{
    public interface IPreserver
    {
        Task<User> CreateAsync(User model);
        Task<User> ReadUserAsync(Guid id);
        Task<IEnumerable<User>> ReadUsersAsync(String name);
        Task<User> UpdateAsync(Guid id, User user);
        Task<User> PartialUpdateAsync(Guid id, User user);
        Task<Int64> PartialUpdateAsync(User user, String name);
        Task<Int64> DeleteUserAsync(Guid id);
        Task<Int64> DeleteUsersAsync(String name);
    }
}
