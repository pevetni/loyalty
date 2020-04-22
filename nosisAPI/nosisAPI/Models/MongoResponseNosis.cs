using MongoDB.Bson;

namespace nosisAPI.Models
{
    public class MongoResponseNosis
    {
        public ObjectId Id { get; set; }
        public Contenido Contenido { get; set; }
    }
}
