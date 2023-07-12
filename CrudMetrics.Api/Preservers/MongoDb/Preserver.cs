using CrudMetrics.Api.Models;
using CrudMetrics.Api.Options;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
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

        public async Task<User> ReadUserAsync(Guid id)
        {
            var dbClient = new MongoClient(_mongoDbOptions.ConnectionString);
            var database = dbClient.GetDatabase(_mongoDbOptions.DatabaseName);

            var collection = database.GetCollection<User>("users");
            var builder = Builders<User>.Filter;
            var filter = builder.Eq(user => user.ExternalId, id);

            return await collection.Find<User>(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> ReadUsersAsync(String name)
        {
            var dbClient = new MongoClient(_mongoDbOptions.ConnectionString);
            var database = dbClient.GetDatabase(_mongoDbOptions.DatabaseName);

            var collection = database.GetCollection<User>("users");
            var builder = Builders<User>.Filter;
            var filter = builder.Eq(user => user.Name, name);

            var models = await collection.FindAsync<User>(filter);
            return await models.ToListAsync();
        }

        public async Task<User> UpdateAsync(Guid id, User user)
        {
            var dbClient = new MongoClient(_mongoDbOptions.ConnectionString);
            var database = dbClient.GetDatabase(_mongoDbOptions.DatabaseName);

            var collection = database.GetCollection<User>("users");
            var builder = Builders<User>.Filter;
            var filter = builder.Eq(user => user.ExternalId, id);

            return await collection.FindOneAndReplaceAsync<User>(filter, user, new FindOneAndReplaceOptions<User, User>
            {
                ReturnDocument = ReturnDocument.After
            });
        }

        public async Task<User> PartialUpdateAsync(Guid id, User user)
        {
            var dbClient = new MongoClient(_mongoDbOptions.ConnectionString);
            var database = dbClient.GetDatabase(_mongoDbOptions.DatabaseName);

            var collection = database.GetCollection<User>("users");
            var builder = Builders<User>.Filter;
            var filter = builder.Eq(user => user.ExternalId, id);
            var updates = new List<UpdateDefinition<User>>();
            updates.Add(Builders<User>.Update.Set(user => user.HairColor, user.HairColor));
            updates.Add(Builders<User>.Update.Set(user => user.Age, user.Age));
            var update = Builders<User>.Update.Combine(updates);

            return await collection.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<User, User>
            {
                ReturnDocument = ReturnDocument.After
            });
        }

        public async Task<Int64> PartialUpdateAsync(User user, String name)
        {
            var dbClient = new MongoClient(_mongoDbOptions.ConnectionString);
            var database = dbClient.GetDatabase(_mongoDbOptions.DatabaseName);

            var collection = database.GetCollection<User>("users");
            var builder = Builders<User>.Filter;
            var filter = builder.Eq(user => user.Name, name);
            var updates = new List<UpdateDefinition<User>>();
            updates.Add(Builders<User>.Update.Set(user => user.HairColor, user.HairColor));
            updates.Add(Builders<User>.Update.Set(user => user.Age, user.Age));
            var update = Builders<User>.Update.Combine(updates);

            var updateResult = await collection.UpdateManyAsync(filter, update);
            return updateResult.ModifiedCount;
        }

        public async Task<Int64> DeleteUserAsync(Guid id)
        {
            var dbClient = new MongoClient(_mongoDbOptions.ConnectionString);
            var database = dbClient.GetDatabase(_mongoDbOptions.DatabaseName);

            var collection = database.GetCollection<User>("users");
            var builder = Builders<User>.Filter;
            var filter = builder.Eq(user => user.ExternalId, id);

            var deleteResult = await collection.DeleteOneAsync(filter);
            return deleteResult.DeletedCount;
        }

        public async Task<Int64> DeleteUsersAsync(String name)
        {
            var dbClient = new MongoClient(_mongoDbOptions.ConnectionString);
            var database = dbClient.GetDatabase(_mongoDbOptions.DatabaseName);

            var collection = database.GetCollection<User>("users");
            var builder = Builders<User>.Filter;
            var filter = builder.Eq(user => user.Name, name);

            var deleteResult = await collection.DeleteManyAsync(filter);
            return deleteResult.DeletedCount;
        }
    }
}
