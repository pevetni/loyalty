using System;
using System.Configuration;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using nosisAPI.Models;

namespace nosisAPI.Repository
{
    public class MongoResponseNosisRepository : IMongoResponseNosisRepository
    {
        private MongoClient client;

        public MongoResponseNosisRepository()
        {
            client = new MongoClient(ConfigurationManager.AppSettings["mongoConnectionString"]);
        }

        public void CreateClienteNosisAsync(MongoResponseNosis mongoResponseNosis)
        {
            IMongoDatabase database = client.GetDatabase("nosis");
            IMongoCollection<MongoResponseNosis> collection = database.GetCollection<MongoResponseNosis>("clientes_nosis");
            //FilterDefinition<MongoResponseNosis> filter = Builders<MongoResponseNosis>.Filter.Eq("Documento", mongoResponseNosis.Contenido.Pedido.Documento);
            //UpdateDefinition<MongoResponseNosis> update = Builders<MongoResponseNosis>.Update.Set("Contenido", mongoResponseNosis.Contenido);
            //UpdateResult ur = collection.UpdateOne(filter, update);
            //if (ur.UpsertedId==null) {
                collection.InsertOneAsync(mongoResponseNosis);
            //}
        }
    }
}
