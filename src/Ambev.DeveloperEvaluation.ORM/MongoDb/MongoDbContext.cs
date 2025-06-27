using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.MongoDb;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var mongoUrl = new MongoUrl(settings.Value.MongoDbConnection);
        var client = new MongoClient(mongoUrl);
        _database = client.GetDatabase(mongoUrl.DatabaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return _database.GetCollection<T>(name);
    }

    public IMongoCollection<Cart> Carts => _database.GetCollection<Cart>("Carts");
}