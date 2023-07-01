using CrudMetrics.Api.Models;
using CrudMetrics.Api.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CrudMetrics.Api.Preservers.MongoDb
{
    public class Preserver : IPreserver
    {
        private readonly MongoDbOptions _mongoDbOptions;
        private readonly MongoCollectionSettings _mongoCollectionSettings;

        public Preserver(IOptions<MongoDbOptions> mongoDbOptions)
        {
            _mongoDbOptions = mongoDbOptions.Value;
            _mongoCollectionSettings = new MongoCollectionSettings
            {
                AssignIdOnInsert = true
            };
        }

        public async Task<User> CreateAsync(User user)
        {
            if (user is null)
                throw new Exception("Cannot create because model is null.");

            user.ExternalId = Guid.NewGuid();

            var dbClient = new MongoClient(_mongoDbOptions.ConnectionString);
            var database = dbClient.GetDatabase(_mongoDbOptions.DatabaseName);

            var collection = database.GetCollection<User>("users", _mongoCollectionSettings);

            await collection.InsertOneAsync(user);

            return user;
        }

        public async Task<IEnumerable<User>> ReadUserAsync(String name)
        {
            var dbClient = new MongoClient(_mongoDbOptions.ConnectionString);
            var database = dbClient.GetDatabase(_mongoDbOptions.DatabaseName);

            var collection = database.GetCollection<User>("users");
            var builder = Builders<User>.Filter;
            var filter = builder.Eq(user => user.Name, name);

            var models = await collection.FindAsync<User>(filter);
            return await models.ToListAsync();
        }
    }
}
