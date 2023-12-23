using KudinovoBot.DAL.Configs;
using KudinovoBot.DAL.Models;
using MongoDB.Driver;

namespace KudinovoBot.DAL.Repositories
{
    public class WorkRepository
    {
        private readonly Config _config;

        private readonly string _connectionString;
        private readonly string _databaseName;
        private readonly string _collectionName;

        public WorkRepository(Config config)
        {
            _config = config;

            _connectionString = _config.Mongo.ConnectionString;
            _databaseName = _config.Mongo.DatabaseName;
            _collectionName = "works";
        }

        private IMongoCollection<T> ConnectToMongo<T>(in string collection)
        {
            var client = new MongoClient(_connectionString);
            var db = client.GetDatabase(_databaseName);
            return db.GetCollection<T>(collection);
        }

        public async Task CreateAsync(Work item)
        {
            var collection = ConnectToMongo<Work>(_collectionName);
            await collection.InsertOneAsync(item);
        }

        public async Task<Work> GetById(Guid id)
        {
            var collection = ConnectToMongo<Work>(_collectionName);
            return await collection.Find(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Work> GetByAuthorId(long id)
        {
            var collection = ConnectToMongo<Work>(_collectionName);
            return await collection.Find(i => i.AuthorId == id).FirstOrDefaultAsync();
        }

        public async Task<List<Work>> GetAllAsync()
        {
            var collection = ConnectToMongo<Work>(_collectionName);
            return await collection.Find(_ => true).ToListAsync();
        }

        public async Task UpdateAsync(Work item)
        {
            var collection = ConnectToMongo<Work>(_collectionName);

            var filter = Builders<Work>.Filter.Eq(i => i.Id, item.Id);
            await collection.ReplaceOneAsync(filter, item);
        }

        public async Task DeleteById(Guid id)
        {
            var collection = ConnectToMongo<Work>(_collectionName);

            var filter = Builders<Work>.Filter.Eq(i => i.Id, id);
            await collection.DeleteOneAsync(filter);
        }
    }
}
