using System;
using nosisAPI.Models;

namespace nosisAPI.Repository
{
    public interface IMongoResponseNosisRepository
    {
        public void CreateClienteNosisAsync(MongoResponseNosis mongoResponseNosis);
    }
}
