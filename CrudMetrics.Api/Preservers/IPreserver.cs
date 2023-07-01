using CrudMetrics.Api.Models;

namespace CrudMetrics.Api.Preservers
{
    public interface IPreserver
    {
        Task<User> CreateAsync(User model);
        Task<IEnumerable<User>> ReadUserAsync(String name);
    }
}
